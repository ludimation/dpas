using UnityEngine;
using System.Collections;

public class Stream : MonoBehaviour {

	public float size = 1;
	public Cloud cloudPrefab;
	public Cloud cloud;
	public WaterAttack watAtk;
	float emit;
	public float emissionWait = .5f;
	public bool obstacle = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!obstacle&&size<=0){

			Destroy(gameObject);
		}
		if (!obstacle&&emit < 0 &&Time.deltaTime < .1f){
			emit = emissionWait;
			WaterAttack temp = (WaterAttack)Instantiate (watAtk, transform.position, Quaternion.identity);
			temp.size = Mathf.Min (size, .5f);
			temp.gameObject.layer = 9;
			size -= .5f;
		}
		emit -= Time.deltaTime;
	
	}
	void OnTriggerEnter(Collider other){
		WaterAttack w = other.GetComponent<WaterAttack>();
		if(w){
			Debug.Log ("waterAttack found");
			size += w.size;
			Destroy(w.gameObject);
			return;
		}
	}
	void OnTriggerStay (Collider other){

		Water waterElemental = other.GetComponent<Water>();
		if(waterElemental && waterElemental.enabled){
			//Debug.Log ("water elemental, "+obstacle.ToString ()+", "+Time.frameCount.ToString ());
			if(!obstacle){
				General.changeSize(Time.deltaTime, 100, 0);
				size -= Time.deltaTime;
			}
			else{
				General.changeSize (General.pullEnergy(Time.deltaTime), 100, 0);
			}
			return;

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
