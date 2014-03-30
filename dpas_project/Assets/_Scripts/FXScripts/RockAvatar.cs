using UnityEngine;
using System.Collections;

public class RockAvatar : MonoBehaviour {

	public Vector3 rot = Vector3.up;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Time.deltaTime*rot);
	}
}
