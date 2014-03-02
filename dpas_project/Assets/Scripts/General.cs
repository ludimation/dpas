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
	bool instructions = false;
	public Texture2D menuIndicator;
	public Texture2D quitIndicator;
	public Texture2D menuScreen;
	public Texture2D startScreen;
	public Texture2D endScreen;
	public Texture2D airInstructions;
	public Texture2D earthInstructions;
	public Texture2D fireInstructions;
	public Texture2D waterInstructions;
	public Texture2D instructionIndicator;
	Texture2D image;
	Texture2D activeImage;
	public bool destroyOnReload = true;
	//KinectManager kMngr;

	Air airControl;
	Earth earthControl;
	Fire fireControl;
	Water waterControl;

	public Element startingElement = Element.None;
	public static Element element = Element.None;
	GameObject[] airObjects;
	GameObject[] earthObjects;
	GameObject[] fireObjects;
	GameObject[] waterObjects;

	public enum Element {
		Air = 1,
		Earth = 2,
		Fire = 3,
		Water = 4,
		None = 5
	};
	// Use this for initialization
	void Start () {
		if(!destroyOnReload){
			DontDestroyOnLoad(gameObject);
		}
		Time.timeScale = 0;
		//DontDestroyOnLoad(gameObject);
		General.kinectControl = controlByKinect;
		//kMngr = (KinectManager)gameObject.GetComponent ("KinectManager");
		airObjects = GameObject.FindGameObjectsWithTag("Air");
		earthObjects = GameObject.FindGameObjectsWithTag("Earth");
		fireObjects = GameObject.FindGameObjectsWithTag("Fire");
		waterObjects = GameObject.FindGameObjectsWithTag("Water");

		airControl = GameObject.FindObjectOfType<Air>();
		earthControl = GameObject.FindObjectOfType<Earth>();
		fireControl = GameObject.FindObjectOfType<Fire>();
		waterControl = GameObject.FindObjectOfType<Water>();

		changeElement(element);
		image = menuScreen;
		activeImage = image;
	}
	
	// Update is called once per frame
	void Update () {
		manageMenu();


	}
	void manageMenu(){
		if(!kinectControl){
			if(Input.GetKeyUp (KeyCode.F)){
				
				changeElement(Element.Fire);
			}
			if(Input.GetKeyUp (KeyCode.G)){
				
				changeElement(Element.Earth);
			}
			if(Input.GetKeyUp (KeyCode.H)){
				
				
				changeElement(Element.Water);
			}
			if(Input.GetKeyUp (KeyCode.J)){
				
				
				changeElement(Element.Air);
			}
			if (Input.GetKeyUp (KeyCode.P)){
				pause(!paused);
			}
		}

			/*if (Input.GetKeyUp (KeyCode.P)){
				paused = !paused;
				isPaused = paused;
				if(paused){
					Time.timeScale = 0;
				}
				else{
					Time.timeScale = 1;
				}
			}*/
		//}
		if(Input.GetKeyUp (KeyCode.Escape)){
			Application.Quit ();
		}
		
		
		if(Vector3.Angle(lElbow.position-lShoulder.position, Vector3.up) < 60 && Vector3.Angle (lHand.position-lElbow.position, Vector3.right) < 45){
			menu = true;
		}
		else{
			menu = false;
		}
		//if (menu && ((Vector3.Angle (rElbow.position-rShoulder.position, Vector3.right) <45)||(!General.kinectControl&&Input.GetKey(KeyCode.P)))){
		if(menu){
			if (Vector3.Angle (rElbow.position-rShoulder.position, Vector3.right) <45){
				if (Vector3.Angle (rHand.position-rElbow.position, Vector3.up)<45){
					pause(true);
				}
				else if (Vector3.Angle (rHand.position-rElbow.position, Vector3.right)<45){
					pause(false);
				}
			}

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
			if(Vector3.Angle (lElbow.position - lShoulder.position, Vector3.left)<45 && Vector3.Angle(lHand.position - lElbow.position, Vector3.up)<45){
				instructions = true;
				if(Vector3.Angle(rElbow.position-rShoulder.position, Vector3.right)<45){
					activeImage = image;
				}
				if(Vector3.Angle(rElbow.position-rShoulder.position, Vector3.up)<45){
					activeImage = menuScreen;
				}
			}
			else{
				instructions = false;
			}
			if(Input.GetKeyUp (KeyCode.T)){
				activeImage = image;
			}
			if(Input.GetKeyUp(KeyCode.Y)){
				activeImage = menuScreen;
			}



		}
	}
	public void pause(bool p){
		if (p){
			paused = true;
			isPaused = paused;
			Time.timeScale = 0;
		}
		else{
			paused = false;
			isPaused = paused;
			Time.timeScale = 1;
		}
	}
	public void changeElement(Element e){
		element = e;
		airControl.enabled = false;
		earthControl.enabled = false;
		fireControl.enabled = false;
		waterControl.enabled = false;

		foreach (GameObject g in airObjects){
			g.SetActive (false);
		}
		foreach (GameObject g in earthObjects){
			g.SetActive (false);
		}
		foreach (GameObject g in fireObjects){
			g.SetActive (false);
		}
		foreach (GameObject g in waterObjects){
			g.SetActive (false);
		}
		if (element == Element.Air){
			airControl.enabled = true;
			image = airInstructions;
			foreach (GameObject g in airObjects){
				g.SetActive (true);
			}
		}
		else if (element == Element.Earth){
			earthControl.enabled = true;;
			image = earthInstructions;
			foreach (GameObject g in earthObjects){
				g.SetActive (true);
			}
		}
		else if (element == Element.Fire){
			fireControl.enabled = true;;
			image = fireInstructions;
			foreach (GameObject g in fireObjects){
				g.SetActive (true);
			}
		}
		else if (element == Element.Water){
			waterControl.enabled = true;;
			image = waterInstructions;
			foreach (GameObject g in waterObjects){
				g.SetActive (true);
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

		if (paused){

			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), activeImage);


		}
		if (menu){
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), menuIndicator);
		}
		if (quitting){
			GUI.DrawTexture (new Rect((Screen.width/2)-50, (Screen.height/2)-25, 100, 50), quitIndicator);
		}
		if (instructions){
			GUI.DrawTexture(new Rect(Screen.width-75, 0, 75, Screen.height), instructionIndicator);
		}

	}
}
