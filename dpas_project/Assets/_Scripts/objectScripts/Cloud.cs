using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {
	public WaterAttack raindrop;
	// Use this for initialization
	public float size = 1;
	float rainAmnt;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (rainAmnt >0){
			Instantiate(raindrop, transform.position + size*2*(Random.rotation*Vector3.up), Quaternion.identity);
			//size -= Time.deltaTime;
			rainAmnt -= Time.deltaTime;
		}
	
	}
	public void Rain (float amnt){
		Debug.Log ("raining: "+amnt.ToString ());
		rainAmnt += amnt;
	}
}
