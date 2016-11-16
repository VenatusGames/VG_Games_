using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
public class Cast : MonoBehaviour {
	GameObject currentLure;

	//Set These In Inspector
	public GameObject floatPrefab;
	public Transform instObj;
	public int maxPower;
	public Animator poleAnim;
	public FishDatabase fishDatabase;
	public List<DetectHover> hovers;
	public float swipeStrength;

	//Casting variables
	public bool canCast;
	public bool hasCasted;

	private float startTime;
	private Vector3 beginPos,endPos;

	public bool isInHotspot;
	public BaitType currentBait;

	//Reeling Variables
	public bool isReeling;
	public GameObject reelStick;
	public float reelSpeed;
	private Vector3 fishTarget;
	public float tension;

	//Catching Variables
	public bool catchStarted;
	public Fish currentFishToCatch;

	//Coroutine Handling
	Coroutine curFightCoroutine,curCatchLoopCoroutine,curHookCoroutine;

	public float reelAnimTime = 0f;
	public GameObject reelControlPanel;

	public bool overControls;
	public bool exitedMenu;
	public GameObject tensionSlider;
	public GameObject floatDirection;
	void Update(){
		if(hasCasted){
			reelControlPanel.SetActive(true);
		}else{
			reelControlPanel.SetActive(false);
		}
		if(instObj == null){
			instObj = FindObjectOfType<ConfigurableJoint>().transform;
		}
		poleAnim.SetFloat("Blend",reelAnimTime);
		bool isHovering = false;
		foreach (var item in hovers) {
			if(item.isDown){
				isHovering = true;
			}
		}
		//Controls
		if(!isHovering && !overControls && !exitedMenu){
			if(Application.isMobilePlatform){
				//Mobile Controls
				if(Input.touches.Length == 1){
					Touch curTouch = Input.touches[0];
					if(curTouch.phase == TouchPhase.Began){
						if(canCast){
							startTime = Time.time;
							beginPos = curTouch.position;
						}
					}else if(curTouch.phase == TouchPhase.Canceled){

					}else if(curTouch.phase == TouchPhase.Ended){
						if(canCast){
							endPos = curTouch.position;
							if(beginPos.y < endPos.y){
								float timeTaken = Time.time - startTime;
								float speed = (Mathf.Clamp(Vector3.Distance(beginPos,endPos) / timeTaken,0,maxPower)/maxPower) * 100;
								float angle = Mathf.Clamp(Vector2.Angle(Vector2.right,(beginPos-endPos)),45,135);
								StartCoroutine(CastLure(speed, angle));
							}
						}
					}
				}
			}else{
				//Desktop Controls(for testing)
				if(Input.GetMouseButtonDown(0)){
					if(canCast){
						startTime = Time.time;
						beginPos = Input.mousePosition;
					}
				}else if(Input.GetMouseButtonUp(0)){
					if(canCast){
						endPos = Input.mousePosition;
						if(beginPos.y < endPos.y){
							float timeTaken = Time.time - startTime;
							float speed = (Mathf.Clamp(Vector3.Distance(beginPos,endPos) / timeTaken,0,maxPower)/maxPower) * 100;
							float angle = Mathf.Clamp(Vector2.Angle(Vector2.right,(beginPos-endPos)),45,135);
							StartCoroutine(CastLure(speed, angle));
						}
					}
				}
			}
		}


		//Passive Functionality
		if(isReeling && hasCasted){
			reelStick.GetComponent<Rigidbody2D>().AddTorque(-7);
			reelAnimTime = Mathf.Clamp(reelAnimTime+=0.1f,0,1);
			if(reelSpeed == 0){
				reelAnimTime = Mathf.Clamp(reelAnimTime-=0.07f,0,1);
			}
			currentLure.GetComponent<Rigidbody>().AddForce(((currentLure.transform.position-floatDirection.transform.position).normalized)*reelSpeed/100 * -1);
		}else{
			reelAnimTime = Mathf.Clamp(reelAnimTime-=0.1f,0,1);
		}
		//Tension Slider
		float percent = tension / 100f;
		tensionSlider.transform.localPosition = Vector3.Lerp(new Vector3(-95,0,0),new Vector3(0,0,0),percent);
		tensionSlider.GetComponent<Image>().color = Color.Lerp(Color.green,Color.red,percent);

		if(hasCasted && isReeling){
			if(Vector3.Distance(currentLure.transform.position,floatDirection.transform.position) < 0.5f){
				foreach (var item in hovers) {
					item.isDown = false;
				}
				canCast = true;
				hasCasted = false;
				isReeling = false;
				//Add a fish to this method
				Catch();
			}
		}

		if(isReeling)
			overControls = true;
		if(!isReeling && Input.GetMouseButtonUp(0)){
			overControls = false;
		}
		if(!FindObjectOfType<MainMenu>().isOpen){
			if(Input.GetMouseButtonUp(0)){
				exitedMenu = false;
			}
		}

	}

	void FixedUpdate(){
		if(currentLure){
			if(Physics.OverlapSphere(currentLure.transform.position,0.1f).Length > 0){
				isInHotspot = true;
			}else{
				isInHotspot = false;
			}
		}
	}

	IEnumerator CatchLoop(){
		yield return new WaitForSeconds(4);
		catchStarted = false;
		while (!catchStarted) {
			if(Mathf.FloorToInt(Random.Range(0f,10f - currentBait.hookChanceBuff)) == 0){
				curHookCoroutine =	(Coroutine)StartCoroutine(Hooking(FindObjectOfType<FishDatabase>().GetRandomFish()));
			}
			if(isInHotspot)
				yield return new WaitForSeconds(0.25f);
			else
				yield return new WaitForSeconds(1f);
		}
	}

	IEnumerator CastLure(float speed, float angle){
		yield return new WaitForEndOfFrame();
		canCast = false;
		poleAnim.SetBool("cast",true);
		poleAnim.SetBool("reel",false);
		currentFishToCatch = null;
		//Throw the bobber
		yield return new WaitForSeconds(0.4f);
		hasCasted = true;
		currentLure = (GameObject)Instantiate(floatPrefab,instObj.position,floatPrefab.transform.rotation);
		ConfigurableJoint joint = instObj.GetComponent<ConfigurableJoint>();
		Rigidbody lureRigid = currentLure.GetComponent<Rigidbody>();
		joint.connectedBody = lureRigid;
		currentLure.transform.Rotate(new Vector3(0,angle,0));
		lureRigid.AddRelativeForce(new Vector3(0,speed*2,-speed * 3));
		FindObjectOfType<RenderLine>().lure = currentLure;
		curCatchLoopCoroutine = (Coroutine)StartCoroutine(CatchLoop());
	}

	IEnumerator Hooking(Fish fish){
		catchStarted = true;
		float startTime = Time.time;
		bool willCatch = Random.Range(0f,1f) > fish.catchChance;

		WaterPhysics water = FindObjectOfType<WaterPhysics>();
		float startLevel = water.waterLevel;
		float catchTime = Random.Range(1f,3f);

		while(Time.time-startTime < catchTime){
			water.waterLevel -= 0.1f;
			if(isReeling){
				water.waterLevel = startLevel;
				willCatch = false;
				break;
			}
			yield return new WaitForSeconds(Random.Range(0.2f,0.4f));
			if(isReeling){
				water.waterLevel = startLevel;
				willCatch = false;
				curCatchLoopCoroutine = (Coroutine)StartCoroutine(CatchLoop());
				break;
			}
			water.waterLevel += 0.1f;
			yield return new WaitForSeconds(Random.Range(0.2f,0.4f));
		}

		if(willCatch){
			currentFishToCatch = fish;
			curFightCoroutine = (Coroutine)StartCoroutine(FishFight(fish));
			water.waterLevel = startLevel - 0.2f;
		}else{
			water.waterLevel = startLevel;
			catchStarted = false;
			currentFishToCatch = null;
			curCatchLoopCoroutine = (Coroutine)StartCoroutine(CatchLoop());
		}
		yield return new WaitForEndOfFrame();
	}

	IEnumerator FishFight(Fish fish){
		Vector3 posToFightTo = new Vector3(Random.Range(-9f,9f),1.1f,11.85f);
		int reset = 0;
		bool willFight = true;
		bool stop = false;
		Rigidbody lureRigid = currentLure.GetComponent<Rigidbody>();
		while(Vector3.Distance(currentLure.transform.position,posToFightTo) > 2f || stop){
			if(willFight){
				lureRigid.AddForce((posToFightTo-currentLure.transform.position).normalized*Time.deltaTime*(fish.strength+20));
				float pull = 0f;
				if(fish.strength == 0)
					pull = 1f;
				else
					pull = fish.strength;
				if(isReeling){
					tension = Mathf.Clamp(tension += 0.5f,0,100);
				}
				tension = Mathf.Clamp(tension += (pull/4),0,100);
				reelStick.GetComponent<Rigidbody2D>().AddTorque(5);
			}else{
				tension = Mathf.Clamp(tension -= 0.5f,0,100);
			}
			if(tension >= 100 && isReeling){
				Break();
				stop = true;

			}
			reset++;
			if(reset % 50 == 0){
				willFight = Random.value > 0.5f;
			}
			if(reset == 200){
				posToFightTo = new Vector3(Random.Range(-9f,9f),1.1f,11.85f);
				reset = 0;
			}
			yield return new WaitForEndOfFrame();
		}
		if(!stop){
			Break();
		}
	}

	void Catch(){
		if(currentFishToCatch == null || currentFishToCatch.fishName == null){
			currentFishToCatch = null;
		}else{
			StartCoroutine(FindObjectOfType<PopupController>().CreateFishPopup(currentFishToCatch));
			currentFishToCatch = null;
		}
		ResetRod();
	}

	void Break(){
		ResetRod();
	}

	public void ResetRod(){
		if(curCatchLoopCoroutine != null)
			StopCoroutine(curCatchLoopCoroutine);
		if(curFightCoroutine != null)
			StopCoroutine(curFightCoroutine);
		if(curHookCoroutine != null)
			StopCoroutine(curHookCoroutine);
		tension = 0f;
		hovers.ForEach(x => x.isDown = false);
		poleAnim.SetBool("cast",false);
		poleAnim.SetBool("reel",false);
		Destroy(currentLure);
		currentFishToCatch = null;
		canCast = true;
		isReeling = false;
		catchStarted = false;
		isInHotspot = false;
		hasCasted = false;
	}
	public void pointerDown(){
		isReeling = true;
	}
	public void pointerUp(){
		isReeling = false;
	}
}
