using UnityEngine;
using System.Collections;

public class WaterAttack : MonoBehaviour {
	public float time = 5;
	public float priority;
	public float size = 1;
	public float maxSize = 25f;
	public GameObject collisionHolder;
	SpringJoint spring;
	public ParticleSystem initial;
	public ParticleSystem final;
	//HingeJoint spring;
	// Use this for initialization
	void Start () {
		priority = Random.Range (int.MinValue, int.MaxValue);
		initial.enableEmission = true;
		final.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		if (time < 0){
			Destroy(gameObject);
		}
		//rigidbody.AddForce (size*(Random.rotationUniform*Vector3.up),ForceMode.VelocityChange);
	
	}
	void OnCollisionEnter(Collision col){
		collisionHolder.layer = 0;
		initial.enableEmission = false;
		final.enableEmission = true;

	}
	void OnTriggerEnter(Collider other){
		WaterAttack wA = other.gameObject.GetComponent<WaterAttack>();
		if(wA){
			if(size<maxSize&&wA.size<maxSize){
				size+=wA.size;
				if(priority > wA.priority){
					Destroy(wA.gameObject);

					transform.localScale = Mathf.Pow (size, 1f/3f) * Vector3.one;

					transform.Translate (Vector3.up);
					time = 150;
				}
			}
			else if(!spring){
				spring = (SpringJoint)gameObject.AddComponent ("SpringJoint");
				//spring = (HingeJoint)gameObject.AddComponent ("HingeJoint");
				spring.connectedBody = other.rigidbody;
			}
		}
	}
}
