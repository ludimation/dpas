using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {
	public WaterAttack raindrop;
	// Use this for initialization
	public float size = 1;
	public float rainWait = .3f;
	float rainT = .3f;
	public bool obstacle = false;
	float rainAmnt;
	public float priority;
	public float floatHeight = 15;
	public float floatSpeed = 2.5f;
	Ray r;
	public static LayerMask mask;
	public static bool mSet = false;
	void Start () {
		r = new Ray(transform.position, Vector3.down);
		if(!mSet){
			mSet = true;
			mask = new LayerMask();
			mask.value = 1 << 11;
		}
		if(obstacle){
			priority = float.MaxValue;
		}
		else{
			priority = Random.Range (float.MaxValue, float.MinValue);
		}
	}
	
	// Update is called once per frame
	void Update () {
		r.origin = transform.position;
		Debug.DrawRay(transform.position, floatHeight * Vector3.down);
		if(Physics.Raycast(r, floatHeight, mask)){
			transform.Translate (Time.deltaTime*floatSpeed*Vector3.up);
		}
		//Debug.Log (LayerMask.LayerToName (11));
		if(!General.isPaused){
			if (!obstacle&&size <= 0){
				Destroy(gameObject);
			}
			if(obstacle&&rainAmnt > 0){
				if(General.pullEnergy(raindrop.size)>0){
					if(rainT<0){
						Instantiate(raindrop, transform.position + size*2*(Random.rotation*Vector3.up), Quaternion.identity);
						//size -= raindrop.size;
						rainAmnt -= raindrop.size;
						rainT = rainWait;
					}
				}
			}
			else if (rainAmnt >0){
				if(rainT<0){
					Instantiate(raindrop, transform.position + size*2*(Random.rotation*Vector3.up), Quaternion.identity);
					size -= raindrop.size;
					rainAmnt -= raindrop.size;
					rainT = rainWait;
				}
			}
			rainT -= Time.deltaTime;
		}
		Debug.Log ("t = "+rainT.ToString()+", amnt = " +rainAmnt.ToString ());
	}
	public void Rain (float amnt){
		Debug.Log ("raining: "+amnt.ToString ());
		rainAmnt += amnt;
	}

	void OnTriggerEnter (Collider col){
		Debug.Log ("trigger entered");
		AirAttack aA = col.gameObject.GetComponent<AirAttack>();
		//Debug.Log (aA.ToString ());
		if(aA){
			Debug.Log ("air attack detected");
			rigidbody.AddForce (col.rigidbody.velocity, ForceMode.Impulse);
			return;
		}
		Cloud c = col.gameObject.GetComponent<Cloud>();
		if(c && priority > c.priority){
			size += c.size;
			Destroy(c.gameObject);
		}
	}
}
