using UnityEngine;
using System.Collections;

public class Shrubbery : MonoBehaviour {

	public float fuel = 1;
	public float fuelLimit = 1;
	public float resistance = 1;
	public float burnRate = .1f;
	public bool burning = false;
	public ParticleSystem flame;

	// Use this for initialization
	void Start () {
		flame.enableEmission = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (burning){
			fuel -= burnRate * Time.deltaTime;
			//flame.transform.localScale *= 1+Time.deltaTime;
			//flame.emissionRate *= 1+Time.deltaTime;
		}

		if (fuel < 0){
			GameObject.Destroy (gameObject);
		}

	
	}

	/*void OnCollisionEnter(Collision col){
		Debug.Log ("burn baby burn");
	}*/
	public void Ignite(){
		burning = true;
		flame.enableEmission = true;
	}

	void OnTriggerEnter (Collider other){
		//Debug.Log ("foo");
		Fire temp = other.GetComponent<Fire>();
		if (burning && temp != null){
			General.increaseSize(fuel, fuelLimit);
			//General.playerSize += fuel;
			fuel = 0;
			Debug.Log("trigger entered");
			//burning = true;
			//flame.enableEmission = true;
		}
		else {
			FireAttack source = other.GetComponent<FireAttack>();
			if (source != null){
				if (source.strength > resistance){
					Debug.Log("trigger entered");
					Ignite();
					//burning = true;
					//flame.enableEmission = true;
				}
			}
		}
	}
}
