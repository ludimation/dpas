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
	bool lfiring = false;
	bool rfiring = false;
	Projectile lShot;
	Projectile rShot;

	public float lHandEnergy;
	public float rHandEnergy;
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
		Vector3 move;
		float yaw;
		if (General.kinectControl){
			move = controller.getDiff();
			move.Scale (new Vector3(1,0,1));
			yaw = controller.getYaw ();

			if(move.magnitude<deadzone){
				move = Vector3.zero;
			}
			//transform.Translate (temp);
			yaw = controller.getYaw();
			if (Mathf.Abs (yaw) < rotDeadzone){
				yaw = 0;
			}
			

		}
		else{
			move = new Vector3(Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			yaw = Input.GetAxis ("Mouse X");
		}
		//transform.Rotate (rotSpeed*new Vector3(0, yaw, 0));
		//transform.Translate (speed*Time.deltaTime*move);

		/*if(Mathf.Abs (Vector3.Distance (lHandOld, rootOld) - Vector3.Distance (lHand.position, root.position)) > tolerance*Time.deltaTime){
			//Debug.Log ((lHand.position-lHandOld).ToString());
			if(Vector3.Distance (lHandOld, rootOld) > Vector3.Distance (lHand.position, root.position)) {
				//Debug.Log ("lHandold = "lhandOld.ToString ()+" root = "+root.position.ToString ()
				//Debug.Log ("distance less");
				if (lfiring){
					lfiring = false;
					AudSrc.PlayOneShot (launch);
					//fireball (lHand.position-lShotOrigin, lHandEnergy);
					//lShot.velocity = 5*(lHandOld-lShotOrigin);
					//GameObject temp = (GameObject)Instantiate (lShot);
					
					Projectile temp = (Projectile)Instantiate (lShot);
					temp.velocity = 5*(lHandOld-lShotOrigin);
					temp.mortal = true;
					temp.lifespan = 3;

					//lShot.lifespan = 1+ lHandEnergy * 3;
					//lShot = ((GameObject)Instantiate (projectile, lHand.position, Quaternion.identity)).GetComponent<Projectile>();
					lHandEnergy = 0;
					
					
				}
				else{
					lShot.transform.position = lHand.position;
					lShot.lifespan = 1;
					lHandEnergy += Time.deltaTime;
					
				}
			}
			else{
				Debug.Log ("distance greater");
				if(!lfiring){
					lfiring = true;
					lShotOrigin = lHandOld;
				}
				//lShot.velocity += lHandOld-lHand.position;
				lShot.transform.position = lHand.transform.position;
				
			}
		}
		if(Mathf.Abs (Vector3.Distance (rHandOld, rootOld) - Vector3.Distance (rHand.position, root.position)) > tolerance*Time.deltaTime){
			//Debug.Log ((lHand.position-lHandOld).ToString());
			if(Vector3.Distance (rHandOld, rootOld) > Vector3.Distance (rHand.position, root.position)) {
				//Debug.Log ("lHandold = "lhandOld.ToString ()+" root = "+root.position.ToString ()
				//Debug.Log ("distance less");
				if (rfiring){
					rfiring = false;
					AudSrc.PlayOneShot (launch);
					//fireball (lHand.position-lShotOrigin, lHandEnergy);
					//GameObject temp = (GameObject)Instantiate (rShot);
					
					Projectile temp = (Projectile)Instantiate (rShot);
					temp.velocity = 5*(rHandOld-rShotOrigin);
					temp.mortal = true;
					temp.lifespan = 3;
					//rShot.velocity = 5*(rHandOld-rShotOrigin);
					//lShot.lifespan = 1+ lHandEnergy * 3;
					//rShot = ((GameObject)Instantiate (projectile, rHand.position, Quaternion.identity)).GetComponent<Projectile>();
					rHandEnergy = 0;
					
					
				}
				else{
					rShot.transform.position = rHand.position;
					
					rHandEnergy += Time.deltaTime;
					
				}
			}
			else{
				Debug.Log ("distance greater");
				if(!rfiring){
					rfiring = true;
					rShotOrigin = rHandOld;
				}
				//lShot.velocity += lHandOld-lHand.position;
				rShot.transform.position = rHand.transform.position;
				rShot.lifespan = 1;
				
			}
		}*/


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
