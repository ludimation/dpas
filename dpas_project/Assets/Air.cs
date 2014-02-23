using UnityEngine;
using System.Collections;

public class Air : MonoBehaviour {
	public Kinectalogue controller;
	public AudioSource AudSrc;
	public AudioClip launch;
	public float speed = 1;
	public float rotSpeed = 45;
	public float deadzone = .1f;
	public float rotDeadzone = 2;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 move;
		float yaw;
		if (General.kinectControl){
			move = controller.getDiff();
			move.Scale (new Vector3(1,1,1));
			yaw = controller.getRoll ();
			
			if(move.magnitude<deadzone){
				move = Vector3.zero;
			}
			//transform.Translate (temp);
			yaw = controller.getYaw();
			if (Mathf.Abs (yaw) < rotDeadzone){
				yaw = 0;
			}
			
			
		}
		else{
			move = new Vector3(Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			yaw = Input.GetAxis ("Mouse X");
		}
		//transform.Rotate (rotSpeed*new Vector3(0, yaw, 0));
		//transform.Translate (speed*Time.deltaTime*move);
		

	}
}
