using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	void Update () {
		transform.Rotate(new Vector3(1,1,1) * 5);
	}
}
