﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningStrike : MonoBehaviour {

	// Use this for initialization
	public float strength = 1;
	public float time = 5;
	List<Shrubbery> strikes;
	Shrubbery temp;
	float minDist = -1f;
	public Air source;
	public LineRenderer line;
	public AudioClip thunder;
	//public Transform cloud;
	public Transform boltOrigin;
	public Transform target;

	void Start () {
		if(line == null){
			line = gameObject.GetComponent<LineRenderer>();
		}
		line.SetVertexCount (3);
		boltOrigin = source.transform;
		//strikes = new List<Shrubbery>();
		//rigidbody.AddForce (-15 * Vector3.up, ForceMode.VelocityChange);

	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("wtf");
		/*line.SetPosition (0, transform.position+Vector3.left);
		line.SetPosition (1, transform.position+Vector3.right);
		line.SetPosition (2, transform.position+Vector3.up);*/
		//Debug.Log (transform.position.ToString ()+" lstrike pos");
		if(!boltOrigin){
			boltOrigin = source.transform;
		}
		if(target){
			line.SetPosition (0, boltOrigin.position);
			line.SetPosition (1, transform.position);
			line.SetPosition (2, target.position);
		}
	}

	void OnTriggerEnter(Collider targ){
		//Debug.Log ("Lstrike entering");
		Shrubbery shrub = targ.GetComponent<Shrubbery>();
		if(shrub!=null){
			Cloud temp = source.cF.Nearest ();
			if (!temp){
				Debug.Log ("no cloud found");
				boltOrigin = source.transform;
			}
			else{
				boltOrigin = temp.transform;
			}

			shrub.Ignite ();
			source.audSrc.PlayOneShot(thunder);
			target = targ.transform;
			/*Debug.Log ("Lstrike foundshrub");
			if (minDist <0){
				minDist = Vector3.Distance(target.transform.position, transform.position);
				temp = shrub;
			}
			else if (minDist>Vector3.Distance(target.transform.position, transform.position)){
				minDist = Mathf.Min (minDist, Vector3.Distance (target.transform.position, transform.position));
				temp = shrub;
			}*/
		}
	}
}