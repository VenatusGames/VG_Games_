using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	Vector3 vec = Vector3.one;
	void Update () {
		transform.Rotate(vec * 5);
	}
}
