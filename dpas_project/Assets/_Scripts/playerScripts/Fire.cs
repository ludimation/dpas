using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {
	//public Kinectalogue controller;
	public AudioSource AudSrc;
	//public AudioClip launch;
	public bool enableFireball = true;
	public FireAttack fireball;
	public List<AudioClip> launchSounds;
	public float fireballCost = 1;
	public float fireballWait = 0;
	float fireballT;
	public float fireballSpeed = 1;
	//public GameObject projectile;
	bool lPrimed = false;
	bool rPrimed = false;

	//public bool rocketJump;
	//public FireAttack fireBlast;
	public Transform lHand;
	public Transform rHand;
	//public List<GameObject> effects;

	public bool enableFlamethrower = true;
	public FireAttack flamethrower;
	public float flamethrowerCost = 1f;
	public float flamethrowerWait = 0;
	float flamethrowerT = 0f;
	public List<AudioClip> flamethrowerSounds;
	public float flamethrowerSpeed = 1f;
	//public float fireballCost = 1f;

	public bool enableRocketJump = true;
	public FireAttack rocketJump;
	public GameObject explosionPrefab;
	public float rocketJumpCost = 1f;
	public float rocketJumpWait = 0;
	float rocketJumpT = 0;
	public List<AudioClip> rocketJumpSounds;
	public float rocketJumpSpeed = 1;

	Vector3 lHandOld;
	Vector3 rHandOld;
	public float tolerance = .02f;
	//public float fireballSpeed = 1f;
	//public float flameDelay = .2f;
	//float flameWait;

	//public bool isAwake = false;

	public List<ParticleSystem> flames;
	public CharacterMotor charMotor;
	public CharacterController charCol;
	public Texture2D leftThrowIcon;
	public Texture2D rightThrowIcon;
	public Texture2D leftReadyIcon;
	public Texture2D rightReadyIcon;
	public Texture2D leftPrepIcon;
	public Texture2D rightPrepIcon;
	public Texture2D rocketIcon;
	public Texture2D rocketActivatedIcon;
	public Texture2D flamethrowerIcon;
	public Texture2D flamethrowerActivatedIcon;
	public float iconSize = 64;
	//public FireAttack fireBlast;

	float oldY = 0;

	//public float size;
	// Use this for initialization
	void Start () {
		//flameWait = flameDelay;
		fireballT = fireballWait;
		rocketJumpT = rocketJumpWait;
		flamethrowerT = flamethrowerWait;
		lHandOld = Gestures.LArmDir();
		rHandOld = Gestures.RArmDir();
		if (charMotor == null){
			charMotor = gameObject.GetComponent<CharacterMotor>();
		}
		if (!charCol){
			charCol = gameObject.GetComponent<CharacterController>();
		}
		//rootOld = root.position;

	}
	
	// Update is called once per frame
	void Update () {
		if(!General.isPaused){
			//transform.localScale = (1+(.1f*General.playerSize))*Vector3.one;

			if (General.playerSize <.01f){
				General.g.changeElement (General.Element.Air);
			}
			//flameWait -= Time.deltaTime;
			fireballT -= Time.deltaTime;
			flamethrowerT -= Time.deltaTime;
			rocketJumpT -= Time.deltaTime;

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
			if (Gestures.LArmBent ()){
				//if(Input.GetKeyDown (KeyCode.Mouse1)){
				lHand.localScale = 2*Vector3.one;
				lPrimed = true;
			}
			if (lPrimed && Gestures.LArmStraight ()){
				ThrowFireball(lHand.position, transform.rotation*Gestures.LArmDir ());
				lPrimed = false;
				//lHand.localScale = Vector3.one;
			}
			if (Gestures.RArmBent ()){
				rPrimed = true;
				//rHand.localScale = 2*Vector3.one;
			}
			if (rPrimed && Gestures.RArmStraight ()){
				ThrowFireball(rHand.position, transform.rotation*Gestures.RArmDir ());
				rPrimed = false;
				rHand.localScale = Vector3.one;
			}

			if(Gestures.ArmsTogether () && !Gestures.ArmsDown()){

				//ShootFlamethrower(transform.position+(2*transform.forward)+Vector3.up, transform.forward);
				ShootFlamethrower(.5f*(lHand.position + rHand.position), transform.rotation*Gestures.CommonDir ());

				
			}

			//if(Gestures.LArmStraight()&&Gestures.RArmStraight ()&&
			if (Vector3.Angle (Gestures.flatShoulderRot ()*Gestures.LArmDir(), new Vector3(-1,-1,-1))<45 && Vector3.Angle (Gestures.flatShoulderRot ()*Gestures.RArmDir(), new Vector3(1,-1,-1))<45){
				//if(flameWait<0){
				FireRocket(.5f*(lHand.position+rHand.position), transform.rotation*Gestures.CommonDir ());
				//}
				//if(charMotor.grounded){
				if(charCol.isGrounded){
					Instantiate(explosionPrefab, transform.position, transform.rotation);

				}
				charMotor.inputJump = true;

			}
			else{			
				charMotor.inputJump = false;

			}
			if(!General.kinectControl){
				//Debug.Log (Time.deltaTime.ToString ());
				if (Input.GetKeyDown (KeyCode.Q)){
					ThrowFireball(lHand.position, transform.forward);
				}
				if(Input.GetKeyDown (KeyCode.E)){
					ThrowFireball(rHand.position, transform.forward);
				}
				if (Input.GetKey (KeyCode.X)){
					ShootFlamethrower(.5f*(rHand.position+lHand.position), transform.forward);
				}
				if(Input.GetKey (KeyCode.LeftShift)){
					FireRocket(.5f*(lHand.position+rHand.position), transform.rotation*Gestures.CommonDir ());

					//if(charMotor.grounded){
					if(charCol.isGrounded){
						Instantiate(explosionPrefab, transform.position, transform.rotation);
					}
					charMotor.inputJump = true;


				}

			}
			//Debug.Log ((Vector3.Distance (lHandOld, Gestures.LArmDir())/Time.deltaTime).ToString ()+", "+(Vector3.Distance (rHandOld, Gestures.RArmDir())/Time.deltaTime).ToString ());
			rHandOld = Gestures.RArmDir();
			lHandOld = Gestures.LArmDir();
			//Debug.Log (flameWait.ToString ());
		}
	}
	void ThrowFireball(Vector3 pos, Vector3 dir){
		if(enableFireball&&fireballT<0){
			fireballT = fireballWait;
			General.screenShake.NewImpact ();
			AudSrc.PlayOneShot (launchSounds[Random.Range(0,launchSounds.Count)]);
			FireAttack temp = (FireAttack)Instantiate (fireball, (pos), Quaternion.identity);
			temp.rigidbody.AddForce (dir*fireballSpeed, ForceMode.VelocityChange);
			temp.strength = General.playerSize;
			General.changeSize (fireballCost, 100, 0);
		}
	}
	void ShootFlamethrower(Vector3 pos, Vector3 dir){
		if(enableFlamethrower && flamethrowerT < 0){
			flamethrowerT = flamethrowerWait;
			General.screenShake.NewImpact ();
			General.changeSize (-Time.deltaTime*flamethrowerCost, 100, 0);
			AudSrc.PlayOneShot (flamethrowerSounds[Random.Range(0,flamethrowerSounds.Count)]);
			FireAttack temp = (FireAttack)Instantiate(flamethrower, pos, transform.rotation);
			temp.rigidbody.AddForce (dir*flamethrowerSpeed, ForceMode.VelocityChange);
			temp.strength = General.playerSize;
		}
	}
	void FireRocket (Vector3 pos, Vector3 dir){
		if(enableRocketJump && rocketJumpT<0){
			rocketJumpT = rocketJumpWait;
			//General.screenShake.NewImpact ();
			General.changeSize (-Time.deltaTime * rocketJumpCost, 100, 0);
			FireAttack temp = (FireAttack)Instantiate(fireball, .5f*(lHand.position+rHand.position), transform.rotation);
			AudSrc.PlayOneShot (rocketJumpSounds[Random.Range(0,rocketJumpSounds.Count)]);
			temp.rigidbody.AddForce (rocketJumpSpeed*(transform.rotation*Gestures.CommonDir()), ForceMode.VelocityChange);
			temp.strength = General.playerSize;
		}
	}
	void adjustFlames(float s){

		foreach (ParticleSystem part in flames){
			part.emissionRate = s*50;
			part.startSpeed = s*.1f;
		}
	}
	public void Sleep(){
		lPrimed = false;
		rPrimed = false;
		//called before deactivating script
		//isAwake = false;

	}
	public void UnSleep(){
		//called when activating the script
		//isAwake = true;
	}
	//void OnLevelWasLoaded(){


	void OnGUI(){
		if(General.icons){
			if(!lPrimed){
				GUI.DrawTexture (new Rect(0,0, iconSize, iconSize), leftPrepIcon);
			}
			else{
				GUI.DrawTexture (new Rect (0,0, iconSize, iconSize), leftReadyIcon);
				GUI.DrawTexture (new Rect (0, iconSize, iconSize, iconSize), leftThrowIcon);
			}
			if(!rPrimed){
				GUI.DrawTexture (new Rect (iconSize, 0, iconSize, iconSize), rightPrepIcon);
			}
			else{
				GUI.DrawTexture (new Rect (iconSize,0, iconSize, iconSize), rightReadyIcon);
				GUI.DrawTexture (new Rect (iconSize, iconSize, iconSize, iconSize), rightThrowIcon);
			}
			if(flamethrowerT < 0){
				GUI.DrawTexture(new Rect(iconSize*2, 0, iconSize, iconSize), flamethrowerIcon);
			}
			else{
				GUI.DrawTexture(new Rect(iconSize*2, 0, iconSize, iconSize), flamethrowerActivatedIcon);
			}
			
			if(rocketJumpT < 0){
				GUI.DrawTexture(new Rect(iconSize*3, 0, iconSize, iconSize), rocketIcon);
			}
			else{
				GUI.DrawTexture(new Rect(iconSize*3, 0, iconSize, iconSize), rocketActivatedIcon);
			}
		}
	}
	/*Void OnTriggerEnter(Collider other){
		RockThing r = other.GetComponent<RockThing>();
		if(r){
			General.g.changeElement(General.Element.Air);
			//General.playerSize = 1
		}
	}*/
}
