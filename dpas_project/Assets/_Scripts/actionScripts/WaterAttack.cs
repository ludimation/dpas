﻿using UnityEngine;
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
	public Cloud cloudPrefab;
	public Cloud cloud;
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
		if (time < 0 || size <=0){
			Destroy(gameObject);
		}
		if(cloud){
			cloud.rigidbody.AddForce(transform.position - cloud.transform.position, ForceMode.Acceleration);
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
	void OnTriggerStay(Collider other){
		FireAttack fA = other.gameObject.GetComponent<FireAttack>();
		//Water w = other.gameObject.GetComponent<Water>();
		//Fire f = other.gameObject.GetComponent<Fire>();
		if(fA){
			//Cloud temp = (Cloud)Instantiate(cloudPrefab, transform.position + Vector3.Up, Quaternion.identity);
			//temp.size = 2*Mathf.Min(size, fA.Strength);
			if(!cloud){
				cloud = (Cloud)Instantiate(cloudPrefab, transform.position + 5*Vector3.up, Quaternion.identity);
				cloud.size = Mathf.Min (fA.strength, size);
			}
			else{
				cloud.size += Mathf.Min (fA.strength, size);
			}
			fA.strength -= Mathf.Min (fA.strength, size);
			size -= Mathf.Min (fA.strength, size);

		}
		Fire f = other.gameObject.GetComponent<Fire>();
		if(f&&f.enabled){
			size-= Time.deltaTime;
			General.changeSize (-Time.deltaTime, 100, 0);
			if(!cloud){
				cloud = (Cloud)Instantiate(cloudPrefab, transform.position + 5*Vector3.up, Quaternion.identity);
				cloud.size = 2*Time.deltaTime;
			}
			else{
				cloud.size += 2*Time.deltaTime;
			}

			return;

		}
		Water w = other.gameObject.GetComponent<Water>();
		if(w&&w.enabled){
			
			size-= Time.deltaTime;
			General.changeSize (Time.deltaTime, 100, 0);
			/*if(!cloud){
				cloud = (Cloud)Instantiate(cloudPrefab, transform.position + 5*Vector3.up, Quaternion.identity);
				cloud.size = 2*Time.deltaTime;
			}
			else{
				cloud.size += 2*Time.deltaTime;
			}*/
		}
	}
}
