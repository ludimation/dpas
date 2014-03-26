using UnityEngine;
using System.Collections;

public class Cyclone : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		//this.transform.Rotate (Vector3.right, Time.deltaTime * (100.0f), Space.World);
		this.transform.RotateAround (Vector3.zero, Vector3.up, 10 * Time.deltaTime);
	
	}
}
