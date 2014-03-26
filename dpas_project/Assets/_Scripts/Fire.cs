using UnityEngine;
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

	public FireAttack flamethrower;
	public FireAttack fireball;
	public FireAttack rocketJump;

	Vector3 lHandOld;
	Vector3 rHandOld;
	public float tolerance = .02f;
	public float fireballSpeed = 1f;
	public float flameDelay = .2f;
	float flameWait;

	public List<ParticleSystem> flames;

	//public FireAttack fireBlast;

	//public float size;
	// Use this for initialization
	void Start () {
		flameWait = flameDelay;
		lHandOld = Gestures.LArmDir();
		rHandOld = Gestures.RArmDir();
		//rootOld = root.position;

	}
	
	// Update is called once per frame
	void Update () {
		flameWait -= Time.deltaTime;
		if(!General.kinectControl){
			if (Input.GetKey(KeyCode.Space)){
				//General.playerSize += Time.deltaTime;
				General.increaseSize(Time.deltaTime, 15);

			}
			if(Input.GetKey (KeyCode.B)){
				General.increaseSize(-Time.deltaTime, 15);
				//General.playerSize -= Time.deltaTime;
			}
		}
		adjustFlames(General.playerSize);
		
		//if(Vector3.Angle (lHandOld, Gestures.LArmDir())>60){
		if(Vector3.Angle (lHandOld - Gestures.LArmDir(), lHandOld)>60){
			//Debug.Log ("L fireball");
			if(Vector3.Distance (lHandOld, Gestures.LArmDir ())> tolerance*Time.deltaTime){
				AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				FireAttack temp = (FireAttack)Instantiate(fireball, lHand.position, Quaternion.identity);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				foo.AddForce (fireballSpeed*(transform.rotation*(Gestures.LArmDir()-lHandOld)), ForceMode.VelocityChange);
				//foo.AddForce (fireballSpeed*Gestures.LArmDir(), ForceMode.VelocityChange);
				temp.strength = General.playerSize;

			}
		}
		if(Vector3.Angle (rHandOld - Gestures.RArmDir(), rHandOld)>60){
			//Debug.Log ("R fireball");
			if(Vector3.Distance (rHandOld, Gestures.RArmDir ())> tolerance*Time.deltaTime){
				AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				FireAttack temp = (FireAttack)Instantiate(fireball, rHand.position, Quaternion.identity);
				//FireAttack temp = (FireAttack)Instantiate(fireball, transform.position+(transform.rotation*(RArmDir()-rHandOld)), Quaternion.identity);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				foo.AddForce (fireballSpeed*(transform.rotation*(Gestures.RArmDir()-rHandOld)), ForceMode.VelocityChange);
				temp.strength = General.playerSize;
				
			}
		}
		if(flameWait<0f&&Gestures.ArmsTogether ()){
			//FireAttack temp = (FireAttack)Instantiate(fireBlast, transform.position+(2*transform.forward)+Vector3.Up, transform.rotation);
			FireAttack temp = (FireAttack)Instantiate(fireball, .5f*(lHand.position+rHand.position), transform.rotation);
			Physics.IgnoreCollision (temp.collider, collider);
			Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
			AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
			foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
			temp.strength = General.playerSize;
			flameWait = flameDelay;
			
		}

		//if(Gestures.LArmStraight()&&Gestures.RArmStraight ()&&
		if (Vector3.Angle (Gestures.LArmDir(), new Vector3(-1,-1,-1))<45 && Vector3.Angle (Gestures.RArmDir(), new Vector3(1,-1,-1))<45){
			FireAttack temp = (FireAttack)Instantiate(fireball, .5f*(lHand.position+rHand.position), transform.rotation);
			Physics.IgnoreCollision (temp.collider, collider);
			Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
			AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
			foo.AddForce (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir()), ForceMode.VelocityChange);
			temp.strength = General.playerSize;
			gameObject.GetComponent<CharacterMotor>().inputJump = true;
		}
		else{			
			gameObject.GetComponent<CharacterMotor>().inputJump = false;
		}
		if(!General.kinectControl){
			if (Input.GetKeyDown (KeyCode.Q)){
				AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				FireAttack temp = (FireAttack)Instantiate (fireball, (transform.rotation*lHand.position), Quaternion.identity);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				foo.AddForce (transform.forward*fireballSpeed, ForceMode.VelocityChange);
				temp.strength = General.playerSize;
			}
			if(Input.GetKeyDown (KeyCode.E)){
				AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
				FireAttack temp = (FireAttack)Instantiate (fireball, (transform.rotation *rHand.position), Quaternion.identity);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				foo.AddForce (transform.forward*fireballSpeed, ForceMode.VelocityChange);
				temp.strength = General.playerSize;
			}
			if (Input.GetKeyDown (KeyCode.X)){
				AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);

				FireAttack temp = (FireAttack)Instantiate(fireBlast, transform.position+(2*transform.forward)+Vector3.up, transform.rotation);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				foo.AddForce (transform.forward*fireballSpeed, ForceMode.VelocityChange);
				temp.strength = General.playerSize;
			}
			//Debug.DrawRay(root.position, transform.forward);

		}
		rHandOld = Gestures.RArmDir();
		lHandOld = Gestures.LArmDir();

	}
	void adjustFlames(float s){

		foreach (ParticleSystem part in flames){
			part.emissionRate = s*50;
			part.startSpeed = s*.1f;
		}
	}
	public void Sleep(){
		//called before deactivating script

	}
	public void UnSleep(){
		//called when activating the script
	}


	/*void OnGUI(){
		GUI.Box(new Rect(100, 100, Screen.width-200, 200), (transform.rotation*(Gestures.LArmDir()+Gestures.RArmDir())).ToString());
	}*/
}
