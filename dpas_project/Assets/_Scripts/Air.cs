using UnityEngine;
using System.Collections;

public class Air : MonoBehaviour {
	public Kinectalogue controller;
	public AudioSource audSrc;
	public AudioClip launch;
	public ParticleSystem part;
	public CharacterMotor charMotor;
	public basicControl baseControl;
	public Vector3 partRot = Vector3.forward;
	public bool ionized = false;
	public float ionizationHeight = 200;
	public float grav = 25f;

	public Vector3 cycloneSize = Vector3.one;
	public LightningStrike bolt;

	public float upDraft = 0f;

	public Transform lHand;
	public Transform rHand;
	public Transform lElbow;
	public Transform rElbow;
	public Transform lShoulder;
	public Transform rShoulder;

	public float lightningCharge = 0f;
	public float chargeTime = 5f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Angle(lHand.position-lElbow.position, Vector3.up)<45&&Vector3.Angle(rHand.position-rElbow.position, Vector3.up)<45){
			lightningCharge += Time.deltaTime / chargeTime;
			audSrc.PlayOneShot(launch);
		}

		if(!General.kinectControl){

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

	}

	public void Sleep(){
		//called before deactivating
		//charMotor.inputJump = false;
		//charMotor.movement.gravity = 10;
		charMotor.enabled = true;
		ionized = false;
		baseControl.fly = false;
	}
	public void UnSleep(){
		charMotor.enabled = false;
		baseControl.fly = true;


	}
	void OnTriggerStay(){
		//upDraft += 20*Time.deltaTime;
//		Debug.Log ("air elemental is colliding");
	}
	void OnGUI(){
		GUI.Box(new Rect (0, 0, lightningCharge*Screen.width, 150), "lightningCharge");

	}
}
