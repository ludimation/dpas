using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Air : MonoBehaviour {
	public Kinectalogue controller;
	public AudioSource audSrc;
	public AudioClip launch;
	public ParticleSystem part;
	public CharacterMotor charMotor;
	public basicControl baseControl;
	public Vector3 partRot = Vector3.forward;
	//public bool ionized = false;
	public CloudFinder cF;
	public LightningStrike leftStrike;
	public LightningStrike rightStrike;
	//public float ionizationHeight = 200;
	public float grav = 25f;

	public Vector3 cycloneSize = Vector3.one;
	//public LightningStrike bolt;
	//public LightningStrike zap;

	//public float upDraft = 0f;
	
	//public bool isAwake = false;


	public float lightningCharge = 0f;
	public float chargeTime = 5f;

	Vector3 lHandOld;
	Vector3 rHandOld;
	public Transform lHand;
	public Transform rHand;
	public bool enableGust = true;
	public AirAttack gust;
	public float gustSpeed = 1;
	public float gustWait = .1f;
	float gustT;
	public List<AudioClip> gustSounds;
	public float gustCost = 0; //does nothing for now
	public float minThrowSpeed = 2.5f;//does nothing I think for now

	public bool enableRain = true;
	public List<AudioClip> rainSounds;
	public float rainWait = 0;
	float rainT;
	public float rainCost = 0;

	public bool enableLightning = true;
	public List<AudioClip> lightningSounds;
	public float lightningWait = 0;
	float lightningT;
	public float lightningCost; //dos nothing for now

	public Texture2D gustIcon;
	public Texture2D gustActivatedIcon;
	public Texture2D lightningIcon;
	public Texture2D lightningActivatedIcon;
	public Texture2D rainIcon;
	public Texture2D rainActivatedIcon;
	public float iconSize = 64;

	// Use this for initialization
	void Start () {
		gustT = gustWait;
		lightningT = lightningWait;
		rainT = rainWait;
	}
	
	// Update is called once per frame
	void Update () {
		if(!General.isPaused){
			gustT -= Time.deltaTime;
			lightningT -= Time.deltaTime;
			rainT -= Time.deltaTime;

			if (Gestures.ArmsUp ()){
				//General.screenShake.NewImpact ();
				Rain();
			}
			else if(Gestures.ArmsTogether() && !Gestures.ArmsDown()){
				WindGust(.5f*(lHand.position +rHand.position), transform.rotation*Gestures.CommonDir ());
			}
			//Debug.Log ("lArmStraight = "+Gestures.LArmStraight ().ToString ()+", LArmDirAngle = " + Vector3.Angle(Gestures.LArmDir(), Vector3.up));
			if(Gestures.LArmStraight ()&&Vector3.Angle (Gestures.LArmDir(), Vector3.up)<45&&Gestures.RArmStraight ()&&Vector3.Angle (Gestures.RArmDir(), Vector3.up)>60){
				//General.screenShake.NewImpact ();
				rightStrike.gameObject.SetActive (true);
				//rightStrike.enabled = true;
			}
			else{
				rightStrike.gameObject.SetActive(false);
				//rightStrike.enabled = false;
			}
			if(Gestures.RArmStraight ()&&Vector3.Angle (Gestures.RArmDir(), Vector3.up)<45&&Gestures.LArmStraight ()&&Vector3.Angle (Gestures.LArmDir(), Vector3.up)>60){
				//General.screenShake.NewImpact ();
				leftStrike.gameObject.SetActive (true);
				//leftStrike.enabled = true;
			}
			else{
				leftStrike.gameObject.SetActive(false);
				//leftStrike.enabled = false;
			}
			if(General.dbgMode){
	//			Debug.Log ((Vector3.Distance (lHandOld, Gestures.LArmDir())/Time.deltaTime).ToString ()+", "+(Vector3.Distance (rHandOld, Gestures.RArmDir())/Time.deltaTime).ToString ());
			}



			lHandOld = Gestures.LArmDir();
			rHandOld = Gestures.RArmDir();
			

			if(!General.kinectControl){
				
				if (Input.GetKey(KeyCode.Space)){
					General.changeSize(3*Time.deltaTime, 100, 0);
				}

				if(Input.GetKey (KeyCode.B)){
					General.changeSize(-3*Time.deltaTime, 100, 0);
				}
				if (Input.GetKey (KeyCode.R)){
					//General.screenShake.NewImpact ();
					//cF.Rain (Time.deltaTime);
					Rain();
				}


				if(Input.GetKeyDown (KeyCode.E)){
					/*General.screenShake.NewImpact ();
					AirAttack temp = (AirAttack)Instantiate(gust, rHand.position, Quaternion.identity);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					foo.AddForce (gustSpeed*transform.forward, ForceMode.VelocityChange);*/
					WindGust(rHand.position, transform.forward);

					
				}


				if(Input.GetKeyDown (KeyCode.Q)){
					/*General.screenShake.NewImpact ();
					AirAttack temp = (AirAttack)Instantiate(gust, lHand.position, Quaternion.identity);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					foo.AddForce (gustSpeed*transform.forward, ForceMode.VelocityChange);*/
					WindGust(lHand.position, transform.forward);
					
				}

				if(Input.GetKey(KeyCode.Alpha1)){
					rightStrike.gameObject.SetActive (true);
					//rightStrike.enabled = true;
				}
				if(Input.GetKeyUp (KeyCode.Alpha1)){
					//rightStrike.gameObject.SetActive (false);
					//rightStrike.enabled = false;
				}

				if(Input.GetKey (KeyCode.Alpha3)){
					leftStrike.gameObject.SetActive (true);
					//leftStrike.enabled = true;
				}
				if(Input.GetKeyUp (KeyCode.Alpha3)){
					//leftStrike.gameObject.SetActive (false);
					//leftStrike.enabled = false;
				}

			}
		}
	}

	void WindGust(Vector3 pos, Vector3 dir){

		if(enableGust&&gustT < 0){

			gustT = gustWait;
			General.screenShake.NewImpact ();
			AirAttack temp = (AirAttack)Instantiate(gust, pos, Quaternion.identity);
		//	General.changeSize (gustCost, 100, 0);
			temp.rigidbody.AddForce (gustSpeed*dir, ForceMode.VelocityChange);
			audSrc.PlayOneShot (gustSounds[Random.Range(0,gustSounds.Count)]);

		}
	}
	public bool CastLightning(){
		if(enableLightning && lightningT< 0){
			lightningT = lightningWait;
			General.screenShake.NewImpact ();
			audSrc.PlayOneShot (lightningSounds[Random.Range(0, lightningSounds.Count)]);
			//cF.Rain (Time.deltaTime);
			return true;

		}
		else{
			return false;
		}
	}
	public void Rain(){
		if(enableRain && rainT<0){
			rainT = rainWait;
			General.screenShake.NewImpact();
			cF.Rain (Time.deltaTime);
			if(rainSounds.Count>0){
				audSrc.PlayOneShot (rainSounds[Random.Range (0,rainSounds.Count)]);
			}
		}
	}
	public void Sleep(){
		//isAwake = false;
		charMotor.enabled = true;
		//ionized = false;
		baseControl.fly = false;
	}
	public void UnSleep(){
		//isAwake = true;
		charMotor.enabled = false;
		baseControl.fly = true;


	}
	void OnTriggerStay(Collider other){
		if (enabled){
			//Void OnTriggerEnter(Collider other){
			RockThing r = other.GetComponent<RockThing>();
			if(r){
				General.g.changeElement(General.Element.Earth);
				//General.playerSize = 1
				return;
			}
			//}
			FireAttack fA = other.GetComponent<FireAttack>();
			if(fA){
				General.g.changeElement(General.Element.Fire);
				General.playerSize = 1f;
			}
			Shrubbery shrub = other.GetComponent<Shrubbery>();
			if(shrub&&shrub.state>2){
				Debug.Log ("shrubbery encountered, changing form");
				General.g.changeElement(General.Element.Fire);
				General.playerSize = 1f;
				return;
			}
			WaterAttack wA = other.GetComponent<WaterAttack>();
			if(wA){
				General.g.changeElement(General.Element.Water);
				General.playerSize = 1f;
				return;
			}
			Stream strm = other.GetComponent<Stream>();
			if(strm){
				General.g.changeElement(General.Element.Water);
				General.playerSize = 1f;
				return;
			}
		}

	}
	void OnGUI(){
		if(General.icons){
			if(gustT < 0){
				GUI.DrawTexture(new Rect(0, 0, iconSize, iconSize), gustIcon);
			}
			else{
				GUI.DrawTexture(new Rect(0, 0, iconSize, iconSize), gustActivatedIcon);
			}
			
			if(lightningT < 0){
				GUI.DrawTexture(new Rect(iconSize, 0, iconSize, iconSize), lightningIcon);
			}
			else{
				GUI.DrawTexture(new Rect(iconSize, 0, iconSize, iconSize), lightningActivatedIcon);
			}
			if(rainT < 0){
				GUI.DrawTexture(new Rect(iconSize*2, 0, iconSize, iconSize), rainIcon);
			}
			else{
				GUI.DrawTexture(new Rect(iconSize*2, 0, iconSize, iconSize), rainActivatedIcon);
			}
		}
	}
}
