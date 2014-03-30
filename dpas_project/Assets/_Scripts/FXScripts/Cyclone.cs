using UnityEngine;
using System.Collections;

public class Cyclone : MonoBehaviour {
	public Vector3 _point = Vector3.zero;
	void Start () {
		//_point = new Vector3(1,1,1)
	}
	// Update is called once per frame
	void Update () {
		//this.transform.Rotate (Vector3.right, Time.deltaTime * (100.0f), Space.World);
		transform.RotateAround (_point, Vector3.up, 200 * Time.deltaTime);
	
	}
}
