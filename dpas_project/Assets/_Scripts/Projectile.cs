using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public Vector3 velocity = Vector3.zero;
	public float lifespan = 1f;
	public bool mortal = false;
	float initialLife = 1f;
	ParticleEmitter p;

	// Use this for initialization
	void Start () {
		initialLife = lifespan;
		p = gameObject.GetComponent<ParticleEmitter>();
		if (lifespan <0){
			mortal = false;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if (mortal && lifespan<0){
			GameObject.Destroy (gameObject);
		}
		transform.Translate (Time.deltaTime*velocity);
		lifespan -= Time.deltaTime;
		if(mortal && p !=null){
			p.emitterVelocityScale = lifespan/initialLife;
		}

	
	}
}
