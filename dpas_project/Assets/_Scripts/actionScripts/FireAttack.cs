using UnityEngine;
using System.Collections;

public class FireAttack : MonoBehaviour {

	public float strength = 1;
	public float fadeRate = 0;
	public float time = 5;
	public Cloud cloudPrefab;
	public Cloud cloud;
	//public float ceiling = System.Single.MaxValue;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		strength -=  fadeRate * Time.deltaTime;
		General.pushEnergy (fadeRate * Time.deltaTime);
		time -= Time.deltaTime;
		if (strength <= 0 || time < 0 ){
			GameObject.Destroy (gameObject);

		}
	
	}
	void OnCollisionEnter(Collision col){
		gameObject.layer = 0;
	}
	void OnTriggerStay(Collider other){
		Fire f = other.gameObject.GetComponent<Fire>();
		if(f&&f.enabled){
			strength-= Time.deltaTime;
			General.changeSize (Time.deltaTime, 100, 0);
			return;
		}
		Water w = other.gameObject.GetComponent<Water>();
		if(w&&w.enabled){
			
			strength-= Time.deltaTime;
			General.changeSize (-Time.deltaTime, 100, 0);
			if(!cloud){
				cloud = (Cloud)Instantiate(cloudPrefab, transform.position + 5*Vector3.up, Quaternion.identity);
				cloud.size = 2*Time.deltaTime;
			}
			else{
				cloud.size += 2*Time.deltaTime;
			}
		}

	}
}
