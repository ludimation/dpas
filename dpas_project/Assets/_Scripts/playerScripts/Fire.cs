﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {
	public Kinectalogue controller;
	public AudioSource AudSrc;
	public AudioClip launch;
	public List<AudioClip> launchSounds;
	public GameObject projectile;
	bool primed = false;
	public FireAttack fireBlast;
	public Transform lHand;
	public Transform rHand;
	public List<GameObject> effects;

	public FireAttack flamethrower;
	public float flamethrowerCost = 1f;
	public FireAttack fireball;
	public float fireballCost = 1f;
	public FireAttack rocketJump;
	public float rocketJumpCost = 1f;

	Vector3 lHandOld;
	Vector3 rHandOld;
	public float tolerance = .02f;
	public float fireballSpeed = 1f;
	public float flameDelay = .2f;
	float flameWait;

	public bool isAwake = false;

	public List<ParticleSystem> flames;
	public CharacterMotor charMotor;

	//public FireAttack fireBlast;

	//public float size;
	// Use this for initialization
	void Start () {
		flameWait = flameDelay;
		lHandOld = Gestures.LArmDir();
		rHandOld = Gestures.RArmDir();
		if (charMotor == null){
			charMotor = gameObject.GetComponent<CharacterMotor>();
		}
		//rootOld = root.position;

	}
	
	// Update is called once per frame
	void Update () {
		if(!General.isPaused){
			transform.localScale = (1+(.1f*General.playerSize))*Vector3.one;

			if (General.playerSize <.01f){
				General.g.changeElement (General.Element.Air);
			}
			flameWait -= Time.deltaTime;
			if(!General.kinectControl){
				if (Input.GetKey(KeyCode.Space)){
					//General.playerSize += Time.deltaTime;
					General.changeSize(Time.deltaTime, 100, 0);

				}
				if(Input.GetKey (KeyCode.B)){
					General.changeSize(-Time.deltaTime, 100, 0);
					//General.playerSize -= Time.deltaTime;
				}
			}
			adjustFlames(Mathf.Min (General.playerSize, 10));
			
			//if(Vector3.Angle (lHandOld, Gestures.LArmDir())>60){
			if(Vector3.Angle (lHandOld - Gestures.LArmDir(), lHandOld)>60){
				//Debug.Log ("L fireball");
				if(Vector3.Distance (lHandOld, Gestures.LArmDir ())> tolerance*Time.deltaTime){
					General.screenShake.NewImpact ();
					AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					FireAttack temp = (FireAttack)Instantiate(fireball, lHand.position, Quaternion.identity);
					General.changeSize (-Time.deltaTime*fireballCost, 100, 0);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					foo.AddForce (fireballSpeed*(transform.rotation*(Gestures.LArmDir()-lHandOld).normalized), ForceMode.VelocityChange);
					//foo.AddForce (fireballSpeed*Gestures.LArmDir(), ForceMode.VelocityChange);
					//General.changeSize (fireballCost/Time.deltaTime);
					temp.strength = General.playerSize;


				}
			}
			if(Vector3.Angle (rHandOld - Gestures.RArmDir(), rHandOld)>60){
				//Debug.Log ("R fireball");
				if(Vector3.Distance (rHandOld, Gestures.RArmDir ())> tolerance*Time.deltaTime){
					General.screenShake.NewImpact ();
					AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					FireAttack temp = (FireAttack)Instantiate(fireball, rHand.position, Quaternion.identity);
					General.changeSize (-Time.deltaTime*fireballCost, 100, 0);
					//FireAttack temp = (FireAttack)Instantiate(fireball, transform.position+(transform.rotation*(RArmDir()-rHandOld)), Quaternion.identity);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					foo.AddForce (fireballSpeed*(transform.rotation*((Gestures.RArmDir()-rHandOld).normalized)), ForceMode.VelocityChange);
					//General.changeSize (fireballCost/Time.deltaTime);
					temp.strength = General.playerSize;
					
				}
			}
			Debug.Log ("armsTogether = "+Gestures.ArmsTogether().ToString ()+", armsDown = "+Gestures.ArmsDown().ToString()+", dir = "+(transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir())).ToString());
			if(flameWait<0&&Gestures.ArmsTogether () && !Gestures.ArmsDown()){
				General.screenShake.NewImpact ();
				General.changeSize (-Time.deltaTime*flamethrowerCost, 100, 0);
				AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				
				FireAttack temp = (FireAttack)Instantiate(flamethrower, transform.position+(2*transform.forward)+Vector3.up, transform.rotation);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				//foo.AddForce (transform.forward*fireballSpeed, ForceMode.VelocityChange);
				foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
				//General.changeSize (flamethrowerCost/Time.deltaTime);
				temp.strength = General.playerSize;
				/*General.screenShake.NewImpact ();
				//FireAttack temp = (FireAttack)Instantiate(fireBlast, transform.position+(2*transform.forward)+Vector3.Up, transform.rotation);
				General.changeSize (-Time.deltaTime*flamethrowerCost, 100, 0);
				FireAttack temp = (FireAttack)Instantiate(flamethrower, .5f*(lHand.position+rHand.position).normalized, transform.rotation);
				//Physics.IgnoreCollision (temp.collider, collider);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				//foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
				foo.AddForce (transform.forward*fireballSpeed, ForceMode.VelocityChange);
				Debug.Log (transform.forward.ToString());
				//General.changeSize (flamethrowerCost/Time.deltaTime);
				temp.strength = General.playerSize;
				flameWait = flameDelay;*/
				
			}

			//if(Gestures.LArmStraight()&&Gestures.RArmStraight ()&&
			if (Vector3.Angle (Gestures.flatShoulderRot ()*Gestures.LArmDir(), new Vector3(-1,-1,-1))<45 && Vector3.Angle (Gestures.flatShoulderRot ()*Gestures.RArmDir(), new Vector3(1,-1,-1))<45){
				if(flameWait<0){
					General.screenShake.NewImpact ();
					General.changeSize (-Time.deltaTime*rocketJumpCost, 100, 0);
					FireAttack temp = (FireAttack)Instantiate(fireball, .5f*(lHand.position+rHand.position), transform.rotation);
					Physics.IgnoreCollision (temp.collider, collider);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
					//General.changeSize (rocketJumpCost/Time.deltaTime);
					temp.strength = General.playerSize;
					flameWait = flameDelay;
				}
				charMotor.inputJump = true;
				//gameObject.GetComponent<CharacterMotor>().inputJump = true;
			}
			else{			
				charMotor.inputJump = false;
				//gameObject.GetComponent<CharacterMotor>().inputJump = false;
			}
			if(!General.kinectControl){
				//Debug.Log (Time.deltaTime.ToString ());
				if (Input.GetKeyDown (KeyCode.Q)){
					General.changeSize (-Time.deltaTime, 100, 0);
					General.screenShake.NewImpact ();
					Debug.Log ("Q");
					AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					FireAttack temp = (FireAttack)Instantiate (fireball, (lHand.position), Quaternion.identity);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					foo.AddForce (transform.forward*fireballSpeed, ForceMode.VelocityChange);
					//General.changeSize (fireballCost*Time.deltaTime, 50, 1);
					temp.strength = General.playerSize;
				}
				if(Input.GetKeyDown (KeyCode.E)){
					General.changeSize (-Time.deltaTime, 100, 0);
					General.screenShake.NewImpact ();
					Debug.Log ("E");
					AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					FireAttack temp = (FireAttack)Instantiate (fireball, (rHand.position), Quaternion.identity);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					foo.AddForce (transform.forward*fireballSpeed, ForceMode.VelocityChange);
					//General.changeSize (fireballCost/Time.deltaTime);
					temp.strength = General.playerSize;
				}
				if (Input.GetKey (KeyCode.X)){
					General.screenShake.NewImpact ();
					General.changeSize (-Time.deltaTime*flamethrowerCost, 100, 0);
					AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);

					FireAttack temp = (FireAttack)Instantiate(flamethrower, transform.position+(2*transform.forward)+Vector3.up, transform.rotation);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					foo.AddForce (transform.forward*fireballSpeed, ForceMode.VelocityChange);
					//General.changeSize (flamethrowerCost/Time.deltaTime);
					temp.strength = General.playerSize;
				}
				if(Input.GetKey (KeyCode.LeftShift)){
					General.screenShake.NewImpact ();
					General.changeSize (-Time.deltaTime, 100, 0);
					FireAttack temp = (FireAttack)Instantiate(fireball, .5f*(lHand.position+rHand.position), transform.rotation);
					Physics.IgnoreCollision (temp.collider, collider);
					Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
					AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
					foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
					//General.changeSize (rocketJumpCost/Time.deltaTime);
					temp.strength = General.playerSize;
					charMotor.inputJump = true;
					//gameObject.GetComponent<CharacterMotor>().inputJump = true;

				}
				//Debug.DrawRay(root.position, transform.forward);

			}
			//Debug.Log ((Vector3.Distance (lHandOld, Gestures.LArmDir())/Time.deltaTime).ToString ()+", "+(Vector3.Distance (rHandOld, Gestures.RArmDir())/Time.deltaTime).ToString ());
			rHandOld = Gestures.RArmDir();
			lHandOld = Gestures.LArmDir();
			//Debug.Log (flameWait.ToString ());
		}
	}
	void adjustFlames(float s){

		foreach (ParticleSystem part in flames){
			part.emissionRate = s*50;
			part.startSpeed = s*.1f;
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


	/*void OnGUI(){
		GUI.Box(new Rect(100, 100, Screen.width-200, 200), (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir())).ToString());
	}*/
}
