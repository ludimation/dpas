using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	public Vector3 target = Vector3.zero;
	public Vector3 start = Vector3.zero;
	public float initialTime = 1f;
	public float time;

	void Start () {
		start = transform.position;
		time = initialTime;
		//Rigidbody temp = (Rigidbody)Instantiate(collider, transform.position, Quaternion.identity);
		//joint.connectedBody = temp;

	
	}
	
	// Update is called once per frame
	void Update () {
		if(time>0){
			//Debug.Log ((time/initialTime).ToString ());
			transform.position = Vector3.Lerp(start, target, 1-(time/initialTime));
			time -= Time.deltaTime;
		}
	
	}
}
