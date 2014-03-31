using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

	public Transform lHand;
	public Transform rHand;
	public float minThrowSpeed = 2.5f;
	public bool isAwake = false;
	public WaterAttack wave;
	public float waveCost;
	public WaterAttack geyser;
	public float geyserCost;
	public WaterAttack waterJet;
	public float waterJetCost;

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
		//Debug.DrawRay (transform.position, Gestures.flatShoulderRot()*Vector3.forward);
		if(Gestures.ArmsTogether ()){
			//WaterAttack temp = (WaterAttack)Instantiate(WaterBlast, transform.position+(2*transform.forward)+Vector3.Up, transform.rotation);
			WaterAttack temp = (WaterAttack)Instantiate(wave, .5f*(lHand.position+rHand.position), transform.rotation);
			//Physics.IgnoreCollision (temp.collider, collider);
			Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
			//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
			foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
			//General.changeSize (flamethrowerCost/Time.deltaTime);
			//temp.strength = General.playerSize;
			//flameWait = flameDelay;
			
		}
		if (Vector3.Angle (Gestures.LArmDir(), new Vector3(-1,-1,-1))<45 && Vector3.Angle (Gestures.RArmDir(), new Vector3(1,-1,-1))<45){
			WaterAttack temp = (WaterAttack)Instantiate(wave, .5f*(lHand.position+rHand.position), transform.rotation);
			//Physics.IgnoreCollision (temp.collider, collider);
			Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
			//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
			foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
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
				//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				WaterAttack temp = (WaterAttack)Instantiate(wave, lHand.position, Quaternion.identity);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				//foo.AddForce (waveSpeed*(transform.rotation*(Gestures.LArmDir()-lHandOld)), ForceMode.VelocityChange);
				foo.AddForce (waveSpeed*(transform.rotation*Gestures.LArmDir()), ForceMode.VelocityChange);
				//foo.AddForce (waveSpeed*Gestures.LArmDir(), ForceMode.VelocityChange);
				//General.changeSize (waveCost/Time.deltaTime);
				//temp.strength = General.playerSize;
				
				
			}
		}
		if(Vector3.Angle (rHandOld - Gestures.RArmDir(), rHandOld)>60){
			//Debug.Log ("R wave");
			if(Vector3.Distance (rHandOld, Gestures.RArmDir ())> minThrowSpeed*Time.deltaTime){
				//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				WaterAttack temp = (WaterAttack)Instantiate(wave, rHand.position, Quaternion.identity);
				//WaterAttack temp = (WaterAttack)Instantiate(wave, transform.position+(transform.rotation*(RArmDir()-rHandOld)), Quaternion.identity);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				//foo.AddForce (waveSpeed*(transform.rotation*(Gestures.RArmDir()-rHandOld)), ForceMode.VelocityChange);
				foo.AddForce (waveSpeed*(transform.rotation*Gestures.RArmDir()), ForceMode.VelocityChange);
				//General.changeSize (waveCost/Time.deltaTime);
				//temp.strength = General.playerSize;
				
			}
		}
		lHandOld = Gestures.LArmDir();
		rHandOld = Gestures.RArmDir();
		
		if(!General.kinectControl){
			if(Input.GetKeyDown (KeyCode.E)){
				WaterAttack temp = (WaterAttack)Instantiate(wave, rHand.position, Quaternion.identity);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				foo.AddForce (waveSpeed*transform.forward, ForceMode.VelocityChange);
				
			}
			if(Input.GetKeyDown (KeyCode.Q)){
				WaterAttack temp = (WaterAttack)Instantiate(wave, lHand.position, Quaternion.identity);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				foo.AddForce (waveSpeed*transform.forward, ForceMode.VelocityChange);
				
			}
			if(Input.GetKey(KeyCode.LeftShift)){
				WaterAttack temp = (WaterAttack)Instantiate(geyser, .5f*(lHand.position+rHand.position), transform.rotation);
				//Physics.IgnoreCollision (temp.collider, collider);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
				//General.changeSize (rocketJumpCost/Time.deltaTime);
				//temp.strength = General.playerSize;
				charMotor.inputJump = true;
				//gameObject.GetComponent<CharacterMotor>().inputJump = true;
			}

			if(Input.GetKey (KeyCode.X)){
				WaterAttack temp = (WaterAttack)Instantiate(waterJet, .5f*(lHand.position+rHand.position), transform.rotation);
				//Physics.IgnoreCollision (temp.collider, collider);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				//AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
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

}
