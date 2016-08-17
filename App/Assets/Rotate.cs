using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	public int speed;
	Vector3 vec = Vector3.one;
	void Update () {
		transform.Rotate(vec * speed);
	}
}
