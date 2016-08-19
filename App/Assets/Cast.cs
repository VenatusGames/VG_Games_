using UnityEngine;
using System.Collections;
using System.Linq;
public class Cast : MonoBehaviour {
	GameObject currentLure;
	public GameObject floatPrefab;
	public Transform instObj;
	public Vector3 beginPos,endPos;

	public int maxPower;

	Coroutine curReelCoroutine;

	public bool canCast;
	public bool castStarted;
	public bool reelStarted;
	private float startTime;
	public float lastSpeed;
	public float angle;

	public Animator poleAnim;

	void Update(){
		if(Application.isMobilePlatform){
			if(Input.touches.Length > 1 || Input.touches.Length < 1) return;

			switch (Input.touches[0].phase) {
				
			case TouchPhase.Began:
				if(canCast){
					beginPos = Input.touches[0].position;
					startTime = Time.time;
					castStarted = true;
				}else{
					beginPos = Input.touches[0].position;
					castStarted = false;
				}
				break;
			case TouchPhase.Stationary:
				if(castStarted){
					beginPos = Input.touches[0].position;
					startTime = Time.time;
				}
				break;
			case TouchPhase.Ended:
				if(castStarted){
					if(beginPos.y < Input.touches[0].position.y){
						poleAnim.SetBool("cast",true);
						StartCoroutine(StartCast());
					}
				}
				else
					StartReel();
				break;

			default:
				break;
			}
		}else{
			if(Input.GetMouseButtonDown(0)){
				if(canCast){
					beginPos = Input.mousePosition;
					startTime = Time.time;
					castStarted = true;
				}else{
					beginPos = Input.mousePosition;
					castStarted = false;
				}
			}
			if(Input.GetMouseButtonUp(0)){
				if(castStarted){
					if(beginPos.y < Input.mousePosition.y){
						poleAnim.SetBool("cast",true);
						StartCoroutine(StartCast());
					}
				}
				else
					StartReel();
			}
		}
	}

	IEnumerator StartCast(){
		if(curReelCoroutine != null)
			StopCoroutine(curReelCoroutine);
		poleAnim.SetBool("reel",false);
		if(castStarted){
			if(Application.isMobilePlatform){
			endPos = Input.touches[0].position;
				if(endPos.y > beginPos.y){
					float timeToRelease = Time.time - startTime;
					lastSpeed = Mathf.Clamp(Vector3.Distance(beginPos,endPos) / timeToRelease,0,maxPower);
					lastSpeed = (lastSpeed / maxPower) * 100;
					Vector2 swipe = beginPos - endPos;
					angle = Vector2.Angle(Vector2.right,swipe);
					angle = Mathf.Clamp(Vector2.Angle(Vector2.right,swipe),45,135);
					yield return new WaitForSeconds(0.4f);
					ThrowFloat(angle,Mathf.RoundToInt(lastSpeed));
				}
			}else{
				endPos = Input.mousePosition;
				if(endPos.y > beginPos.y){
					float timeToRelease = Time.time - startTime;
					lastSpeed = Mathf.Clamp(Vector3.Distance(beginPos,endPos) / timeToRelease,0,maxPower);
					lastSpeed = (lastSpeed / maxPower) * 100;
					Vector2 swipe = beginPos - endPos;
					angle = Mathf.Clamp(Vector2.Angle(Vector2.right,swipe),45,135);
					yield return new WaitForSeconds(0.4f);
					ThrowFloat(angle,Mathf.RoundToInt(lastSpeed));
				}
			}
		}
		beginPos = Vector3.zero;
		endPos = Vector3.zero;
		castStarted = false;
	}

	void ThrowFloat(float angle, int power){
		power = Mathf.Clamp(power,10,90);
		Destroy(currentLure);
		currentLure = (GameObject)Instantiate(floatPrefab,instObj.position,floatPrefab.transform.rotation);
		ConfigurableJoint joint = instObj.GetComponent<ConfigurableJoint>();
		Rigidbody lureRigid = currentLure.GetComponent<Rigidbody>();
		joint.connectedBody = lureRigid;
		currentLure.transform.Rotate(new Vector3(0,angle,0));
		lureRigid.AddRelativeForce(new Vector3(0,power*2,-power * 3));
		FindObjectOfType<RenderLine>().lure = currentLure;
		canCast = false;
		Invoke("AttractFish",2);
	}

	void StartReel(){
		if(Application.isMobilePlatform){
			endPos = Input.touches[0].position;
			if(endPos.y < beginPos.y){
				poleAnim.SetBool("reel",true);
				poleAnim.SetBool("cast",false);
				CancelInvoke();
				//TODO: Replace with catching mechanic
				foreach (var item in GameObject.FindGameObjectsWithTag("Fish")) {
					if(Vector2.Distance(new Vector2(item.transform.position.x,item.transform.position.z),new Vector2(currentLure.transform.position.x,currentLure.transform.position.z)) < 0.5f){
						item.GetComponent<Fish>().bobber = currentLure;
						item.GetComponent<Fish>().PrepareFishToReel();
						break;
					}
				}
				curReelCoroutine = StartCoroutine(ReelIn());
				canCast = true;
				GameObject[] fish = GameObject.FindGameObjectsWithTag("Fish");
				foreach (var item in fish) {
					item.GetComponent<Fish>().attracted = false;
				}
			}
		}else{
			endPos = Input.mousePosition;
			if(endPos.y < beginPos.y){
				poleAnim.SetBool("cast",false);
				poleAnim.SetBool("reel",true);
				CancelInvoke();
				foreach (var item in GameObject.FindGameObjectsWithTag("Fish")) {
					if(Vector2.Distance(new Vector2(item.transform.position.x,item.transform.position.z),new Vector2(currentLure.transform.position.x,currentLure.transform.position.z)) < 0.5f){
						item.GetComponent<Fish>().bobber = currentLure;
						item.GetComponent<Fish>().PrepareFishToReel();
						break;
					}
				}
				canCast = true;
				curReelCoroutine = StartCoroutine(ReelIn());
				GameObject[] fish = GameObject.FindGameObjectsWithTag("Fish");
				foreach (var item in fish) {
					item.GetComponent<Fish>().attracted = false;
				}
			}
		}
	}

	IEnumerator ReelIn(){
		Vector3 pos = currentLure.transform.position;
		float step = 1 * Time.deltaTime;
		while(Vector3.Distance(currentLure.transform.position,new Vector3(poleAnim.gameObject.transform.position.x,1.14f,poleAnim.gameObject.transform.position.z)) > 0.1f){
			currentLure.transform.position = Vector3.MoveTowards(currentLure.transform.position,new Vector3(poleAnim.gameObject.transform.position.x,1.14f,poleAnim.gameObject.transform.position.z),step);
			yield return new WaitForEndOfFrame();
		}
		Destroy(currentLure);
	}

	void AttractFish(){
		GameObject[] fish = GameObject.FindGameObjectsWithTag("Fish");
		if(fish.Length > 0){
			fish.OrderBy(x => Vector2.Distance(currentLure.transform.position,x.transform.position)).ToArray();
			if(Random.Range(0,2)==1 || fish[0].GetComponent<Fish>().attracted){
				fish[0].GetComponent<Fish>().attracted = true;
				fish[0].GetComponent<Fish>().bobber = currentLure;
			}
			for (int i = 1; i < fish.Length; i++) {
				fish[i].GetComponent<Fish>().attracted = false;
				fish[i].GetComponent<Fish>().bobber = currentLure;
			}
		}
		Invoke("AttractFish",2);
	}

	void OnGUI(){
		GUI.Label(new Rect(0,0,200,200),"Start Pos: " +beginPos.ToString());
		GUI.Label(new Rect(0,20,200,200),"End Pos: " +endPos.ToString());
		GUI.Label(new Rect(0,40,200,200),"Speed: " +lastSpeed.ToString());
		GUI.Label(new Rect(0,60,200,200),"Start Time: " +startTime.ToString());
		GUI.Label(new Rect(0,80,200,200),"Angle in degrees: " +angle.ToString());
	}
}
