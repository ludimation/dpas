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
	public int mode = 0;
	public float deadzone = 0;
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
			rot = new Vector3(0, Input.GetAxis ("Mouse X"), 0);
		}

		else{
			//pitch, yaw, roll
			//transform.Translate (inPut.getDiff()*Time.deltaTime*sensetivity);
			//transform.Rotate (inPut.getPitch()*Time.deltaTime,inPut.getYaw()*Time.deltaTime, inPut.getRoll()*Time.deltaTime);
			move = (inPut.getDiff());
			rot = inPut.getRot ();
		}
		move.Scale (sensetivity);
		rot.Scale (Time.deltaTime*rotSensetivity);
		motor.inputMoveDirection = transform.rotation*move;
		transform.Rotate (rot);

		/*else if(mode == 1){
			//pitch, yaw,
			transform.Translate (inPut.getDiff()*Time.deltaTime*sensetivity);
			transform.Rotate (inPut.getPitch()*Time.deltaTime,inPut.getYaw()*Time.deltaTime, 0);
		}
		else if(mode == 2){
			//pitch only
			transform.Translate (inPut.getDiff()*Time.deltaTime*sensetivity);
			transform.Rotate (inPut.getPitch()*Time.deltaTime,0, 0);
		}
		
		else if(mode == 3){
			//pitch, roll
			transform.Translate (inPut.getDiff()*Time.deltaTime*sensetivity);
			transform.Rotate (inPut.getPitch()*Time.deltaTime,0, inPut.getRoll ()*Time.deltaTime);
		}
		else if(mode == 4){
			//yaw only
			Vector3 temp =(inPut.getDiff()*Time.deltaTime*sensetivity);
			if(temp.magnitude<deadzone){
				temp = Vector3.zero;
			}
			transform.Translate (temp);
			transform.Rotate (0,inPut.getYaw()*Time.deltaTime, 0);
		}
		else if (mode == 5){
			//no rotation
			transform.Translate (inPut.getDiff()*Time.deltaTime*sensetivity);

		}*/

		/*if (transform.position.y < floor){
			transform.Translate (new Vector3(0, - transform.position.y, 0));
		}*/
	}
}
