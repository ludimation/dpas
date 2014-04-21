using UnityEngine;
using System.Collections;

public class Enlarge_Flame : MonoBehaviour {
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale += new Vector3(1* Time.deltaTime, 1* Time.deltaTime, 1* Time.deltaTime);
	
	}
}
