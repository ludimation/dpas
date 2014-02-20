using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public Vector3 velocity = Vector3.zero;
	public float lifespan = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (lifespan<0){
			GameObject.Destroy (gameObject);
		}
		transform.Translate (Time.deltaTime*velocity);
		lifespan -= Time.deltaTime;
	
	}
}
