using UnityEngine;
using System.Collections;

public class blahhh : MonoBehaviour {
	public Transform startPos;
	// Use this for initialization
	void Start () {
		transform.position = startPos.position;
		transform.rotation = startPos.rotation;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
