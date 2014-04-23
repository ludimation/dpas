﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class General : MonoBehaviour {
	public static bool dbgMode;
	public bool debugMode = true;
	public static bool kinectControl;
	public bool controlByKinect = true;
	public static bool icons = true;
	public bool showIcons = true;
	public static ScreenShake screenShake;
	public ScreenShake screenShaker;
	public GameObject player;
	public GameObject avatarController;
	public GameObject camera;
	public GameObject music;

	public SmoothFollow cameraFollow;

	public List<AudioClip> songs;
	//public static bool keyControlOnly;
	//public bool keyboardOnly = false;

	public Transform avatarRoot;

	public GameObject airEffect;
	public GameObject earthEffect;
	public GameObject fireEffect;
	public GameObject waterEffect;

	public List<Transform> effectJoints;

	public Transform lHand;
	public Transform lElbow;
	public Transform lShoulder;

	public Transform rHand;
	public Transform rElbow;
	public Transform rShoulder;

	public bool paused = true;
	public static bool isPaused = true;


	public static float playerSize = 1;
	public float startingEnergy = 100;
	static float availableEnergy = 100;
	public static General g;

	bool quitting = false;
	bool menu = false;
	bool instructions = false;
	public Texture2D selector;
	public Texture2D notCalibrated;
	Vector2 rHandLoc;
	Vector2 lHandLoc;
	public float maxSelectDistance = 20;
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
	int selectedLevel = 0;
	int currentLevel = 0;

	public List<Texture2D> levelGUIs;
	public List<Texture2D> selectedLevelGUIs;

	/*public Texture2D lvl0;
	public Texture2D lvl0Selected;
	public Texture2D lvl1;
	public Texture2D lvl1Selected;
	public Texture2D lvl2;
	public Texture2D lvl2Selected;*/
	//public AudioSource audSrc;
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
		availableEnergy = startingEnergy;

		/*
		Gestures.lHand = lHand;
		Gestures.rHand = rHand;
		Gestures.lElbow = lElbow;
		Gestures.rElbow = rElbow;
		Gestures.lShoulder = lShoulder;
		Gestures.rShoulder = rShoulder;
*/
		InitializeGestures();
		g = this;

		screenShake = screenShaker;
		//Time.timeScale = 0;
		//DontDestroyOnLoad(gameObject);
		General.kinectControl = controlByKinect;
		General.dbgMode = debugMode;
		General.icons = showIcons;

		airControl = GameObject.FindObjectOfType<Air>();
		earthControl = GameObject.FindObjectOfType<Earth>();
		fireControl = GameObject.FindObjectOfType<Fire>();
		waterControl = GameObject.FindObjectOfType<Water>();
		//General.keyControlOnly = keyboardOnly;
		//kMngr = (KinectManager)gameObject.GetComponent ("KinectManager");
		if(!kMngr){
			kMngr = (KinectManager)gameObject.GetComponent<KinectManager>();
		}
		//kMngr.
		GameObject t;
		//airControl.effects = new List<GameObject>();
		earthControl.effects = new List<GameObject>();
		fireControl.effects = new List<GameObject>();
		waterControl.effects = new List<GameObject>();

		foreach (Transform j in effectJoints){
			//t = (GameObject)Instantiate(airEffect, j.position, Quaternion.identity);
			//t.transform.parent = j;
			//t.tag = "Air";
			//airControl.effects.Add

			
			t = (GameObject)Instantiate(earthEffect, j.position, Quaternion.identity);
			t.transform.parent = j;
			t.tag = "Earth";
			earthControl.effects.Add (t);
			
			t = (GameObject)Instantiate(fireEffect, j.position, Quaternion.identity);
			t.transform.parent = j;
			t.tag = "Fire";
			fireControl.effects.Add (t);

			
			t = (GameObject)Instantiate(waterEffect, j.position, Quaternion.identity);
			t.transform.parent = j;
			t.tag = "Water";
			waterControl.effects.Add (t);

		}
		airObjects = GameObject.FindGameObjectsWithTag("Air");
		earthObjects = GameObject.FindGameObjectsWithTag("Earth");
		fireObjects = GameObject.FindGameObjectsWithTag("Fire");
		waterObjects = GameObject.FindGameObjectsWithTag("Water");
		//foreach (GameObject gO in waterObjects){
		//	Debug.Log (gO.name);
		//}


		
		currentInstructions = airInstructions;
		//General.element = startingElement;
		changeElement(startingElement);
		currentMenu = startScreen;
		activeImage = currentMenu;

		if(!destroyOnReload){
			DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(camera);
			DontDestroyOnLoad(screenShaker);
			DontDestroyOnLoad(player);
			DontDestroyOnLoad(avatarController);
			DontDestroyOnLoad(music);

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!kMngr.IsPlayerCalibrated(1)){
			//Debug.Log ("player 1 not calibrated");
		}
		manageMenu();
		cameraFollow.distance = 3 + Mathf.Sqrt (playerSize);
		//Debug.Log ("checkjoints = "+checkJoints().ToString());
		//Debug.Log (Vector2.Distance (lHandLoc, rHandLoc).ToString ());
		//Debug.Log(playerSize.ToString()+", "+General.playerSize.ToString());



	}
	public static bool checkJoints(){
		//foreach
		for(int i = 0; i< 13; ++i){

			if (!g.kMngr.IsJointPositionTracked(1, i)){
				return false;
			}
		}
		return true;
	}
	void manageMenu(){
		if(paused){
			//kMngr.
			rHandLoc = new Vector2(Gestures.RArmDir().x, -Gestures.RArmDir().y);
			lHandLoc = new Vector2(Gestures.LArmDir().x, -Gestures.LArmDir().y);
			float rHandAngle = Vector2.Angle (rHandLoc, Vector2.up);
			rHandLoc.Scale (new Vector2(Screen.width, Screen.height));
			lHandLoc.Scale (new Vector2(Screen.width, Screen.height));
			//if(Vector2.Distance (lHandLoc, rHandLoc)<maxSelectDistance){
			//	pause(false);
			//}
			/*if(rHandAngle <45){
				selectedLevel = 2;
			}
			else if(rHandAngle <135){
				selectedLevel = 1;
			}
			else{
				selectedLevel = 0;
			}*/
			selectedLevel = (int)(1f+((rHandAngle)/(180f/((float)levelGUIs.Count))));
			if (Gestures.LArmStraight()&&Vector3.Angle (Gestures.LArmDir(), Vector3.up)<45){
				LoadLevel(selectedLevel);

			}

		}
		if(!kinectControl){
			if(Input.GetKeyUp (KeyCode.Return)){
				//kMngr.RecalibratePlayer1();
				kMngr.ClearKinectUsers();
				//Application.LoadLevel("Level1");

			}
			if(Input.GetKeyUp (KeyCode.L)){
				LoadLevel(2);

			}
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
		if(Gestures.MenuGesture() && !paused){
			menu = true;
		}
		else{
			menu = false;
		}
		//if (menu && ((Vector3.Angle (rElbow.position-rShoulder.position, Vector3.right) <45)||(!General.kinectControl&&Input.GetKey(KeyCode.P)))){
		if(menu){
			if (Vector3.Angle (rElbow.position-rShoulder.position, Vector3.right) <45){
				if(Gestures.PauseGesture()){
					pause(true);
					currentMenu = menuScreen;
				}
			}

		}
			
		if (paused){

			if(Input.GetKeyUp (KeyCode.T)){
				activeImage = currentInstructions;
			}
			if(Input.GetKeyUp(KeyCode.Y)){
				activeImage = currentMenu;
			}



		}
	}
	void InitializeGestures(){
		
		Gestures.lHand = lHand;
		Gestures.rHand = rHand;
		Gestures.lElbow = lElbow;
		Gestures.rElbow = rElbow;
		Gestures.lShoulder = lShoulder;
		Gestures.rShoulder = rShoulder;

	}
	public void pause(bool p){
		if (p){
			paused = true;
			isPaused = paused;
			//Time.timeScale = .00001f;
		}
		else{
			paused = false;
			isPaused = paused;
			//Time.timeScale = .00001f;
			//Time.timeScale = 1;
		}
	}
	public void changeElement(Element e){
		if(element == e){
			//return;
		}
		element = e;

		airControl.Sleep();
		earthControl.Sleep ();
		fireControl.Sleep();
		waterControl.Sleep ();

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

				if(g.particleSystem){
					g.particleSystem.enableEmission = false;
				}
				else{
					g.SetActive (false);
				}
			}
		}

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
				if(g.particleSystem){
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
				if(!g&&General.dbgMode){
					//Debug.LogWarning ("water avatar object has been destroyed, cannot reactivate");
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
	public void LoadLevel(int lvl){
		if(lvl!= currentLevel){
			currentLevel = lvl;
			if(songs.Count>lvl){
				music.audio.clip = songs[lvl];
				music.audio.Play();
			}
			Application.LoadLevel( lvl);
		}
		else{
			pause(false);
		}
	}
	public static void printJointPos(Transform root){
		string msg = "";
		msg += root.name + ": "+root.position.ToString();
		foreach(Transform c in root.transform){
			msg += "\n"+c.name+": "+c.position.ToString ();
		}
		
		if(General.dbgMode){
			Debug.Log (msg);
		}

	}
	public static float pullEnergy(float amount){
		if (amount > availableEnergy){
			amount = availableEnergy;
			availableEnergy = 0;
			return amount;
		}
		else{
			availableEnergy -= amount;
			return amount;
		}

	}
	public static void pushEnergy(float amount){
		availableEnergy += amount;
	}
	public static float changeSize(float amount, float upperLimit, float lowerLimit){
		if (playerSize + amount > upperLimit){
			//msg += "greater than upper; ";
			playerSize = Mathf.Max (playerSize, upperLimit);
			//return playerSize;
		}
		else if (playerSize + amount < lowerLimit){
			//msg += "less than lower; ";
			playerSize = Mathf.Min (playerSize, lowerLimit);
		}
		else{
			playerSize += amount;
		}
		playerSize = Mathf.Max (playerSize, 0);
		playerSize = Mathf.Min (playerSize, 100);
		//msg +="amount " + amount.ToString () +", t: "+Time.frameCount.ToString ()+", pSize = "+playerSize.ToString ();
		//Debug.Log (msg);

		return playerSize;
	}
	void OnLevelWasLoaded(){
		playerSize = 1;
		availableEnergy = 100;
		pause(false);
		changeElement(Element.Air);
	}
	void OnGUI(){

		if (paused){
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), activeImage);
			if(true||currentLevel !=0){
				for (int i = 0; i<levelGUIs.Count; ++i){
					if(selectedLevel == i+1){
						GUI.DrawTexture (new Rect(Screen.width - 128, i*128, 128, 128), selectedLevelGUIs[i]);
					}
					else{
						GUI.DrawTexture (new Rect(Screen.width - 128, i*128, 128, 128), levelGUIs[i]);
					}
				}
			}
			/*if(selectedLevel == 0){
				GUI.DrawTexture (new Rect(Screen.width - 128, 0, 128, 128), lvl0Selected);

			}
			else{

				GUI.DrawTexture (new Rect(Screen.width - 128, 0, 128, 128), lvl0);
			}
			if(selectedLevel == 1){
				GUI.DrawTexture (new Rect(Screen.width - 128, Screen.height/2, 128, 128), lvl1Selected);
			}
			else{
				GUI.DrawTexture (new Rect(Screen.width - 128, Screen.height/2, 128, 128), lvl1);
			}
			if(selectedLevel == 2){
				GUI.DrawTexture (new Rect(Screen.width - 128, Screen.height-128, 128, 128), lvl2Selected);
			}
			else{
				GUI.DrawTexture (new Rect(Screen.width - 128, Screen.height-128, 128, 128), lvl2);
			}*/
			//GUI.DrawTexture (new Rect((Screen.width/2)+(1000*Gestures.RArmDir().x), (Screen.height/2)-(1000*Gestures.RArmDir().y), 64,64), selector);
			GUI.DrawTexture (new Rect(lHandLoc.x+(Screen.width/2), lHandLoc.y+(Screen.height/2), 64,64), selector);
			GUI.DrawTexture (new Rect(rHandLoc.x+(Screen.width/2), rHandLoc.y+(Screen.height/2), 64,64), selector);


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
		if(!kMngr.IsPlayerCalibrated(1)){
			GUI.DrawTexture (new Rect (0,0, Screen.width/4, Screen.height/4), notCalibrated);
		}

	}
}
