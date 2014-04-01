using UnityEngine;
using System.Collections;

public class parentAtRuntime : MonoBehaviour {

	public GameObject RuntimeParent;

	// Use this for initialization
	void Start () {
		transform.parent = RuntimeParent.transform;
		transform.localPosition = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
