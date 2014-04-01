using UnityEngine;
using System.Collections;

public class Stream : MonoBehaviour {

	public float size = 1;
	public Cloud cloudPrefab;
	public Cloud cloud;
	public WaterAttack watAtk;
	float emit;
	public float emissionRate;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay (Collider other){
		Water waterElemental = other.GetComponent<Water>();
		if(waterElemental){
			General.changeSize(Time.deltaTime, 100, 0);
			size -= Time.deltaTime;

		}

		Fire f = other.GetComponent<Fire>();
		if (f != null && f.enabled){
			size-= Time.deltaTime;
			General.changeSize (-Time.deltaTime, 100, 0);
			if(!cloud){
				cloud = (Cloud)Instantiate(cloudPrefab, transform.position + 5*Vector3.up, Quaternion.identity);
				cloud.size = 2*Time.deltaTime;
			}
			else{
				cloud.size += 2*Time.deltaTime;
			}
			
			return;

		}
		FireAttack atk = other.GetComponent<FireAttack>();
		if (atk != null){
			if(!cloud){
				cloud = (Cloud)Instantiate(cloudPrefab, transform.position + 5*Vector3.up, Quaternion.identity);
				cloud.size = Mathf.Min (atk.strength, size);
			}
			else{
				cloud.size += Mathf.Min (atk.strength, size);
			}
			atk.strength -= Mathf.Min (atk.strength, size);
			size -= Mathf.Min (atk.strength, size);
		}

	}
}
