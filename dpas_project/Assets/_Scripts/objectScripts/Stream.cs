using UnityEngine;
using System.Collections;

public class Stream : MonoBehaviour {

	public float size = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other){
		Water waterElemental = other.GetComponent<Water>();
		if(waterElemental){
			General.changeSize(size, 100, 0);
			//UnIgnite();
		}

		Fire f = other.GetComponent<Fire>();
		if (f != null && f.enabled){
			General.g.changeElement(General.Element.Air);
			return;
		}
		FireAttack atk = other.GetComponent<FireAttack>();
		if (atk != null){
			Destroy(other.gameObject);
		}

	}
}
