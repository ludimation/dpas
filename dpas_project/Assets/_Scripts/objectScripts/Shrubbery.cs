using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shrubbery : MonoBehaviour {

	public float fuel = 1;
	public float fuelLimit = 1;
	public float resistance = 1;
	public float burnRate = .1f;
	public bool burning = false;
	public bool dead = false;
	public List<GameObject> liveModels;
	public List<GameObject> deadModels;
	public List<GameObject> burnModels;
	/*public GameObject smallDeadModel;
	public GameObject smallBurnModel;
	public GameObject smallModel;
	public GameObject bigDeadModel;
	public GameObject bigBurnModel;
	public GameObject bigModel;*/
	GameObject currentModel;
	public float waterAmount =1;
	public float waterThreshold = 3;
	public float dryRate = .01f;
	public ParticleSystem flame;
	public float timeUpperBound = 2;
	public float timeLowerBound = 2;
	float time;
	public int currentSize = 0;


	// Use this for initialization
	void Start () {
		if (burnModels.Count != deadModels.Count || deadModels.Count != liveModels.Count){
			Debug.LogWarning ("improper number of tree models");
		}
		time = Random.Range (timeLowerBound, timeUpperBound);
		StuffHappen();
		//flame.enableEmission = false;
		//new Vector3 (
	
	}
	
	// Update is called once per frame
	void Update () {
		/*if (waterAmount<=0){
			dead = true;
		}*/
		//if(currentSize > 0){
		//	time -= Time.deltaTime;
		//}
		time -= Time.deltaTime;
		if(time<0){
			StuffHappen();
		}
		if(fuel<0){
			ParticleSystem[] ps = currentModel.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem p in ps){
				p.enableEmission = false;
			}
			fuel = 0;
		}
		else if (burning){
			fuel -= burnRate * Time.deltaTime;
			General.availableEnergy += Time.deltaTime;
		}
		if(!dead&&currentSize>0){
			waterAmount -= dryRate * Time.deltaTime;
		}
		//if(fuel
		/*if (fuel <= 0){
			GameObject.Destroy (gameObject);
			smallModel.SetActive (false);
			deadModel.SetActive (false);
			burnModel.SetActive (false);
			bigModel.SetActive (false);
		}*/
		//Debug.Log ("dead = "+dead.ToString ()+", burning = "+burning.ToString ()+", water = "+waterAmount.ToString ()+", fuel = "+fuel.ToString());
	
	}
	void StuffHappen(){
		time = Random.Range (timeLowerBound, timeUpperBound);
		if(!burning&&!dead&&waterAmount<=0){
			Debug.Log ("out of water, death is upon us");
			dead = true;
			Destroy(currentModel);
			currentModel = (GameObject)Instantiate (deadModels[currentSize], transform.position, transform.rotation);
			currentModel.transform.parent = transform;

		}
		else if (!burning&&waterAmount > waterThreshold * currentSize){
			++currentSize;
			if(currentSize<liveModels.Count){
				Debug.Log ("threshold "+currentSize.ToString ()+" exceeded, growing");
				Destroy(currentModel);
				currentModel = (GameObject)Instantiate (liveModels[currentSize], transform.position, transform.rotation);
				currentModel.transform.parent = transform;
			}
			else{
				Debug.Log ("threshold "+currentSize.ToString ()+" exceeded, not growing");
				--currentSize;
			}
			General.availableEnergy -=fuel;
			fuel = currentSize *.5f;
			
			General.availableEnergy +=fuel;
			resistance = currentSize;
			fuelLimit = currentSize *.5f;
			//currentSize = Mathf.Min (liveModels.Count-1, currentSize);
			dead = false;
		}
		/*else if(waterAmount <= 0 && !dead && !burning){
			dead = true;
			Destroy(currentModel);
			currentModel =  GameObject.Instantiate (livModels[currentSize], transform.position, transform.rotation);

		}*/
	}
	public void AddWater(float w){
		waterAmount += w;
		//General.availableEnergy += waterAmount;
		if(fuel<waterAmount){
			//General.availableEnergy -= waterAmount-fuel;
			//fuel = waterAmount;
		}
		if(burning){
			burning = false;
		}
		//fuel += w;
		//General.availableEnergy -= w;
		//fuel = Mathf.Max (waterAmount, fuel);
		
	}

	/*void OnCollisionEnter(Collision col){
		Debug.Log ("burn baby burn");
	}*/

	public void Ignite(){
		burning = true;
		dead = true;
		Destroy(currentModel);
		currentModel = (GameObject)Instantiate (burnModels[currentSize], transform.position, transform.rotation);
		currentModel.transform.parent = transform;

		//flame.enableEmission = true;
	}
	public void UnIgnite(){
		burning = false;
		//flame.enableEmission = false;
		Destroy(currentModel);
		currentModel = (GameObject)Instantiate (deadModels[currentSize], transform.position, transform.rotation);
		currentModel.transform.parent = transform;

	}

	void OnTriggerEnter (Collider other){
		//Debug.Log ("foo");

		Fire temp = other.GetComponent<Fire>();
		if (burning && temp != null && temp.enabled){
			General.changeSize(fuel, fuelLimit, 0);
			//General.playerSize += fuel;
			fuel = 0;
			//Debug.Log("shrubbery trigger entered");
			//burning = true;
			//flame.enableEmission = true;
		}

		else {

			FireAttack fA = other.GetComponent<FireAttack>();
			//if (source != null == true){
			if(fA){
				//Debug.Log ("firea attack strength: "+fA.strength.ToString ());
				waterAmount -= fA.strength;
				if (fA.strength > waterAmount){
					//Debug.Log("shrub trigger entered");
					Ignite();
					//burning = true;
					//flame.enableEmission = true;
				}
			}
		}
		Water waterElemental = other.GetComponent<Water>();
		if(burning&&waterElemental&&waterElemental.enabled){
			General.changeSize(-fuel, 100, 0);
			UnIgnite();
		}
		WaterAttack wat = other.GetComponent<WaterAttack>();
		if (wat){
			AddWater(wat.size);
			//Debug.Log("wat, new size = " + waterAmount.ToString ());
		}

	}
	void OnTriggerStay(Collider other){
		Fire temp = other.GetComponent<Fire>();
		if (burning && temp != null && temp.enabled){
			General.changeSize((fuel+fuelLimit)*Time.deltaTime, fuelLimit, 0);
			//General.playerSize += fuel;
			fuel -= (fuel+fuelLimit)*Time.deltaTime;
			Debug.Log("shrubbery trigger stay");
			//burning = true;
			//flame.enableEmission = true;
		}
	}
}
