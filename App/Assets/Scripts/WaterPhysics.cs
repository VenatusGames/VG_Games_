using UnityEngine;
using System.Collections;

public class WaterPhysics : MonoBehaviour {
	public float waterLevel;
	public float floatHeight;
	public float bounceDamp;
	public Vector3 buoyancyCentreOffset;

	private float forceFactor;
	private Vector3 actionPoint;
	private Vector3 upLift;

	void FixedUpdate(){
		actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
		forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);

		if(forceFactor > 0f){
			upLift = -Physics.gravity * (forceFactor - GetComponent<Rigidbody>().velocity.y * bounceDamp);
			GetComponent<Rigidbody>().AddForceAtPosition(upLift,actionPoint);
		}
	}
}
