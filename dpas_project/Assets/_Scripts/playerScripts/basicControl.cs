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
			if(General.element == General.Element.Air){
				if(Gestures.OneArmUp ()||Gestures.ArmsDown ()){
					//Debug.Log ("one arm is up");
					move = Vector3.zero;
				}
				else if(Gestures.ArmsTogether()/* && !Gestures.ArmsDown()*/){
					move.y = -downforce;
					move *= windSlowFactor;
				}
				else {
					move.y = -downforce;
				}
			}
			else{
				move.y = 0;
			}
			if((General.element == General.Element.Fire && !cha.isGrounded)||(General.element == General.Element.Air&&!Gestures.OneArmUp ())){
				//move -= (Gestures.LArmDir ()+Gestures.RArmDir ()).normalized;
				move -= (Gestures.LArmDir ()+Gestures.RArmDir ());
			}

			//move.Scale (Vector3.right+Vector3.forward);
			rot = inPut.getRot ();
			/*if(General.element != General.Element.Earth && Gestures.LArmStraight () && Gestures.RArmStraight ()){
				if(Vector3.Angle (Gestures.LArmDir(), Vector3.down) >30 && Vector3.Angle (Gestures.RArmDir(), Vector3.down) >30){
					if ((Vector3.Angle (Gestures.LArmDir(), Vector3.down) <60 && Vector3.Angle (Gestures.RArmDir(), Vector3.down) <60) || (General.element == General.Element.Air && (Vector3.Angle (Gestures.LArmDir(), Vector3.down) <135 && Vector3.Angle (Gestures.RArmDir(), Vector3.down) <135))){
						move += Vector3.Scale (Gestures.LArmDir ()+ Gestures.RArmDir (), new Vector3(-1,-1,-1));
					}
				}
			}*/
			/*if(Genreal.element == General.Element.Air){
				if (Gestures.LArmStraight () && Gestures.RArmStraight ()){
					if(
				}
			}*/
			//Debug.Log ("cut = "+ posCutoff.ToString ()+", mag = "+inPut.getMagnitudeRaw ().ToString ());
			if(inPut.getMagnitudeRaw()>posCutoff*2){
				move = Vector3.zero;
				rot = Vector3.zero;
				//Debug.Log ("cuttoff")
			}
			else if(inPut.getMagnitudeRaw()>posCutoff){
				move.Normalize();
				move *= .5f;
				//Debug.Log ("cuttoff")
			}
			//deadzone stuff
			//Debug.Log ("dz = "+ deadzone.ToString ()+ ", mag = "+move.magnitude.ToString ()+", move = "+move.ToString ());
			if (move.magnitude < deadzone){
				move = Vector3.zero;
			}
			else{
				move -= (deadzone * move.normalized);

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
