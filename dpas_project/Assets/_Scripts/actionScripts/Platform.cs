using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	public Vector3 target = Vector3.zero;
	public Vector3 start = Vector3.zero;
	public float initialTime = 1f;
	public float time;
	public Transform source;
	public float endDistance;

	void Start () {
		start = transform.position;
		time = initialTime;
		//Rigidbody temp = (Rigidbody)Instantiate(collider, transform.position, Quaternion.identity);
		//joint.connectedBody = temp;

	
	}
	
	// Update is called once per frame
	void Update () {
		//if(time>0){
		Debug.Log (transform.position.ToString () +", "+target.ToString ());
		if(transform.position.y > target.y || transform.position.y > source.transform.position.y){
			//rigidbody.FreezePosition = true;
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			Destroy(this);
		}
			//Debug.Log ((time/initialTime).ToString ());
			//transform.position = Vector3.Lerp(start, target, 1-(time/initialTime));
			//rigidbody.AddForce (targettransform.position,
		else{
			//time -= Time.deltaTime;
			if(source){
				rigidbody.AddForce(source.transform.position - transform.position, ForceMode.Acceleration);
				//source.Translate (new Vector3(0, transform.position.y +( .51f * transform.localScale.y)-source.position.y, 0));
				if(source.position.y<transform.position.y +( .51f * transform.localScale.y)){
					source.Translate (new Vector3(0, transform.position.y +( .51f * transform.localScale.y)-source.position.y, 0));
				}
			}
		}
	
	}
	/*void OnCollisionEnter(Collision other){
		Earth e = other.collider.gameObject.GetComponent<Earth>();
		if(e){
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			Destroy(this);
		}
		
	}
	void OnCollisionStay(Collision other){
		Earth e = other.collider.gameObject.GetComponent<Earth>();
		if(e){
			rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			Destroy(this);
		}
		
	}*/
}
