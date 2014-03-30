using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudFinder : MonoBehaviour {

	// Use this for initialization
	List<Cloud> clouds;
	void Start () {
		clouds  = new List<Cloud>();

	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (clouds.Count.ToString ()+" clouds found");
	}

	public Cloud Nearest(){
		if(clouds.Count<=0){
			return null;
		}
		float minDist = Vector3.Distance (clouds[0].transform.position, transform.position);;
		float t;
		int index = 0;
		for(int i = 1; i< clouds.Count; ++i){
			t = Vector3.Distance (clouds[i].transform.position, transform.position);
			if (t<minDist){
				minDist = t;
				index = i;
			}
		}
		return clouds[index];
	}
	public void Rain(float amnt){
		foreach (Cloud c in clouds){
			c.Rain(amnt);
		}
	}

	void OnTriggerEnter(Collider other){
		Cloud c = other.GetComponent<Cloud>();
		if (c != null){
			clouds.Add (c);
		}
	}
	void OnTriggerExit(Collider other){
		Cloud c = other.GetComponent<Cloud>();
		if (c != null){
			clouds.Remove (c);
		}
	}
}
