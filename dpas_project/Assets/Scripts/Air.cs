using UnityEngine;
using System.Collections;

public class Air : MonoBehaviour {
	public Kinectalogue controller;
	public AudioSource AudSrc;
	public AudioClip launch;
	public ParticleSystem part;
	public Vector3 partRot = Vector3.forward;
	public float size = 1;
	public Vector3 cycloneSize = Vector3.one;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(!General.kinectControl){
			if (Input.GetKey(KeyCode.Space)){
				size += Time.deltaTime;
			}
			if(Input.GetKey (KeyCode.B)){
				size -= Time.deltaTime;
			}
			size = Mathf.Max (.1f, size);
			//Debug.Log ("max = "+Mathf.Max (.1f, size).ToString());
		}
		part.transform.localScale = (1/size)*cycloneSize;

		//part.startLifetime = size*5;
		//partRot = Vector3.forward * size * 50;
		part.transform.Rotate(Time.deltaTime * partRot * size);

	}
}
