using UnityEngine;
using System.Collections;

public class Air : MonoBehaviour {
	public Kinectalogue controller;
	public AudioSource AudSrc;
	public AudioClip launch;
	public ParticleSystem part;
	public CharacterMotor charMotor;
	public Vector3 partRot = Vector3.forward;
	public bool ionized = false;
	public float ionizationHeight = 200;
	public float grav = 25f;
	//public float size = 1;
	public Vector3 cycloneSize = Vector3.one;
	public LightningStrike bolt;
	
	public Transform lHand;
	public Transform rHand;
	public Transform lElbow;
	public Transform rElbow;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(!General.kinectControl){
			/*if(charMotor.grounded){
				if(ionized){
					LightningStrike temp = (LightningStrike)Instantiate(bolt, transform.position+(25*Vector3.up), Quaternion.identity);
					temp.source = this;
					temp.strength = General.playerSize;

				}
				else{
					charMotor.inputJump = true;
				}
			}
			else if (General.keyControlOnly){
				charMotor.inputJump = false;
				charMotor.movement.gravity = Input.GetAxis ("Flight") * 50;
			}*/
			if (Input.GetKey(KeyCode.Space)){
				General.increaseSize(Time.deltaTime, 15);
				//General.playerSize += Time.deltaTime;
			}
			if(Input.GetKey (KeyCode.B)){
				General.increaseSize(-Time.deltaTime, 15);
				//General.playerSize -= Time.deltaTime;
			}
			//General.playerSize = Mathf.Max (.1f, General.playerSize);
			if (Input.GetKeyDown (KeyCode.X)){
				LightningStrike temp = (LightningStrike)Instantiate(bolt, transform.position+(25*Vector3.up), Quaternion.identity);
				temp.source = this;
				temp.strength = General.playerSize;
			}
			if (Input.GetKeyDown(KeyCode.Q)){
				ionized = true;
			}
			//Debug.Log ("max = "+Mathf.Max (.1f, General.playerSize).ToString());
		}
		if (transform.position.y > ionizationHeight){
			ionized = true;
		}
		//part.transform.localScale = (1/General.playerSize)*cycloneSize;
		if(charMotor.grounded){
			if(ionized){
				LightningStrike temp = (LightningStrike)Instantiate(bolt, transform.position+(25*Vector3.up), Quaternion.identity);
				temp.source = this;
				temp.strength = General.playerSize;
				ionized = false;
				charMotor.inputJump = false;
				
			}
			else{
				//charMotor.inputJump = true;
				charMotor.inputJump = !charMotor.inputJump;
			}
		}
		else{
			charMotor.inputJump = false;
			if(General.keyControlOnly){
				charMotor.movement.gravity = (Input.GetAxis ("Flight") * -20) +5;
			}
			else if(transform.position.y > ionizationHeight){
				charMotor.movement.gravity = 10;
			}
			else{
				float a = Vector3.Angle (lHand.position-lElbow.position, Vector3.up);
				float b = Vector3.Angle (rHand.position-rElbow.position, Vector3.up);
				/*if(true){
					Debug.Log ("hope this works");
				}*/
				if(a < 60 && b <60){
					//charMotor.movement.gravity = -(grav/60)*(60- Mathf.Min(a, b));
					charMotor.movement.gravity = - grav;
				}
				else if (a>120 && b>120){
					//charMotor.movement.gravity = (grav/60)*(Mathf.Max (a, b) - 180);
					charMotor.movement.gravity = grav;
				}
				else {
					charMotor.movement.gravity = grav*.3f;
				}
			}
				//charMotor.movement.gravity = Input.GetAxis ("Flight") * 50;
		}
		//part.startLifetime = General.playerSize*5;
		//partRot = Vector3.forward * General.playerSize * 50;
		//part.transform.Rotate(Time.deltaTime * partRot * General.playerSize);

	}

	public void Sleep(){
		//called before deactivating
		charMotor.inputJump = false;
		charMotor.movement.gravity = 10;
		ionized = false;
	}
	public void UnSleep(){

	}
}
