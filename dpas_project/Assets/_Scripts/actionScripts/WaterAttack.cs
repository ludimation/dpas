using UnityEngine;
using System.Collections;

public class WaterAttack : MonoBehaviour {
	public float time = 5;
	public float priority;
	public float size = 1;
	public GameObject collisionHolder;
	// Use this for initialization
	void Start () {
		priority = Random.Range (int.MinValue, int.MaxValue);
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		if (time < 0){
			Destroy(gameObject);
		}
	
	}
	void OnCollisionEnter(Collision col){
		collisionHolder.layer = 0;

	}
	void OnTriggerEnter(Collider other){
		WaterAttack wA = other.gameObject.GetComponent<WaterAttack>();
		if(wA){
			if(size<25){
				size+=wA.size;
				if(priority > wA.priority){
					Destroy(wA.gameObject);

					transform.localScale = Mathf.Pow (size, 1f/3f) * Vector3.one;

					transform.Translate (Vector3.up);
					time = 15;
				}
			}
		}
	}
}
