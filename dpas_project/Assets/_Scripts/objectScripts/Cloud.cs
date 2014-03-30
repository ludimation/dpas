using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	// Use this for initialization
	public float size = 1;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Rain (float amnt){
		Debug.Log ("raining: "+amnt.ToString ());
	}
}
