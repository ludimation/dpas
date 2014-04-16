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
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (obstacle&&size <= 0){
			Destroy(gameObject);
		}
		if (!General.isPaused && rainAmnt >0){
			if(rainT<0){
				Instantiate(raindrop, transform.position + size*2*(Random.rotation*Vector3.up), Quaternion.identity);
				size -= Time.deltaTime;
				rainAmnt -= Time.deltaTime;
				rainT = rainWait;
			}
		}
		rainT -= Time.deltaTime;
	
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
		}
	}
}
