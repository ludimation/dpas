using UnityEngine;
using System.Collections;

public class destroyTimer : MonoBehaviour {
	public float timer = 2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(timer<=0){
			Destroy(gameObject);
		}
		timer -= Time.deltaTime;
	}
}
