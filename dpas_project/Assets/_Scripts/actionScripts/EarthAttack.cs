using UnityEngine;
using System.Collections;

public class EarthAttack : MonoBehaviour {
	public float size = 0;
	public float time = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		if(time <=0){
			General.pushEnergy (size);
			Destroy(gameObject);
		}
	
	}
	void OnCollisionEnter(Collision col){
		gameObject.layer = 0;
	}
}
