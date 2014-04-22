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
	void Start () {
		priority = Random.Range (float.MaxValue, float.MinValue);
	}
	
	// Update is called once per frame
	void Update () {
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
