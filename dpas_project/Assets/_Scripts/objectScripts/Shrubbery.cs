using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shrubbery : MonoBehaviour {

	public bool randomizeFuel = false;
	public float fuelAmount = 0;
	public float flameAmount = 0;
	public float waterAmount = 0;

	//public float waterThreshold = 3;
	public float waterLimit = 7;
	//float waterToGrow;

	//public float fuelLimitFactor = 10;
	//public float resistanceFactor = 1;
	public float resistance = 0;
	public float burnRate = .1f;
	public float absorbRate = 1;

	public int state = 0;

	public float growRate = .01f;
	public float growScaleFactor = 10;

	//public List<GameObject> liveModels;
	//public List<GameObject> deadModels;
	//public List<GameObject> burnModels;
	public GameObject liveModel;
	public GameObject deadModel;
	public GameObject burnModel;

	GameObject currentModel;

	public ParticleSystem flame;

	public Cloud cloudPrefab;
	public Vector3 cloudOffset = Vector3.up;
	Cloud cloud;
	//Vector3 currentScale = Vector3.one;


	//public int currentSize = 1;


	// Use this for initialization
	void Start () {
		if(randomizeFuel){
			float r = Random.Range (0.0f, 1.0f);
			fuelAmount = waterLimit*r;
			waterAmount = waterLimit*(1-r);
		}
		//waterToGrow = waterThreshold;
		//if (burnModels.Count != deadModels.Count || deadModels.Count != liveModels.Count){
		//	Debug.LogWarning ("improper number of tree models");
		//}
		ModelChange();

	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("state = " + state.ToString ()+ ", wff = "+new Vector3(waterAmount, fuelAmount, flameAmount).ToString());
		if(state == 1){
			if(waterAmount <= 0){
				++state;
				waterAmount = 0;
				ModelChange();
			}
			else{
				waterAmount -= growRate*Time.deltaTime;
				fuelAmount += growRate*Time.deltaTime;
			}
			
		}

		if(state == 3){
			if (fuelAmount <=0){
				++state;
				ModelChange();

			}
			else{
				flameAmount += burnRate * Time.deltaTime;
				fuelAmount -= burnRate * Time.deltaTime;
			}
		}
		if(state == 4){
			if(flameAmount<=0){
				state = 0;
				flameAmount = fuelAmount = waterAmount = 0;
			}
		}
		/*
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
	*/
	}
	void ModelChange(){
		/*if(burning){
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
		}*/
		if(state == 0){
			Destroy(currentModel);
			flame.enableEmission = false;
		}
		if(state == 1){
			Destroy(currentModel);
			currentModel = (GameObject)Instantiate(liveModel, transform.position, transform.rotation);
			flame.enableEmission = false;
			currentModel.transform.parent = transform;
			//currentModel.transform.localScale = Vector3.one;
		}
		if(state == 2){
			Destroy(currentModel);
			currentModel = (GameObject)Instantiate(deadModel, transform.position, transform.rotation);
			flame.enableEmission = false;
			currentModel.transform.parent = transform;
			//currentModel.transform.localScale = Vector3.one;
		}
		if(state == 3){
			Destroy(currentModel);
			currentModel = (GameObject)Instantiate(burnModel, transform.position, transform.rotation);
			flame.enableEmission = true;
			currentModel.transform.parent = transform;
			//currentModel.transform.localScale = Vector3.one;

		}
		if(state == 4){
			
			Destroy(currentModel);
			flame.enableEmission = true;
		}
		}
	void MakeCloud(float s){
		if(!cloud){
			cloud = (Cloud)Instantiate(cloudPrefab, transform.position+cloudOffset, transform.rotation);
			cloud.size = s;
		}
		else{
			cloud.size += s;
		}
	}
	public void AddWater(float w){
		if(state == 0){
			++state;
			ModelChange();
			waterAmount += w;
			if(waterAmount > waterLimit){
				General.pushEnergy (waterAmount - waterLimit);
				waterAmount = waterLimit;
			}
		}
		else if(state == 1){
			waterAmount += w;
			if(waterAmount > waterLimit){
				General.pushEnergy (waterAmount - waterLimit);
				waterAmount = waterLimit;
			}
		}
		else if(state == 2){
			--state;
			ModelChange();
			waterAmount += w;
			if(waterAmount > waterLimit){
				General.pushEnergy (waterAmount - waterLimit);
				waterAmount = waterLimit;
			}
		}
		else if(state == 3){
			if(w > flameAmount){
				--state;
				ModelChange();
			}
			else{
				flameAmount -= w;
				MakeCloud(2*w);
			}
		}
		else if(state == 4){
			if(w > flameAmount){
				General.pushEnergy (w - flameAmount);
				MakeCloud(2*flameAmount);

				state = 0;
				ModelChange();
			}
			else{
				flameAmount -= w;
				MakeCloud(2*w);
			}

		}
		/*if(!burning){
			waterAmount += w;
			waterToGrow -= w;
			waterAmount = Mathf.Min (waterLimit, waterAmount);
		}
		else{
			flameAmount -= w;
			MakeCloud (2*w);
		}*/
	}



	public void Ignite(float f){
		if(f == -1){
			if(state == 0){
			}
			else if (state == 1){
				state = 3;
				ModelChange();
				General.pushEnergy(waterAmount);
			}
			else if (state == 2){
				state = 3;
				ModelChange();
			}
			else if (state == 3){
			}
			else if (state == 4){
			}
			/*General.pushEnergy (waterAmount);
			if(state == 2 || state == 1){
				state = 3;
				ModelChange();
			}*/
		}
		else{
			if(state == 0){
			}
			else if (state == 1){
				if(f > waterAmount){
					//General.pushEnergy(f-waterAmount);
					fuelAmount += waterAmount;

					flameAmount += f - waterAmount;
					waterAmount = 0;
					state = 3;
					ModelChange();
				}
				else{
					waterAmount -= f;
					fuelAmount += f;
				}
			}
			else if (state == 2){
				flameAmount += f;
				state = 3;
				ModelChange();
				
			}
			else if (state == 3){
				flameAmount += f;
			}
			else if (state == 4){
				flameAmount += f;
			}
			/*if(state == 2){
				if(f > waterAmount){
					//General.pushEnergy(f-waterAmount);
					fuelAmount += waterAmount;
					flameAmount += f;
				}
				else{
					waterAmount -= f;
					fuelAmount += f;
				}
			}*/


		}
		/*burning = true;
		dead = true;
		ModelChange(currentSize);
		*/
		
	}
	/*public void UnIgnite(){
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
	}*/

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
			//waterAmount -= fA.strength;
			//fuel+= fA.strength;
			//if (waterAmount<=0){
				//Debug.Log("shrub trigger entered");
				//Ignite();
				//burning = true;
				//flame.enableEmission = true;
			//}
			////else{
			//	MakeCloud(2*fA.strength);

			//}
			Ignite(fA.strength);
			Destroy(fA.gameObject);
			return;
		}


		WaterAttack wat = other.GetComponent<WaterAttack>();
		if (wat){
			AddWater(wat.size);
			Destroy(wat);
			//Debug.Log("wat, new size = " + waterAmount.ToString ());
		}

	}
	void OnTriggerStay(Collider other){
		Fire fireElemental = other.GetComponent<Fire>();
		//if (burning && fireElemental != null && fireElemental.enabled){
		if(fireElemental && fireElemental.enabled){
			if(state == 1){
				waterAmount -= absorbRate * growRate;
				fuelAmount += absorbRate * growRate;
			}
			else if(state == 2){
				Ignite(absorbRate);
			}
			else if (state > 2){
				flameAmount -= absorbRate*Time.deltaTime;
				General.changeSize (absorbRate*Time.deltaTime, 100, 0);
			}
			return;
		}
		Water waterElemental = other.GetComponent<Water>();
		if(waterElemental&&waterElemental.enabled){
			//General.changeSize(.5f*flameAmount*Time.deltaTime, 100, 0);
			//AddWater(.5f*flameAmount*Time.deltaTime);
			AddWater(absorbRate*Time.deltaTime);
			General.changeSize (-absorbRate*Time.deltaTime, 100, 0);
		}
	}
}
