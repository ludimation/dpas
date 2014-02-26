using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	public Kinectalogue player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(player.getPitch()*Time.deltaTime, 0, 0));
		//transform.Rotate(0, 0, 0));

	}
}
