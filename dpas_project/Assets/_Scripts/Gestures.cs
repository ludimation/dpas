using UnityEngine;
using System.Collections;

public static class Gestures : object {

	public static Transform lHand;
	public static Transform rHand;
	public static Transform lElbow;
	public static Transform rElbow;
	public static Transform lShoulder;
	public static Transform rShoulder;
	static Vector3 lHandOld;
	
	//public Transform player;

	//public bool dbg = false;
	//public LineRenderer line;


	// Use this for initialization
	//void Start () {
		/*if(dbg){
			if(line == null){
				line = gameObject.GetComponent<LineRenderer>();
			}
			line.SetVertexCount(6);
	
		}*/
	//}
	
	// Update is called once per frame
	//void Update () {
		/*if(dbg){
			line.SetPosition(0, (player.rotation*lHand.position)+player.position);
			line.SetPosition(1, (player.rotation*lElbow.position)+player.position);
			line.SetPosition(2, (player.rotation*lShoulder.position)+player.position);
			line.SetPosition(3, (player.rotation*rShoulder.position)+player.position);
			line.SetPosition(4, (player.rotation*rElbow.position)+player.position);
			line.SetPosition(5, (player.rotation*rHand.position)+player.position);
		}*/	
	//}

	public static bool ArmsUp(){
		//Debug.Log (Vector3.Angle(lHand.position-lElbow.position, Vector3.up).ToString ()+", "+Vector3.Angle(rHand.position-rElbow.position, Vector3.up).ToString ()+", "+Vector3.Angle(lElbow.position-lShoulder.position, Vector3.up).ToString ()+", "+Vector3.Angle(rShoulder.position-rShoulder.position, Vector3.up).ToString ());

		if(Vector3.Angle(lHand.position-lElbow.position, Vector3.up)<45&&Vector3.Angle(rHand.position-rElbow.position, Vector3.up)<45&&Vector3.Angle(lElbow.position-lShoulder.position, Vector3.up)<45&&Vector3.Angle(rElbow.position-rShoulder.position, Vector3.up)<45){
			return true;
		}
		else return false;
	}
	public static bool ArmsDown(){
		if(Vector3.Angle(lHand.position-lElbow.position, Vector3.down)<45&&Vector3.Angle(rHand.position-rElbow.position, Vector3.down)<45&&Vector3.Angle(lElbow.position-lShoulder.position, Vector3.down)<45&&Vector3.Angle(rElbow.position-rShoulder.position, Vector3.down)<45){
			return true;
		}
		else return false;
	}
	public static bool ArmsOut(){
		if(Vector3.Angle (lHand.position-lElbow.position, lHand.position-lShoulder.position) < 45&&Vector3.Angle (rHand.position-rElbow.position, rHand.position-rShoulder.position) < 45 && Vector3.Angle(lHand.position-lShoulder.position, rHand.position-rShoulder.position) > 135){
			return true;
		}
		else return false;
	}
	public static bool ArmsTogether(){
		if(Vector3.Angle (lHand.position-lElbow.position, lHand.position-lShoulder.position) < 45&&Vector3.Angle (rHand.position-rElbow.position, rHand.position-rShoulder.position) < 45 && Vector3.Angle(lHand.position-lShoulder.position, rHand.position-rShoulder.position) < 45){
			return true;
		}
		else return false;

	}
	public static bool LArmStraight(){
		if(Vector3.Angle (lHand.position-lElbow.position, lElbow.position-lShoulder.position)<45){
			return true;
		}
		else return false;
	}
	public static Vector3 LArmDir(){
		return lHand.position-lShoulder.position;
	}
	public static bool RArmStraight(){
		if(Vector3.Angle (rHand.position-rElbow.position, rElbow.position-rShoulder.position)<45){
			return true;
		}
		else return false;
	}
	public static Vector3 RArmDir(){
		return rHand.position-rShoulder.position;
	}
	public static float AngleBetweenArms(){
		return (Vector3.Angle (lHand.position-lShoulder.position, rHand.position-rShoulder.position));
	}
	public static Vector3 CommonDir(){
		return .5f*((lHand.position+rHand.position) - (lShoulder.position + rShoulder.position));
	}
	public static Vector3 DirBetweenHands(){
		return (rHand.position - lHand.position);
	}
	public static Quaternion shoulderRot(){
		return Quaternion.FromToRotation (Vector3.right, rShoulder.position - lShoulder.position);
	}
	public static Quaternion flatShoulderRot(){
		return Quaternion.FromToRotation (Vector3.right, Vector3.Scale(rShoulder.position - lShoulder.position, Vector3.one - Vector3.up));
	}
	/*void OnGUI(){
		if(dbg){
			if(HandsUp()){
				GUI.Box (new Rect(0,0, Screen.width, 100), "up");
				Debug.Log ("up");
			}
			if(HandsDown()){
				GUI.Box (new Rect(0,Screen.height-100, Screen.width, 100), "down");
			}
			if(HandsOut()){
				GUI.Box (new Rect(0,0, 100, Screen.height), "out");
				GUI.Box (new Rect(Screen.width-100,0, 100, Screen.height), "out");
			}
			if(HandsTogether()){
				GUI.Box (new Rect(150,150, Screen.width-300, Screen.height-300), "together");
			}

		}
	}*/
}
