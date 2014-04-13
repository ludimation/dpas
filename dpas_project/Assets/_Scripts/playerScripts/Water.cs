﻿using UnityEngine;
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
	public WaterAttack wave;
	public float waveCost;
	public bool enableWave = true;
	public WaterAttack geyser;
	public float geyserCost;
	public bool enableGeyser = true;
	public WaterAttack waterJet;
	public float waterJetCost;
	public bool enableWaterJet = true;
	public float jetWait = .3f;
	float jetT = .3f;

	public float waveSpeed;
	Vector3 lHandOld;
	Vector3 rHandOld;
	public CharacterMotor charMotor;
	// Use this for initialization
	void Start () {
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
			//Debug.DrawRay (transform.position, Gestures.flatShoulderRot()*Vector3.forward);
			if(jetT < 0 && enableWaterJet&&Gestures.ArmsTogether ()&&!Gestures.ArmsDown ()){
				jetT = jetWait;
				//WaterAttack temp = (WaterAttack)Instantiate(WaterBlast, transform.position+(2*transform.forward)+Vector3.Up, transform.rotation);
				WaterAttack temp = (WaterAttack)Instantiate(wave, .5f*(lHand.position+rHand.position), transform.rotation);
				General.changeSize (-temp.size*waterJetCost, 100, 0);
				//Physics.IgnoreCollision (temp.collider, collider);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
				//General.changeSize (flamethrowerCost/Time.deltaTime);
				//temp.strength = General.playerSize;
				//flameWait = flameDelay;
				
			}
			if (enableGeyser&&Vector3.Angle (Gestures.LArmDir(), new Vector3(-1,-1,-1))<45 && Vector3.Angle (Gestures.RArmDir(), new Vector3(1,-1,-1))<45){
				General.screenShake.NewImpact ();
				//jetT = jetWait;
				if(jetT < 0 ){
					WaterAttack temp = (WaterAttack)Instantiate(wave, .5f*(lHand.position+rHand.position), transform.rotation);
				//Physics.IgnoreCollision (temp.collider, collider);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
					jetT = jetWait;
				}
				//General.changeSize (rocketJumpCost/Time.deltaTime);
				//temp.strength = General.playerSize;
				charMotor.inputJump = true;
				//gameObject.GetComponent<CharacterMotor>().inputJump = true;
			}
			else {
				charMotor.inputJump = false;
			}

			if(enableWave&&Vector3.Angle (lHandOld - Gestures.LArmDir(), lHandOld)>60){
				//Debug.Log ("L wave");
				if(Vector3.Distance (lHandOld, Gestures.LArmDir ())> minThrowSpeed*Time.deltaTime){
					General.screenShake.NewImpact ();
					//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					WaterAttack temp = (WaterAttack)Instantiate(wave, lHand.position, Quaternion.identity);
					General.changeSize (-temp.size*waveCost, 100, 0);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					//foo.AddForce (waveSpeed*(transform.rotation*(Gestures.LArmDir()-lHandOld)), ForceMode.VelocityChange);
					foo.AddForce (waveSpeed*(transform.rotation*Gestures.LArmDir()), ForceMode.VelocityChange);
					//foo.AddForce (waveSpeed*Gestures.LArmDir(), ForceMode.VelocityChange);
					//General.changeSize (waveCost/Time.deltaTime);
					//temp.strength = General.playerSize;
					
					
				}
			}
			if(enableWave&&Vector3.Angle (rHandOld - Gestures.RArmDir(), rHandOld)>60){
				//Debug.Log ("R wave");
				if(Vector3.Distance (rHandOld, Gestures.RArmDir ())> minThrowSpeed*Time.deltaTime){
					General.screenShake.NewImpact ();
					//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					WaterAttack temp = (WaterAttack)Instantiate(wave, rHand.position, Quaternion.identity);
					General.changeSize (-temp.size*waveCost, 100, 0);
					//WaterAttack temp = (WaterAttack)Instantiate(wave, transform.position+(transform.rotation*(RArmDir()-rHandOld)), Quaternion.identity);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					//foo.AddForce (waveSpeed*(transform.rotation*(Gestures.RArmDir()-rHandOld)), ForceMode.VelocityChange);
					foo.AddForce (waveSpeed*(transform.rotation*Gestures.RArmDir()), ForceMode.VelocityChange);
					//General.changeSize (waveCost/Time.deltaTime);
					//temp.strength = General.playerSize;
					
				}
			}
			//Debug.Log ((Vector3.Distance (lHandOld, Gestures.LArmDir())/Time.deltaTime).ToString ()+", "+(Vector3.Distance (rHandOld, Gestures.RArmDir())/Time.deltaTime).ToString ());
			lHandOld = Gestures.LArmDir();
			rHandOld = Gestures.RArmDir();
			
			if(!General.kinectControl){
				if(Input.GetKeyDown (KeyCode.E)){
					General.screenShake.NewImpact ();
					WaterAttack temp = (WaterAttack)Instantiate(wave, rHand.position, Quaternion.identity);
					General.changeSize (-temp.size*waveCost, 100, 0);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					foo.AddForce (waveSpeed*transform.forward, ForceMode.VelocityChange);
					
				}
				if(Input.GetKeyDown (KeyCode.Q)){
					General.screenShake.NewImpact ();
					WaterAttack temp = (WaterAttack)Instantiate(wave, lHand.position, Quaternion.identity);
					General.changeSize (-temp.size*waveCost, 100, 0);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					foo.AddForce (waveSpeed*transform.forward, ForceMode.VelocityChange);
					
				}
				if(Input.GetKey(KeyCode.LeftShift)){
					if(jetT < 0){
						General.screenShake.NewImpact ();
						WaterAttack temp = (WaterAttack)Instantiate(geyser, .5f*(lHand.position+rHand.position), transform.rotation);
						General.changeSize (-temp.size*geyserCost, 100, 0);
						//Physics.IgnoreCollision (temp.collider, collider);
						Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
						//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
						foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
					}
					//General.changeSize (rocketJumpCost/Time.deltaTime);
					//temp.strength = General.playerSize;
					charMotor.inputJump = true;
					//gameObject.GetComponent<CharacterMotor>().inputJump = true;
				}

				if(jetT < 0 && Input.GetKey (KeyCode.X)){
					General.screenShake.NewImpact ();
					WaterAttack temp = (WaterAttack)Instantiate(waterJet, .5f*(lHand.position+rHand.position), transform.rotation);
					General.changeSize (-temp.size*waterJetCost, 100, 0);
					//Physics.IgnoreCollision (temp.collider, collider);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
				}
			}
		}
	}
	public void Sleep(){
		//called before deactivating script
		isAwake = false;
		
	}
	public void UnSleep(){
		//called when activating the script
		isAwake = true;
	}

	//void OnCollisionStay(){

	//}

}
