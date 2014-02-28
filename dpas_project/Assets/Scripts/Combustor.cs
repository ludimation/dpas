using UnityEngine;
using System.Collections;

public class Combustor : MonoBehaviour {
	public Kinectalogue controller;
	public AudioSource AudSrc;
	public AudioClip launch;
	public GameObject projectile;
	public float speed = 1;
	public float rotSpeed = 45;
	public float deadzone = .1f;
	public float rotDeadzone = 2;
	public Transform lHand;
	public Transform rHand;
	public Transform root;
	Vector3 lHandOld;
	Vector3 lShotOrigin;
	Vector3 rHandOld;
	Vector3 rShotOrigin;
	Vector3 rootOld;
	public float tolerance = .02f;
	public float fireballSpeed = 1f;
	Projectile lShot;
	Projectile rShot;
	public ParticleSystem part;
	public float size;
	// Use this for initialization
	void Start () {
		lHandOld = lHand.position;
		rHandOld = rHand.position;
		rootOld = root.position;
		lShot = ((GameObject)Instantiate (projectile, lHand.position, Quaternion.identity)).GetComponent<Projectile>();
		//lShot.renderer.enabled = false;
		rShot = ((GameObject)Instantiate (projectile, rHand.position, Quaternion.identity)).GetComponent<Projectile>();

	
	}
	
	// Update is called once per frame
	void Update () {
		if(!General.kinectControl){
			if (Input.GetKey(KeyCode.Space)){
				size += Time.deltaTime;
			}
			if(Input.GetKey (KeyCode.B)){
				size -= Time.deltaTime;
			}
			size = Mathf.Max (.1f, size);
		}
		part.emissionRate = size*50;
		part.startSpeed = size*.1f;


		if(Vector3.Distance (rHandOld, rootOld) < Vector3.Distance (rHand.position, root.position)){
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
		}
		if(!General.kinectControl){
			if (Input.GetKeyDown (KeyCode.Q)){
				AudSrc.PlayOneShot (launch);
				Projectile temp = (Projectile)Instantiate (lShot);
				//temp.velocity = fireballSpeed*(lHand.position-lHandOld+(transform.rotation*move));
				temp.velocity = fireballSpeed*(lHand.position-root.position);
				temp.mortal = true;
				temp.lifespan = 3;
			}
			if(Input.GetKeyDown (KeyCode.E)){
				AudSrc.PlayOneShot (launch);
				Projectile temp = (Projectile)Instantiate (rShot);
				//temp.velocity = fireballSpeed*(rHand.position-rHandOld+(transform.rotation*move));
				temp.velocity = fireballSpeed*(rHand.position-root.position);
				temp.mortal = true;
				temp.lifespan = 3;
			}

		}
		rHandOld = rHand.position;
		lHandOld = lHand.position;
		rootOld = root.position;
		lShot.transform.position = lHand.position;
		rShot.transform.position = rHand.position;

	}



	void OnGUI(){

	}
}
