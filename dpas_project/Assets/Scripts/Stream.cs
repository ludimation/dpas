using UnityEngine;
using System.Collections;

public class Stream : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other){
		Fire f = other.GetComponent<Fire>();
		if (f != null){
			General.g.changeElement(General.Element.Air);
		}

	}
}
