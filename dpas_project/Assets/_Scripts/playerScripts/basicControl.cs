using UnityEngine;
using System.Collections;

public class basicControl : MonoBehaviour {
	public Kinectalogue inPut;
	public float posCutoff = .616f;
	public float hardCutoff = .75f;
	public Vector3 sensetivity = Vector3.one;
	public Vector3 rotSensetivity = Vector3.up;
	public Vector3 flightSensitivityModifier = Vector3.one;

	public float deadzone = .01f;
	public Vector3 rotDeadzone = 5*Vector3.one;
	public CharacterController cha;
	public CharacterMotor motor;
	public bool fly = false;
	Vector3 move;
	Vector3 rot;
	public Texture2D debugCircle;
	public Texture2D debugPoint;
	public float downforce = .5f;
	public float windSlowFactor = .1f;
	// Use this for initialization
	void Start () {
		motor = cha.GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!General.isPaused){




			move = (inPut.getDiff());

			//Debug.Log (move.ToString());

			//move.Scale (Vector3.right+Vector3.forward);
			rot = inPut.getRot ();

			if(inPut.getMagnitudeRaw()>hardCutoff){
				move = Vector3.zero;
				rot = Vector3.zero;
				Debug.Log ("hardcuttoff");
			}
			else if(inPut.getMagnitudeRaw()>posCutoff){
				move.Normalize();
				move *= posCutoff;
				Debug.Log ("cuttoff");
			}
			//deadzone stuff
			//Debug.Log ("dz = "+ deadzone.ToString ()+ ", mag = "+move.magnitude.ToString ()+", move = "+move.ToString ());
			else if (move.magnitude < deadzone){
				//Debug.Log ("deadzone, move = "+move.magnitude.ToString ()+", dz = "+deadzone.ToString ());
				move = Vector3.zero;
			}
			else{
				move -= (deadzone * move.normalized);

			}
			if(General.element == General.Element.Air){
				move -= (Gestures.LArmDir ()+Gestures.RArmDir ());
				if(Gestures.OneArmUp ()||Gestures.ArmsDown ()){
					move = Vector3.zero;
				}
				else if(Gestures.ArmsTogether()){
					move.y = -downforce;
					move *= windSlowFactor;
				}
				else {
					move.y -= downforce;
				}
			}
			else{
				move.y = 0;
			}
			if(General.element == General.Element.Fire && !cha.isGrounded){
				move -= (Gestures.LArmDir ()+Gestures.RArmDir ());
			}

			if(Mathf.Abs (rot.x) < rotDeadzone.x){
				rot.x = 0;
			}
			else{
				rot.x -= Mathf.Sign(rot.x) * rotDeadzone.x;
			}
			if(Mathf.Abs (rot.y) < rotDeadzone.y){
				rot.y = 0;
			}
			else{
				rot.y -= Mathf.Sign (rot.y)*rotDeadzone.y;
			}
			if(Mathf.Abs (rot.z) < rotDeadzone.z){
				rot.z = 0;
			}
			else{
				rot.z -= Mathf.Sign (rot.z)*rotDeadzone.z;
			}
			if(!General.kinectControl){
				move += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis ("Flight"), Input.GetAxis ("Vertical"));
				rot += 45*new Vector3(0, Input.GetAxis ("Yaw"), 0);
			}
			rot.Scale (new Vector3(Mathf.Abs (rot.x), Mathf.Abs (rot.y), Mathf.Abs (rot.z)));
			rot.Scale ((1f/90f)*Vector3.one);
			move.Scale (Time.deltaTime*sensetivity);
			rot.Scale (Time.deltaTime*rotSensetivity);
			if(fly){
				//transform.Translate (move);
				cha.Move (transform.rotation*move);

			}

			else{
				motor.inputMoveDirection = transform.rotation*Vector3.Scale(move, new Vector3(1,0,1));
			}
			//Debug.Log (move.ToString());
			transform.Rotate (rot);

		}
	}
	void OnLevelWasLoaded(){
		//transform.position = new Vector3(100,100,100);

		transform.position = GameObject.Find ("PlayerStartPos").transform.position;
	}
	void OnGUI(){
		
		if(General.dbgMode){
			GUI.DrawTexture (new Rect((Screen.width-128)/2, Screen.height-128, 128, 128), debugCircle);
			GUI.DrawTexture (new Rect(((Screen.width-32)/2)+(move.x*32), (Screen.height-80)-(move.z*32), 32, 32), debugPoint);
			//Debug.Log (move.ToString ());
		}
	}
}
