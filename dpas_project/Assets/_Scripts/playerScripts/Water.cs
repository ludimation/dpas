using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Water : MonoBehaviour {

	//public GameObject effect;
	//public List<Transform>
	public List<GameObject> effects;
	public Transform lHand;
	public Transform rHand;
	public float minThrowSpeed = 2.5f;
	public bool isAwake = false;
	
	public bool enableWave = true;
	public WaterAttack wave;
	public float waveCost = 0;
	public float waveWait = 0;
	public float wave1T = 0;
	public float wave2T = 0;
	public List<AudioClip> waveSounds;
	public float waveSpeed = 1;
	
	public bool enableGeyser = true;
	public WaterAttack geyser;
	public float geyserCost = 0;
	public float geyserWait = 0;
	public float geyserT = 0;
	public List<AudioClip> geyserSounds;
	public float geyserSpeed = 1;
	
	public bool enableJet = true;
	public WaterAttack waterJet;
	public float waterJetCost = 0;
	public float jetWait = .3f;
	float jetT = .3f;
	public List<AudioClip> jetSounds;
	public float jetSpeed;

	//public float waveSpeed;
	Vector3 lHandOld;
	Vector3 rHandOld;
	public CharacterMotor charMotor;
	public AudioSource audSrc;

	public Texture2D leftThrowIcon;
	public Texture2D rightThrowIcon;
	//public Texture2D leftReadyIcon;
	//public Texture2D rightReadyIcon;
	public Texture2D leftPrepIcon;
	public Texture2D rightPrepIcon;
	public Texture2D geyserIcon;
	public Texture2D geyserActivatedIcon;
	public Texture2D waterJetIcon;
	public Texture2D waterJetActivatedIcon;
	public float iconSize = 64;

	// Use this for initialization
	void Start () {
		//if(!audSrc){
		//	audSrc = gameObject.GetComponent<audSrc>();
		//}
		jetT = jetWait;
		wave1T = waveWait;
		wave2T = waveWait;
		geyserT = geyserWait;
		if (!charMotor){
			charMotor = gameObject.GetComponent<CharacterMotor>();
		}
		lHandOld = Gestures.LArmDir ();
		rHandOld = Gestures.RArmDir ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!General.isPaused){
			transform.localScale = (1+(.1f*General.playerSize))*Vector3.one;
			if (General.playerSize <.01f){
				General.g.changeElement (General.Element.Air);
			}
			jetT -= Time.deltaTime;
			wave1T -= Time.deltaTime;
			wave2T -= Time.deltaTime;
			geyserT -= Time.deltaTime;
			//Debug.DrawRay (transform.position, Gestures.flatShoulderRot()*Vector3.forward);
			if(Gestures.ArmsTogether ()&&!Gestures.ArmsDown ()){
				/*jetT = jetWait;
				//WaterAttack temp = (WaterAttack)Instantiate(WaterBlast, transform.position+(2*transform.forward)+Vector3.Up, transform.rotation);
				WaterAttack temp = (WaterAttack)Instantiate(wave, .5f*(lHand.position+rHand.position), transform.rotation);
				General.changeSize (-temp.size*waterJetCost, 100, 0);
				//Physics.IgnoreCollision (temp.collider, collider);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
				//General.changeSize (flamethrowerCost/Time.deltaTime);
				//temp.strength = General.playerSize;
				//flameWait = flameDelay;*/
				Jet(.5f*(lHand.position+rHand.position), transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()));
			
				
			}
			if (Vector3.Angle (Gestures.LArmDir(), new Vector3(-1,-1,-1))<45 && Vector3.Angle (Gestures.RArmDir(), new Vector3(1,-1,-1))<45){
				//General.screenShake.NewImpact ();
				//jetT = jetWait;
				//if(jetT < 0 ){
					/*WaterAttack temp = (WaterAttack)Instantiate(wave, .5f*(lHand.position+rHand.position), transform.rotation);
				//Physics.IgnoreCollision (temp.collider, collider);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
					jetT = jetWait;*/
				Geyser(.5f*(lHand.position+rHand.position), transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()));
				//}
				//General.changeSize (rocketJumpCost/Time.deltaTime);
				//temp.strength = General.playerSize;
				charMotor.inputJump = true;
				//gameObject.GetComponent<CharacterMotor>().inputJump = true;
			}
			else {
				charMotor.inputJump = false;
			}

			if(Vector3.Angle (lHandOld - Gestures.LArmDir(), lHandOld)>60){
				//Debug.Log ("L wave");
				if(Vector3.Distance (lHandOld, Gestures.LArmDir ())> minThrowSpeed*Time.deltaTime){
					/*General.screenShake.NewImpact ();
					//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					WaterAttack temp = (WaterAttack)Instantiate(wave, lHand.position, Quaternion.identity);
					General.changeSize (-temp.size*waveCost, 100, 0);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					//foo.AddForce (waveSpeed*(transform.rotation*(Gestures.LArmDir()-lHandOld)), ForceMode.VelocityChange);
					foo.AddForce (waveSpeed*(transform.rotation*Gestures.LArmDir()), ForceMode.VelocityChange);
					//foo.AddForce (waveSpeed*Gestures.LArmDir(), ForceMode.VelocityChange);
					//General.changeSize (waveCost/Time.deltaTime);
					//temp.strength = General.playerSize;*/
					
					Wave1(lHand.position, transform.rotation*Gestures.LArmDir());
					
					
				}
			}
			if(Vector3.Angle (rHandOld - Gestures.RArmDir(), rHandOld)>60){
				//Debug.Log ("R wave");
				if(Vector3.Distance (rHandOld, Gestures.RArmDir ())> minThrowSpeed*Time.deltaTime){
					/*
					General.screenShake.NewImpact ();
					//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					WaterAttack temp = (WaterAttack)Instantiate(wave, rHand.position, Quaternion.identity);
					General.changeSize (-temp.size*waveCost, 100, 0);
					//WaterAttack temp = (WaterAttack)Instantiate(wave, transform.position+(transform.rotation*(RArmDir()-rHandOld)), Quaternion.identity);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					//foo.AddForce (waveSpeed*(transform.rotation*(Gestures.RArmDir()-rHandOld)), ForceMode.VelocityChange);
					foo.AddForce (waveSpeed*(transform.rotation*Gestures.RArmDir()), ForceMode.VelocityChange);
					//General.changeSize (waveCost/Time.deltaTime);
					//temp.strength = General.playerSize;*/
					Wave2(rHand.position, transform.rotation*Gestures.RArmDir());
					
				}
			}
			//Debug.Log ((Vector3.Distance (lHandOld, Gestures.LArmDir())/Time.deltaTime).ToString ()+", "+(Vector3.Distance (rHandOld, Gestures.RArmDir())/Time.deltaTime).ToString ());
			lHandOld = Gestures.LArmDir();
			rHandOld = Gestures.RArmDir();
			
			if(!General.kinectControl){
				if(Input.GetKeyDown (KeyCode.E)){
					Wave2(rHand.position, transform.forward);
				}
				if(Input.GetKeyDown (KeyCode.Q)){
					/*General.screenShake.NewImpact ();
					WaterAttack temp = (WaterAttack)Instantiate(wave, lHand.position, Quaternion.identity);
					General.changeSize (-temp.size*waveCost, 100, 0);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					foo.AddForce (waveSpeed*transform.forward, ForceMode.VelocityChange);*/
					Wave1(lHand.position, transform.forward);

					
				}
				if(Input.GetKey(KeyCode.LeftShift)){
					if(jetT < 0){
						Geyser(.5f*(lHand.position+rHand.position), transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()));
					}
					charMotor.inputJump = true;
					}

				if(jetT < 0 && Input.GetKey (KeyCode.X)){
					Jet(.5f*(lHand.position+rHand.position), transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()));

				}
			}
		}
	}
	void Wave1(Vector3 pos, Vector3 dir){
		if(enableWave && wave1T<0){
			wave1T = waveWait;
			General.screenShake.NewImpact ();
			WaterAttack temp = (WaterAttack)Instantiate(wave, pos, Quaternion.identity);
			General.changeSize (-temp.size*waveCost, 100, 0);
			temp.rigidbody.AddForce (waveSpeed*dir, ForceMode.VelocityChange);
			if(waveSounds.Count > 0){
				audSrc.PlayOneShot(waveSounds[Random.Range (0, waveSounds.Count)]);
			}
		}
		
	}
	void Wave2(Vector3 pos, Vector3 dir){
		if(enableWave && wave2T<0){
			wave2T = waveWait;
			General.screenShake.NewImpact ();
			WaterAttack temp = (WaterAttack)Instantiate(wave, pos, Quaternion.identity);
			General.changeSize (-temp.size*waveCost, 100, 0);
			temp.rigidbody.AddForce (waveSpeed*dir, ForceMode.VelocityChange);
			if(waveSounds.Count > 0){
				audSrc.PlayOneShot(waveSounds[Random.Range (0, waveSounds.Count)]);
			}
		}
		
	}

	void Jet(Vector3 pos, Vector3 dir){
		if (enableJet && jetT < 0){
			jetT = jetWait;
			General.screenShake.NewImpact ();
			WaterAttack temp = (WaterAttack)Instantiate(waterJet, pos, transform.rotation);
			General.changeSize (-temp.size*waterJetCost, 100, 0);
			temp.rigidbody.AddForce (dir*jetSpeed, ForceMode.VelocityChange);
			if(jetSounds.Count > 0){
				audSrc.PlayOneShot(jetSounds[Random.Range (0, jetSounds.Count)]);
			}
		}

	}

	void Geyser(Vector3 pos, Vector3 dir){
		if(enableGeyser && geyserT < 0){
			geyserT = geyserWait;
			General.screenShake.NewImpact ();
			WaterAttack temp = (WaterAttack)Instantiate(geyser, pos, transform.rotation);
			General.changeSize (-temp.size*geyserCost, 100, 0);
			temp.rigidbody.AddForce (dir*geyserSpeed, ForceMode.VelocityChange);
			if(geyserSounds.Count > 0){
				audSrc.PlayOneShot(geyserSounds[Random.Range (0, geyserSounds.Count)]);
			}
		}
	}
	public void Sleep(){
		//called before deactivating script
		//isAwake = false;
		
	}
	public void UnSleep(){
		//called when activating the script
		//isAwake = true;
	}
	//void OnLevelWasLoaded(){

	//}
	//void OnCollisionStay(){

	//}
	void OnGUI(){
		if(wave1T <0){
			GUI.DrawTexture(new Rect(iconSize*2, 0, iconSize, iconSize), leftPrepIcon);
		}
		else{
			GUI.DrawTexture(new Rect(iconSize*2, 0, iconSize, iconSize), leftThrowIcon);
		}
		if(wave2T <0){
			GUI.DrawTexture(new Rect(iconSize*2, 0, iconSize, iconSize), rightPrepIcon);
		}
		else{
			GUI.DrawTexture(new Rect(iconSize*2, 0, iconSize, iconSize), rightThrowIcon);
		}
		if(jetT < 0){
			GUI.DrawTexture(new Rect(iconSize*2, 0, iconSize, iconSize), waterJetIcon);
		}
		else{
			GUI.DrawTexture(new Rect(iconSize*2, 0, iconSize, iconSize), waterJetActivatedIcon);
		}
		
		if(geyserT < 0){
			GUI.DrawTexture(new Rect(iconSize*3, 0, iconSize, iconSize), geyserIcon);
		}
		else{
			GUI.DrawTexture(new Rect(iconSize*3, 0, iconSize, iconSize), geyserActivatedIcon);
		}
	}
}
