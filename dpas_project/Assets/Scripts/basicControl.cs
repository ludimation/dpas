using UnityEngine;
using System.Collections;

public class basicControl : MonoBehaviour {
	public Kinectalogue inPut;
	//public float sensetivity = 10;
	public Vector3 sensetivity = Vector3.one;
	public Vector3 rotSensetivity = Vector3.up;
	//public float pitchSensetivity = 1;
	//public float yawSensetivity = 1;
	//public float rollSensetivity = 1;
	//public float floor = 0;
	//public int mode = 0;
	public float deadzone = .01f;
	public Vector3 rotDeadzone = 5*Vector3.one;
	public CharacterController cha;
	public CharacterMotor motor; 
	// Use this for initialization
	void Start () {
		//motor = (CharacterMotor)gameObject.GetComponent (CharacterMotor);
		//motor = gameObject.GetComponent<CharacterMotor>();
		motor = cha.GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 move;
		Vector3 rot;
		if(!General.kinectControl){
			//transform.Translate (sensetivity * Time.deltaTime * new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis ("Flight"), Input.GetAxis ("Vertical")));
			//transform.Rotate (0, Input.GetAxis ("Mouse X")*yawSensetivity, 0);
			move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis ("Flight"), Input.GetAxis ("Vertical"));
			rot = 45*new Vector3(0, Input.GetAxis ("Mouse X"), 0);
		}

		else{
			//pitch, yaw, roll
			//transform.Translate (inPut.getDiff()*Time.deltaTime*sensetivity);
			//transform.Rotate (inPut.getPitch()*Time.deltaTime,inPut.getYaw()*Time.deltaTime, inPut.getRoll()*Time.deltaTime);
			move = (inPut.getDiff());
			rot = inPut.getRot ();
			//deadzone stuff
			if (move.magnitude < deadzone){
				move = Vector3.zero;
			}
			else{
				move -= (deadzone * move.normalized);
			}

			//temp = rot - rotDeadzone;
			//Vector3 temp2 = Vector3.Scale (rot, temp);
			//temp.Scale (rot);
			if(rot.x < rotDeadzone.x){
				rot.x = 0;
			}
			else{
				rot.x -= Mathf.Sign(rot.x) * rotDeadzone.x;
			}
			if(rot.x < rotDeadzone.y){
				rot.y = 0;
			}
			else{
				rot.y -= Mathf.Sign (rot.y)*rotDeadzone.y;
			}
			if(rot.z < rotDeadzone.z){
				rot.z = 0;
			}
			else{
				rot.z -= Mathf.Sign (rot.z)*rotDeadzone.z;
			}
		}

		move.Scale (Time.deltaTime*sensetivity);
		rot.Scale (Time.deltaTime*rotSensetivity);
		motor.inputMoveDirection = transform.rotation*move;
		//transform.Translate(move);
		transform.Rotate (rot);


	}
}
