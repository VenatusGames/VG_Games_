using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {
	Vector3 target;
	public float speed;
	public bool attracted;
	public GameObject bobber;
	public FishDatabase database;
	public GameObject fishPrefab;
	public bool caught;
	void Start(){
		StartCoroutine(FadeIn());
		database = FindObjectOfType<FishDatabase>();
		fishPrefab = database.fish[Random.Range(0,database.fish.Count)];
	}

	void Update(){
		if(!caught){
			if(!attracted){
				if(target != Vector3.zero){
					Vector3 pos = transform.position;
					speed = Random.Range(0f,1f);
					float step = speed * Time.deltaTime;
					transform.position = Vector3.MoveTowards(pos,target,step);
					if(transform.position == target){
						CancelInvoke();
						RandomPosInSquare();
					}
				}else{
					RandomPosInSquare();
				}
			}else if(bobber){
				Vector3 pos = transform.position;
				speed = Random.Range(0f,1f);
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards(pos,new Vector3(bobber.transform.position.x,1.14f,bobber.transform.position.z),step);
			}
		}else{
			Vector3 pos = transform.position;
			speed = Random.Range(0f,1f);
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(pos,new Vector3(bobber.transform.position.x,1.14f,bobber.transform.position.z),step);
		}
	}

	void RandomPosInSquare(){
		target = new Vector3(Random.Range(-2f,4f),1.14f,Random.Range(2f,9f));
		Invoke("RandomPosInSquare",2f);

	}

	public void PrepareFishToReel(){
		caught = true;
	}

	IEnumerator FadeIn(){
		SpriteRenderer render = GetComponent<SpriteRenderer>();
		for (int i = 0; i < 10; i++) {
			Color newCol = render.color;
			newCol.a += 0.1f;
			render.color = newCol;
			yield return new WaitForEndOfFrame();
		}
	}
}
