using UnityEngine;
using System.Collections;

public class General : MonoBehaviour {
	public static bool kinectControl;
	public static ScreenShake screenShake;
	public ScreenShake screenShaker;

	public bool controlByKinect = true;
	//public static bool keyControlOnly;
	//public bool keyboardOnly = false;

	public Transform avatarRoot;

	public Transform lHand;
	public Transform lElbow;
	public Transform lShoulder;

	public Transform rHand;
	public Transform rElbow;
	public Transform rShoulder;

	public bool paused = true;
	public static bool isPaused = true;


	public static float playerSize = 1;
	public static General g;

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
	Texture2D currentInstructions;
	Texture2D currentMenu;
	Texture2D activeImage;
	public bool destroyOnReload = true;
	//float magnitude = 0;
	//public float controlCuttoff = 1;
	KinectManager kMngr;

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
		Gestures.lHand = lHand;
		Gestures.rHand = rHand;
		Gestures.lElbow = lElbow;
		Gestures.rElbow = rElbow;
		Gestures.lShoulder = lShoulder;
		Gestures.rShoulder = rShoulder;

		g = this;

		screenShake = screenShaker;
		//Time.timeScale = 0;
		//DontDestroyOnLoad(gameObject);
		General.kinectControl = controlByKinect;
		//General.keyControlOnly = keyboardOnly;
		kMngr = (KinectManager)gameObject.GetComponent ("KinectManager");
		airObjects = GameObject.FindGameObjectsWithTag("Air");
		earthObjects = GameObject.FindGameObjectsWithTag("Earth");
		fireObjects = GameObject.FindGameObjectsWithTag("Fire");
		waterObjects = GameObject.FindGameObjectsWithTag("Water");
		foreach (GameObject gO in waterObjects){
			Debug.Log (gO.name);
		}

		airControl = GameObject.FindObjectOfType<Air>();
		earthControl = GameObject.FindObjectOfType<Earth>();
		fireControl = GameObject.FindObjectOfType<Fire>();
		waterControl = GameObject.FindObjectOfType<Water>();
		
		currentInstructions = airInstructions;
		//General.element = startingElement;
		changeElement(startingElement);
		currentMenu = startScreen;
		activeImage = currentMenu;
	}
	
	// Update is called once per frame
	void Update () {
		manageMenu();
		//Debug.Log(playerSize.ToString()+", "+General.playerSize.ToString());



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
			if(Input.GetKeyUp (KeyCode.Equals)){
				pause(true);
				currentMenu = endScreen;
				activeImage = currentMenu;
			}
			if(Input.GetKeyUp (KeyCode.Backspace)){
				pause(true);
				currentMenu = startScreen;
				activeImage = currentMenu;
			}
		}


		if(Input.GetKeyUp (KeyCode.Escape)){
			Application.Quit ();
		}
		/*if(Input.GetKeyUp (KeyCode.Tab)){
			kMngr.RecalibratePlayer1();
			pause(true);

		}*/
		
		//Debug.Log (Vector3.Angle (lElbow.position-lShoulder.position, Vector3.up).ToString ()+", "+ Vector3.Angle(lHand.position-lElbow.position, Vector3.right).ToString());
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
					currentMenu = menuScreen;
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
					//Application.Quit();
					currentMenu = endScreen;
					activeImage = currentMenu;
				}
			}
			else{
				quitting = false;
			}
			if(Vector3.Angle (lElbow.position - lShoulder.position, Vector3.left)<45 && Vector3.Angle(lHand.position - lElbow.position, Vector3.up)<45){
				instructions = true;
				if(Vector3.Angle(rElbow.position-rShoulder.position, Vector3.right)<45){
					activeImage = currentInstructions;
				}
				if(Vector3.Angle(rElbow.position-rShoulder.position, Vector3.up)<45){
					activeImage = currentMenu;
				}
			}
			else{
				instructions = false;
			}
			if(Input.GetKeyUp (KeyCode.T)){
				activeImage = currentInstructions;
			}
			if(Input.GetKeyUp(KeyCode.Y)){
				activeImage = currentMenu;
			}



		}
	}
	public void pause(bool p){
		if (p){
			paused = true;
			isPaused = paused;
			Time.timeScale = .00001f;
		}
		else{
			paused = false;
			isPaused = paused;
			Time.timeScale = 1;
		}
	}
	public void changeElement(Element e){
		if(element == e){
			//return;
		}
		element = e;

		//if(airControl.enabled){
			airControl.Sleep();
		//}
		//if(earthControl.enabled){
			earthControl.Sleep ();
		//}
		//if(fireControl.enabled){
			fireControl.Sleep();
		//}
		//if(waterControl.enabled){
			waterControl.Sleep ();
		//}

		airControl.enabled = false;
		earthControl.enabled = false;
		fireControl.enabled = false;
		waterControl.enabled = false;

		if(e != Element.Air){
			foreach (GameObject g in airObjects){
				if(g.particleSystem){
					g.particleSystem.enableEmission = false;
				}
				else{
					g.SetActive (false);
				}
			}
		}
		if(e != Element.Earth){
			foreach (GameObject g in earthObjects){
				if(g.particleSystem){
					g.particleSystem.enableEmission = false;
				}
				else{
					g.SetActive (false);
				}
			}
		}
		if(e != Element.Fire){
			foreach (GameObject g in fireObjects){
				if(g.particleSystem){
					g.particleSystem.enableEmission = false;
				}
				else{
					g.SetActive (false);
				}
			}
		}
		if(e != Element.Water){
			foreach (GameObject g in waterObjects){
				if(!g){
					
					Debug.LogWarning ("water avatar object has been destroyed, cannot deactivate");
				}
				else if(g.particleSystem){
					g.particleSystem.enableEmission = false;
				}
				else{
					g.SetActive (false);
				}
			}
		}
		/*foreach (GameObject g in earthObjects){
			g.SetActive (false);
		}
		foreach (GameObject g in fireObjects){
			g.SetActive (false);
		}
		foreach (GameObject g in waterObjects){
			g.SetActive (false);
		}*/
		if (element == Element.Air){
			airControl.enabled = true;
			airControl.UnSleep();
			currentInstructions = airInstructions;
			foreach (GameObject g in airObjects){
				if(g.particleSystem){
					g.particleSystem.enableEmission = true;
				}
				else{
					g.SetActive (true);
				}
			}
		}
		else if (element == Element.Earth){
			earthControl.enabled = true;
			earthControl.UnSleep();
			currentInstructions = earthInstructions;
			foreach (GameObject g in earthObjects){
				if(g.particleSystem){
					g.particleSystem.enableEmission = true;
				}
				else{
					g.SetActive (true);
				}
			}
		}
		else if (element == Element.Fire){
			fireControl.enabled = true;
			fireControl.UnSleep();
			currentInstructions = fireInstructions;
			foreach (GameObject g in fireObjects){
				//Debug.Log ("found fire object");
				if(g.particleSystem){
					//Debug.Log ("with paprticle system");
					g.particleSystem.enableEmission = true;
				}
				else{
					g.SetActive (true);
				}
			}
		}
		else if (element == Element.Water){
			waterControl.enabled = true;
			waterControl.UnSleep();
			currentInstructions = waterInstructions;
			foreach (GameObject g in waterObjects){
				if(!g){
					Debug.LogWarning ("water avatar object has been destroyed, cannot reactivate");
				}
				else if(g.particleSystem){
					g.particleSystem.enableEmission = true;
				}
				else{
					g.SetActive (true);
				}
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
	/*public static float increaseSize(float amount, float limit){
		if (playerSize > limit){
			return playerSize;
		}
		playerSize += amount;
		playerSize = Mathf.Min (playerSize, limit);
		return playerSize;

	}*/
	public static float changeSize(float amount, float upperLimit, float lowerLimit){
		//string msg = "";
		//Debug.Log (msg);
		if (playerSize + amount > upperLimit){
			//msg += "greater than upper";
			playerSize = Mathf.Max (playerSize, upperLimit);
			//return playerSize;
		}
		else if (playerSize + amount < lowerLimit){
			//msg += "less than lower";
			playerSize = Mathf.Min (playerSize, lowerLimit);
			//return playerSize;
		}
		else{
			playerSize += amount;
		}
		playerSize = Mathf.Max (playerSize, 0);
		playerSize = Mathf.Min (playerSize, 100);
		//msg +="amount " + amount.ToString () +"t: "+Time.deltaTime.ToString ();
		//Debug.Log (msg);

		return playerSize;
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
