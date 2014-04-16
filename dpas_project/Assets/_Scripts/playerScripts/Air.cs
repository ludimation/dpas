﻿using UnityEngine;
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

	public bool enableLightning = true;
	public List<AudioClip> lightningSounds;
	public float lightningWait = 0;
	float lightningT;
	float lightningCost; //dos nothing for now


	// Use this for initialization
	void Start () {
		gustT = gustWait;
		lightningT = lightningWait;
	}
	
	// Update is called once per frame
	void Update () {
		gustT -= Time.deltaTime;
		lightningT -= Time.deltaTime;

		if (Gestures.ArmsUp ()){
			General.screenShake.NewImpact ();
			cF.Rain(Time.deltaTime);
		}
		else if(Gestures.ArmsTogether() && !Gestures.ArmsDown()){
			WindGust(.5f*(lHand.position +rHand.position), transform.rotation*Gestures.CommonDir ());
		}
		//Debug.Log ("lArmStraight = "+Gestures.LArmStraight ().ToString ()+", LArmDirAngle = " + Vector3.Angle(Gestures.LArmDir(), Vector3.up));
		if(Gestures.LArmStraight ()&&Vector3.Angle (Gestures.LArmDir(), Vector3.up)<45&&Gestures.RArmStraight ()&&Vector3.Angle (Gestures.RArmDir(), Vector3.up)>60){
			General.screenShake.NewImpact ();
			rightStrike.gameObject.SetActive (true);
			//rightStrike.enabled = true;
		}
		else{
			rightStrike.gameObject.SetActive(false);
			//rightStrike.enabled = false;
		}
		if(Gestures.RArmStraight ()&&Vector3.Angle (Gestures.RArmDir(), Vector3.up)<45&&Gestures.LArmStraight ()&&Vector3.Angle (Gestures.LArmDir(), Vector3.up)>60){
			General.screenShake.NewImpact ();
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
				General.screenShake.NewImpact ();
				cF.Rain (Time.deltaTime);
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
	public void CastLightning(){
		if(enableLightning && lightningT< 0){
			lightningT = lightningWait;
			General.screenShake.NewImpact ();
			audSrc.PlayOneShot (lightningSounds[Random.Range(0, lightningSounds.Count)]);
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
			if(Gestures.ArmsOut ()){
				FireAttack fA = other.GetComponent<FireAttack>();
				if(fA){
					General.g.changeElement(General.Element.Fire);
					General.playerSize = 1f;
				}
				Shrubbery shrub = other.GetComponent<Shrubbery>();
				if(shrub&&shrub.burning){
					Debug.Log ("shrubbery encountered, changing form");
					General.g.changeElement(General.Element.Fire);
					General.playerSize = 1f;
				}
				WaterAttack wA = other.GetComponent<WaterAttack>();
				if(wA){
					General.g.changeElement(General.Element.Water);
					General.playerSize = 1f;
				}
				Stream strm = other.GetComponent<Stream>();
				if(strm){
					General.g.changeElement(General.Element.Water);
					General.playerSize = 1f;
				}
			}
		}
	}

}
