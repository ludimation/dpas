using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shrubbery : MonoBehaviour {

	public float fuel = 0;
	public float fuelLimitFactor = 10;
	public float resistanceFactor = 1;
	public float burnRate = .1f;
	public bool burning = false;
	public bool dead = false;
	public float growRate = .01f;
	public float growScaleFactor = 10;

	public List<GameObject> liveModels;
	public List<GameObject> deadModels;
	public List<GameObject> burnModels;
	GameObject currentModel;
	public float flameAmount;

	public Cloud cloudPrefab;
	public Vector3 cloudOffset = Vector3.up;
	Cloud cloud;
	//Vector3 currentScale = Vector3.one;

	public float waterAmount = 0;
	public float waterThreshold = 3;
	public float waterLimit = 7;
	float waterToGrow;

	public int currentSize = 0;


	// Use this for initialization
	void Start () {
		waterToGrow = waterThreshold;
		if (burnModels.Count != deadModels.Count || deadModels.Count != liveModels.Count){
			Debug.LogWarning ("improper number of tree models");
		}

	
	}
	
	// Update is called once per frame
	void Update () {
		if(fuel<=0&&flameAmount<=0&&waterAmount<=0){
			currentSize = 0;
			dead = false;
			burning = false;
			ModelChange(0);
			UnIgnite();
			waterToGrow = waterThreshold;
		}
		if(waterToGrow <=0){
			++currentSize;
			currentSize = Mathf.Min(currentSize, liveModels.Count-1);
			waterToGrow = waterThreshold;
		}
		if(burning&&fuel<=0){
			//UnIgnite();
			fuel = 0;
		}
		else if (burning){
			fuel -= burnRate * Time.deltaTime;
			flameAmount += burnRate * Time.deltaTime;
			//General.pushEnergy (burnRate*Time.deltaTime);
		}
		if(!dead&&currentSize>0){
			waterAmount -= growRate * Time.deltaTime;
			fuel += growRate * Time.deltaTime;
			//transform.localScale = 1 + (fuel);
			//currentModel.transform.localScale = currentScale;
		}
		if(waterAmount<=0&&currentSize>0){
			dead = true;
			ModelChange(currentSize);
		}
		transform.localScale = (1 + fuel) * Vector3.one;
		Debug.Log ("wa = "+waterAmount.ToString ()+", fu = "+fuel.ToString ()+", fl = "+flameAmount.ToString()+", dead = "+dead.ToString ()+", burning = "+burning.ToString ()+", size = "+currentSize.ToString()+", wtg = "+waterToGrow.ToString());
	
	}
	void ModelChange(int newSize){
		if(burning){
			Destroy(currentModel);
			currentModel = (GameObject)Instantiate(burnModels[newSize], transform.position, transform.rotation);
		}
		else if (dead){
			Destroy(currentModel);
			currentModel = (GameObject)Instantiate(deadModels[newSize],transform.position, transform.rotation);
		}
		else{
			Destroy(currentModel);
			currentModel = (GameObject)Instantiate(liveModels[newSize],transform.position, transform.rotation);
		}
		currentModel.transform.parent = transform;

		//currentModel.transform.localScale = currentScale;
	}
	void MakeCloud(float s){
		if(!cloud){
			cloud = (Cloud)Instantiate(cloudPrefab, transform.position+(cloudOffset*(Mathf.Max (Mathf.Max (waterAmount, fuel), flameAmount))), transform.rotation);
			cloud.size = s;
		}
		else{
			cloud.size += s;
		}
	}
	public void AddWater(float w){

		if(!burning){
			waterAmount += w;
			waterToGrow -= w;
			waterAmount = Mathf.Min (waterLimit, waterAmount);
		}
		else{
			flameAmount -= w;
			MakeCloud (2*w);
		}
	}



	public void Ignite(){
		burning = true;
		dead = true;
		ModelChange(currentSize);

		
	}
	public void UnIgnite(){
		burning = false;
		Destroy(currentModel);
		//currentModel = (GameObject)Instantiate (deadModels[currentSize], transform.position, transform.rotation);
		currentModel.transform.parent = transform;
		ParticleSystem[] ps = currentModel.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem p in ps){
			p.enableEmission = false;
		}
		//if(fuel<0){
		//	MakeCloud(fuel);
		//}
	}

	void OnTriggerEnter (Collider other){
		//Debug.Log ("foo");

		/*Fire temp = other.GetComponent<Fire>();
		if (burning && temp != null && temp.enabled){
			General.changeSize(fuel, fuelLimit, 0);
			//General.playerSize += fuel;
			fuel = 0;
			return;
		}*/

		FireAttack fA = other.GetComponent<FireAttack>();
		//if (source != null == true){
		if(fA){
			//Debug.Log ("firea attack strength: "+fA.strength.ToString ());
			waterAmount -= fA.strength;
			//fuel+= fA.strength;
			if (waterAmount<=0){
				//Debug.Log("shrub trigger entered");
				Ignite();
				//burning = true;
				//flame.enableEmission = true;
			}
			else{
				MakeCloud(2*fA.strength);

			}
			Destroy(fA.gameObject);
			return;
		}


		WaterAttack wat = other.GetComponent<WaterAttack>();
		if (wat){
			AddWater(wat.size);
			//Debug.Log("wat, new size = " + waterAmount.ToString ());
		}

	}
	void OnTriggerStay(Collider other){
		Fire fireElemental = other.GetComponent<Fire>();
		if (burning && fireElemental != null && fireElemental.enabled){
			General.changeSize(flameAmount*Time.deltaTime+.1f, 100, 0);
			flameAmount -= flameAmount*Time.deltaTime+.1f;
			//General.playerSize += fuel;
			//fuel -= (fuel+fuelLimit)*Time.deltaTime;
			return;
		}
		Water waterElemental = other.GetComponent<Water>();
		if(burning&&waterElemental&&waterElemental.enabled){
			General.changeSize(.5f*flameAmount*Time.deltaTime, 100, 0);
			AddWater(.5f*flameAmount*Time.deltaTime);

		}
	}
}
