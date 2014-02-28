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

	public bool paused = true;
	public static bool isPaused = true;
	bool quitting = false;
	bool menu = false;
	public Texture2D image;
	//public bool destroyOnLoad = false;
	public bool destroyOnReload = true;
	KinectManager kMngr;

	// Use this for initialization
	void Start () {
		if(!destroyOnReload){
			DontDestroyOnLoad(gameObject);
		}
		//DontDestroyOnLoad(gameObject);
		General.kinectControl = controlByKinect;
		kMngr = (KinectManager)gameObject.GetComponent ("KinectManager");
	}
	
	// Update is called once per frame
	void Update () {

		/*if(kMngr.Player1Avatars[0] == null){
			kMngr.Player1Avatars[0] = (GameObject)GameObject.Find ("kAvatar");
		}
		if(kMngr.Player1Avatars[1] == null){
			kMngr.Player1Avatars[1] = (GameObject)GameObject.Find ("kController");
		}*/
		if (Input.GetKeyUp (KeyCode.J)){
			General.printJointPos (avatarRoot);
		}
		if(Input.GetKeyUp (KeyCode.F)){
			Application.LoadLevel("s-fire1");
		}
		if(Input.GetKeyUp (KeyCode.E)){
			Application.LoadLevel("s-earth2");
		}
		if(Input.GetKeyUp (KeyCode.T)){
			Application.LoadLevel ("testFire");
		}
		if(Input.GetKeyUp (KeyCode.Y)){
			Application.LoadLevel("testEarth");
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
				isPaused = paused;
			}
			else if (Vector3.Angle (rHand.position-rElbow.position, Vector3.right) < 45){
				paused = false;
				isPaused = paused;
			}
		}
		if (Input.GetKeyUp (KeyCode.P)){
			paused = !paused;
			isPaused = paused;
		}
		if (paused){
			if(Vector3.Angle(rElbow.position-rShoulder.position, Vector3.up) < 60 && Vector3.Angle (rHand.position-rElbow.position, Vector3.left) < 45){
				quitting = true;
				if(Vector3.Angle (lElbow.position-lShoulder.position, Vector3.left) <45 && Vector3.Angle (lHand.position-lElbow.position, Vector3.left)<45){
					Application.Quit();
				}
			}
			else{
				quitting = false;
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
			if (image == null){
				GUI.Box (new Rect(50, 50, Screen.width - 100, 100), "paused");
			}
			else{
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), image);
			}

		}
		if (quitting){
			GUI.Box (new Rect((Screen.width/2)-50, (Screen.height/2)-25, 100, 50), "Are you sure you would like to exit?");
		}
	}
}
