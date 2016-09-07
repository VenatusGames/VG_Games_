using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
public class Cast : MonoBehaviour {
	GameObject currentLure;
	public GameObject floatPrefab;
	public Transform instObj;
	public Vector3 beginPos,endPos;

	public int maxPower;

	Coroutine curReelCoroutine;
	Coroutine curCatchCoroutine;

	public bool canCast;
	public bool castStarted;
	public bool reelStarted;
	private float startTime;
	public float lastSpeed;
	public float angle;

	public Animator poleAnim;

	public bool catchStarted;
	public bool canCatch;
	public Fish caughtFish;
	public FishDatabase fishDatabase;

	public bool isInHotspot;

	public bool hasCasted;

	public DetectHover fishPediaButton,shopButton;
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
						if(fishPediaButton.isDown || shopButton.isDown){
							castStarted = false;
						}else{
							poleAnim.SetBool("cast",true);
							StartCoroutine(StartCast());
						}
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
						if(fishPediaButton.isDown || shopButton.isDown){
							castStarted = false;
						}else{
							poleAnim.SetBool("cast",true);
							StartCoroutine(StartCast());
						}
					}
				}
				else
					if(!reelStarted)
						StartReel();
			}
		}
	}

	void FixedUpdate(){
		if(currentLure)
		if(Physics.OverlapSphere(currentLure.transform.position,0.1f).Length > 0){
				isInHotspot = true;
			}else{
				isInHotspot = false;
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
		canCast = false;
		hasCasted = true;
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
		Invoke("StartCatchLoop",5);
	}

	void StartReel(){
		if(Application.isMobilePlatform){
			endPos = Input.touches[0].position;
			if(endPos.y < beginPos.y){
				poleAnim.SetBool("reel",true);
				poleAnim.SetBool("cast",false);
				CancelInvoke();
				curReelCoroutine = StartCoroutine(ReelIn(canCatch));
			}
		}else{
			endPos = Input.mousePosition;
			if(endPos.y < beginPos.y){
				poleAnim.SetBool("cast",false);
				poleAnim.SetBool("reel",true);
				CancelInvoke();
				curReelCoroutine = StartCoroutine(ReelIn(canCatch));
			}
		}
	}

	void StartCatchLoop(){
		if(Random.Range(0,4) == 1 && !catchStarted){
			curCatchCoroutine = StartCoroutine(CatchMinigame(fishDatabase.fish[Random.Range(0,fishDatabase.fish.Count)]));
		}
		if(!isInHotspot)
			Invoke("StartCatchLoop",Random.Range(1f,2f));
		else
			Invoke("StartCatchLoop",Random.Range(0.5f,1f));
	}

	IEnumerator CatchMinigame(Fish fish){
		catchStarted = true;
		float startTime = Time.time;
		float bobTime = Random.Range(fish.minBobTime,fish.maxBobTime);
		float catchTime = Random.Range(fish.minCatchTime,fish.maxCatchTime);
		catchTime = bobTime + catchTime;
		WaterPhysics water = FindObjectOfType<WaterPhysics>();
		float startLevel = water.waterLevel;

		while(Time.time-startTime < bobTime){
			water.waterLevel -= 0.1f;
			yield return new WaitForSeconds(Random.Range(0.2f,0.4f));
			water.waterLevel += 0.1f;
			yield return new WaitForSeconds(Random.Range(0.2f,0.4f));
		}
		while (Time.time-startTime < catchTime) {
			water.waterLevel = startLevel - 0.2f;
			canCatch = true;
			caughtFish = fish;
			yield return new WaitForEndOfFrame();
		}
		canCatch = false;
		if(!reelStarted)
			caughtFish = null;
		catchStarted = false;
		water.waterLevel = startLevel;

	}

	IEnumerator ReelIn(bool didCatch){
		if(curCatchCoroutine != null)
			StopCoroutine(curCatchCoroutine);
		FindObjectOfType<WaterPhysics>().waterLevel = 0.6f;
		reelStarted = true;
		float step = 3 * Time.deltaTime;
		while(Vector3.Distance(currentLure.transform.position,new Vector3(instObj.position.x,1.14f,instObj.gameObject.transform.position.z)) > 0.1f){
			currentLure.transform.position = Vector3.MoveTowards(currentLure.transform.position,new Vector3(instObj.gameObject.transform.position.x,1.14f,instObj.gameObject.transform.position.z),step);
			yield return new WaitForEndOfFrame();
		}
		if(didCatch){
			FindObjectOfType<PopupController>().CreatePopup(caughtFish);
		}else{
			canCast = true;
		}
		hasCasted = false;
		reelStarted = false;
		caughtFish = null;
		canCatch = false;
		catchStarted = false;
		Destroy(currentLure);
	}
}
