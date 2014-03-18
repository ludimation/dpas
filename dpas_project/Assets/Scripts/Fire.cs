using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fire : MonoBehaviour {
	public Kinectalogue controller;
	public AudioSource AudSrc;
	public AudioClip launch;
	public GameObject projectile;
	public FireAttack fireBlast;
	//public float speed = 1;
	//public float rotSpeed = 45;
	//public float deadzone = .1f;
	//public float rotDeadzone = 2;
	public Transform lHand;
	public Transform rHand;
	public Transform root;
	Vector3 lHandOld;
	//Vector3 lShotOrigin;
	Vector3 rHandOld;
	//Vector3 rShotOrigin;
	Vector3 rootOld;
	public float tolerance = .02f;
	public float fireballSpeed = 1f;
	//public Projectile lShot;
	public Projectile fireball;
	//public ParticleSystem part;
	public List<ParticleSystem> flames;

	//public FireAttack fireBlast;

	//public float size;
	// Use this for initialization
	void Start () {
		lHandOld = lHand.position;
		rHandOld = rHand.position;
		rootOld = root.position;

	}
	
	// Update is called once per frame
	void Update () {
		if(!General.kinectControl){
			if (Input.GetKey(KeyCode.Space)){
				//General.playerSize += Time.deltaTime;
				General.increaseSize(Time.deltaTime, 15);

			}
			if(Input.GetKey (KeyCode.B)){
				General.increaseSize(Time.deltaTime, 15);
				//General.playerSize -= Time.deltaTime;
			}
			//General.playerSize = Mathf.Max (.1f, General.playerSize);
			//General.playerSize = Mathf.Min (10f, General.playerSize);
		}
		//part.emissionRate = General.playerSize*50;
		//part.startSpeed = General.playerSize*.1f;
		adjustFlames(General.playerSize);

		////require movement away from root
		/*if(Vector3.Distance (rHandOld, rootOld) < Vector3.Distance (rHand.position, root.position)){
			if(Vector3.Distance (rHandOld, rHand.position)> tolerance*Time.deltaTime){
				AudSrc.PlayOneShot (launch);
				Projectile temp = (Projectile)Instantiate (rShot);
				//temp.velocity = fireballSpeed*(rHand.position-rHandOld+(transform.rotation*move));
				temp.velocity = fireballSpeed*(rHand.position-root.position);
				temp.mortal = true;
				temp.lifespan = 3;
			}
		}
		if(Vector3.Distance (lHandOld, rootOld) < Vector3.Distance (lHand.position, root.position)){
			if(Vector3.Distance (lHandOld, lHand.position)> tolerance*Time.deltaTime){
				AudSrc.PlayOneShot (launch);
				Projectile temp = (Projectile)Instantiate (lShot);
				//temp.velocity = fireballSpeed*(lHand.position-lHandOld+(transform.rotation*move));
				temp.velocity = fireballSpeed*(lHand.position-root.position);
				temp.mortal = true;
				temp.lifespan = 3;
			}
		}*/

		//based on angle between hand velocity and root/hand position
		if(Vector3.Angle (lHand.position-lHandOld, lHandOld-rootOld)>60){
			if(Vector3.Distance (lHandOld, lHand.position)> tolerance*Time.deltaTime){
				AudSrc.PlayOneShot (launch);
				Projectile temp = (Projectile)Instantiate (fireball, lHand.position, Quaternion.identity);
				//temp.velocity = fireballSpeed*(lHand.position-lHandOld+(transform.rotation*move));
				temp.velocity = fireballSpeed*(lHand.position-root.position);
				temp.mortal = true;
				temp.lifespan = 3;
			}
		}
		if(Vector3.Angle (rHand.position-rHandOld, rHandOld-rootOld)>60){
			if(Vector3.Distance (rHandOld, rHand.position)> tolerance*Time.deltaTime){
				AudSrc.PlayOneShot (launch);
				Projectile temp = (Projectile)Instantiate (fireball, rHand.position, Quaternion.identity);
				//temp.velocity = fireballSpeed*(rHand.position-rHandOld+(transform.rotation*move));
				temp.velocity = fireballSpeed*(rHand.position-root.position);
				temp.mortal = true;
				temp.lifespan = 3;
			}
		}
		if(!General.kinectControl){
			if (Input.GetKeyDown (KeyCode.Q)){
				AudSrc.PlayOneShot (launch);
				Projectile temp = (Projectile)Instantiate (fireball, lHand.position, Quaternion.identity);
				//temp.velocity = fireballSpeed*(lHand.position-lHandOld+(transform.rotation*move));
				temp.velocity = fireballSpeed*(lHand.position-root.position);
				temp.mortal = true;
				temp.lifespan = 3;
			}
			if(Input.GetKeyDown (KeyCode.E)){
				AudSrc.PlayOneShot (launch);
				Projectile temp = (Projectile)Instantiate (fireball, rHand.position, Quaternion.identity);
				//temp.velocity = fireballSpeed*(rHand.position-rHandOld+(transform.rotation*move));
				temp.velocity = fireballSpeed*(rHand.position-root.position);
				temp.mortal = true;
				temp.lifespan = 3;
			}
			if (Input.GetKeyDown (KeyCode.X)){
				FireAttack temp = (FireAttack)Instantiate(fireBlast, root.position+transform.forward, transform.rotation);
				Rigidbody foo = temp.gameObject.GetComponent<Rigidbody>();
				foo.AddForce (transform.forward*fireballSpeed, ForceMode.VelocityChange);
				temp.strength = General.playerSize;
			}
			//Debug.DrawRay(root.position, transform.forward);

		}
		rHandOld = rHand.position;
		lHandOld = lHand.position;
		rootOld = root.position;
		//lShot.transform.position = lHand.position;
		//rShot.transform.position = rHand.position;

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

	/*void activate(){
		part.gameObject.SetActive(true);
		lShot.gameObject.SetActive (true);
		rShot.gameObject.SetActive (true);
	}*/
	//void OnGUI(){

	//}
}
