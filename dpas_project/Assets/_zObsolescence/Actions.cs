using UnityEngine;
using System.Collections;

public class Actions : MonoBehaviour {
	public Transform lHand;
	public Transform rHand;
	public Transform lElbow;
	public Transform rElbow;
	public Transform lShoulder;
	public Transform rShoulder;
	public AudioSource audSrc;
	public AudioClip blast;
	public AudioClip laser;
	public AudioClip boom;
	public AudioClip buzzer;
	public GameObject bullet;

	public float energy = 1;
	float cooldown = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		energy += Time.deltaTime;
		energy = Mathf.Min (energy, 1);
		cooldown -= Time.deltaTime;
		float theta =  Vector3.Angle (lHand.transform.position - lElbow.transform.position, lShoulder.transform.position-lElbow.transform.position);
		float iota = Vector3.Angle (rHand.transform.position - rElbow.transform.position, rShoulder.transform.position-rElbow.transform.position);

		if (theta > 120 && iota > 120 && cooldown <0){
			if(energy > .4f && Vector3.Angle (lHand.transform.position-rHand.transform.position, Vector3.up) < 45 || Vector3.Angle (lHand.transform.position-rHand.transform.position, Vector3.up) > 135){
				audSrc.PlayOneShot (blast);
				GameObject temp = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
				Projectile p = temp.GetComponent<Projectile>();
				p.velocity = 10*(transform.rotation*((lHand.transform.position-lShoulder.transform.position)+(rHand.transform.position-rShoulder.transform.position)));
				p.lifespan = 5;
				temp.transform.localScale=.75f*Vector3.one;
				cooldown = .75f;
				energy -= .4f;
			}
			else if(energy > .8f && Vector3.Angle (lHand.transform.position-lShoulder.transform.position, rHand.transform.position-rShoulder.transform.position) > 150){
				audSrc.PlayOneShot (boom);
				GameObject temp = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
				Projectile p = temp.GetComponent<Projectile>();
				//p.velocity = transform.rotation*((lHand.transform.position-lShoulder.transform.position)+(rHand.transform.position-rShoulder.transform.position));
				p.velocity = Vector3.zero;
				p.lifespan = 2;
				temp.transform.localScale = new Vector3(10, .25f, 10f);
				cooldown = 2;
				energy -= .8f;
			}
			//else if(Vector3.Angle (lHand.transform.position-rHand.transform.position, Vector3.left) < 60 || Vector3.Angle (lHand.transform.position-rHand.transform.position, Vector3.left) > 120){
			else if(energy > .02f && Vector3.Angle (lHand.transform.position-lShoulder.transform.position, rHand.transform.position-rShoulder.transform.position) < 30 
			        && Vector3.Angle (lHand.transform.position-rHand.transform.position, Vector3.up) < 120 
					&& Vector3.Angle (lHand.transform.position-rHand.transform.position, Vector3.up)>60)
			{
				audSrc.PlayOneShot (laser);
				GameObject temp = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
				Projectile p = temp.GetComponent<Projectile>();
				p.velocity = 25*(transform.rotation*((lHand.transform.position-lShoulder.transform.position)+(rHand.transform.position-rShoulder.transform.position)));
				p.lifespan = 5;
				temp.transform.localScale = .25f*Vector3.one;
				cooldown = .1f;
				energy-=.02f;
			}
			else{
				audSrc.PlayOneShot (buzzer);
			}
		
		}
	
	}
	void OnGUI(){
		GUI.Box (new Rect(Screen.width-50, (1-energy)*Screen.height, 50, energy*Screen.height), "energy");

	}	
}
