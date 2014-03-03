using UnityEngine;
using System.Collections;

public class Waterfall : MonoBehaviour {

	// Use this for initialization
	public ParticleSystem p;
	public Transform target;
	public bool useGravity;
	float g = 0;
	void Start () {
		g = p.gravityModifier;
		if(g == 0){
			g = .1f;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target.position.y>transform.position.y){
			p.enableEmission = false;
		}
		else{
			p.enableEmission = true;
		}
		if(useGravity){
			Vector3 temp = target.position;
			temp.y = transform.position.y;
			transform.LookAt (temp);
			float timeExpected = .5f*Mathf.Sqrt(Mathf.Abs ((transform.position.y- target.position.y)/g));
			p.startSpeed = (Vector3.Distance(transform.position, temp)/timeExpected);
			p.startLifetime = 1.25f*timeExpected;

		}
		else{
			transform.LookAt (target.position);
		}
	
	}
}
