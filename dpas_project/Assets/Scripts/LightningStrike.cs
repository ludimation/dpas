using UnityEngine;
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

	void Start () {
		if(line == null){
			line = gameObject.GetComponent<LineRenderer>();
		}
		strikes = new List<Shrubbery>();
		rigidbody.AddForce (-15 * Vector3.up, ForceMode.VelocityChange);

	
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		if (time < 0){
			Destroy(gameObject);
			return;
		}
		line.SetVertexCount(strikes.Count);
		if(strikes.Count>0){
			line.SetPosition (0, strikes[0].transform.position);
		}
		for (int i = 1; i< strikes.Count; ++i){
			if(strikes[i] == null || strikes[i-1] == null){
				Destroy(gameObject);
				return;
			}
			Debug.DrawRay (strikes[i-1].transform.position, strikes[i].transform.position-strikes[i-1].transform.position);
			
			line.SetPosition (i, strikes[i].transform.position+(7*Vector3.up));
		}
		if (temp != null){
			if(temp.resistance < strength){
				source.AudSrc.PlayOneShot(thunder);
				strikes.Add(temp);
				temp.Ignite();
				Debug.Log ("successful strike, "+ strikes.Count.ToString () + " targets total");
				temp = null;
				General.g.changeElement (General.Element.Fire);



			}
			else{
				strikes.Add(temp);
				temp = null;

			}
		}
		minDist = -1f;
	}

	void OnTriggerEnter(Collider target){
		Shrubbery shrub = target.GetComponent<Shrubbery>();
		if(shrub!=null){
			if (minDist <0){
				minDist = Vector3.Distance(target.transform.position, transform.position);
				temp = shrub;
			}
			else if (minDist>Vector3.Distance(target.transform.position, transform.position)){
				minDist = Mathf.Min (minDist, Vector3.Distance (target.transform.position, transform.position));
				temp = shrub;
			}
		}
	}
}
