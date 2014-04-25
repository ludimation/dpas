using UnityEngine;
using System.Collections;

public class ElementChanger : MonoBehaviour {

	// Use this for initialization
	public General.Element elem;
	void Start () {
	
	}
	
	// Update is called once per frame
	//void Update () {
	
	//}
	void OnTriggerEnter(Collider col){
		Debug.Log ("trigger entered" + Time.frameCount.ToString ());
		Debug.Log (LayerMask.LayerToName(col.gameObject.layer));
		Air a = col.GetComponent<Air>();
		if(a){
		//if(col.gameObject.layer == 8){
			General.g.changeElement(elem);
		}
	}
}
