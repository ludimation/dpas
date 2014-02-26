using UnityEngine;
using System.Collections;

public class General : MonoBehaviour {
	public static bool kinectControl;
	public bool controlByKinect = true;
	public Transform avatarRoot;

	public Transform lHand;
	public Transform lElbow;
	public Transform lShoulder;

	public Transform rHand;
	public Transform rElbow;
	public Transform rShoulder;

	bool paused = false;
	bool menu = false;

	// Use this for initialization
	void Start () {
		General.kinectControl = controlByKinect;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.J)){
			General.printJointPos (avatarRoot);
		}

		if(Vector3.Angle(lElbow.position-lShoulder.position, Vector3.up) < 60 && Vector3.Angle (lHand.position-lElbow.position, Vector3.right) < 45){
			menu = true;
		}
		else{
			menu = false;
		}
		if (menu && Vector3.Angle (rElbow.position-rShoulder.position, Vector3.right) <45){
			if (Vector3.Angle (rHand.position-rElbow.position, Vector3.up) < 45){
				paused = true;
			}
			else if (Vector3.Angle (rHand.position-rElbow.position, Vector3.right) < 45){
				paused = false;
			}

		}
	
	}

	public static void printJointPos(Transform root){
		string msg = "";
		msg += root.name + ": "+root.position.ToString();
		foreach(Transform c in root.transform){
			msg += "\n"+c.name+": "+c.position.ToString ();
		}
		Debug.Log (msg);

	}
	void OnGUI(){
		if (menu){
			GUI.Box (new Rect(50, 0, Screen.width - 100, 50), "menu");
		}
		if (paused){
			GUI.Box (new Rect(50, 50, Screen.width - 100, 100), "paused");

		}
	}
}
