using UnityEngine;
using System.Collections;

public class FireAttack : MonoBehaviour {

	public float strength = 1;
	public float fadeRate = 0;
	public float time = 5;
	//public float ceiling = System.Single.MaxValue;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		strength -=  fadeRate * Time.deltaTime;
		time -= Time.deltaTime;
		if (strength < 0 || time < 0 ){
			GameObject.Destroy (gameObject);
		}
	
	}
}
