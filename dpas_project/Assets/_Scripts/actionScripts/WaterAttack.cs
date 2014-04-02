using UnityEngine;
using System.Collections;

public class WaterAttack : MonoBehaviour {
	public float time = 5;
	public float priority;
	public float size = 1;
	public float maxSize = 25f;
	public float secondaryTimer = 25f;
	public GameObject collisionHolder;
	SpringJoint spring;
	public ParticleSystem initial;
	public ParticleSystem final;
	public Cloud cloudPrefab;
	public Cloud cloud;
	public Stream streamPrefab;
	//HingeJoint spring;
	// Use this for initialization
	void Start () {
		priority = Random.Range (int.MinValue, int.MaxValue);
		initial.enableEmission = true;
		final.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		if (time < 0 || size <=0){
			Destroy(gameObject);
		}
		if(cloud){
			cloud.rigidbody.AddForce(transform.position - cloud.transform.position, ForceMode.Acceleration);
		}
		if(spring&&!spring.connectedBody){
			Destroy(spring);
			spring = null;
		}
		//rigidbody.AddForce (size*(Random.rotationUniform*Vector3.up),ForceMode.VelocityChange);
	
	}
	void OnCollisionEnter(Collision col){
		//gameObject.GetComponent<ParticleSystem>().enableEmission = false;
		//foreach(ParticleSystem p in gameObject.GetComponentsInChildren<ParticleSystem>()){
		//	p.enableEmission = false;
		//}
		collisionHolder.layer = 0;
		initial.enableEmission = false;
		final.enableEmission = true;

	}
	void OnTriggerEnter(Collider other){
		WaterAttack wA = other.gameObject.GetComponent<WaterAttack>();

		if(wA){
			if(size<maxSize&&wA.size<maxSize){
				size+=wA.size;
				if(priority > wA.priority){
					Destroy(wA.gameObject);

					transform.localScale = Mathf.Pow (size, 1f/3f) * Vector3.one;

					transform.Translate (Vector3.up);
					time = secondaryTimer;
				}
			}
			else if(!spring){
				spring = (SpringJoint)gameObject.AddComponent ("SpringJoint");
				//spring = (HingeJoint)gameObject.AddComponent ("HingeJoint");
				spring.connectedBody = other.rigidbody;
				spring.minDistance = 2f;
				spring.maxDistance = 7f;
			}
			else if(time < 5){
				Instantiate(streamPrefab, transform.position+.5f*Vector3.up, Quaternion.identity);
				Destroy(other.gameObject);
				//Destroy(spring.connectedBody.gameObject);
				Destroy(gameObject);
			}

		}
		Shrubbery shrub = other.gameObject.GetComponent<Shrubbery>();
		if (shrub&&shrub.burning){
			shrub.UnIgnite ();
			shrub.AddWater ();
			if(!cloud){
				cloud = (Cloud)Instantiate(cloudPrefab, transform.position + 5*Vector3.up, Quaternion.identity);
				cloud.size = 2*Mathf.Min (shrub.fuel, size);
			}
			else{
				cloud.size += 2*Mathf.Min (shrub.fuel, size);
			}
			size -= Mathf.Min (shrub.fuel, size);
		}

	}
	void OnTriggerStay(Collider other){
		FireAttack fA = other.gameObject.GetComponent<FireAttack>();
		//Water w = other.gameObject.GetComponent<Water>();
		//Fire f = other.gameObject.GetComponent<Fire>();
		if(fA){
			//Cloud temp = (Cloud)Instantiate(cloudPrefab, transform.position + Vector3.Up, Quaternion.identity);
			//temp.size = 2*Mathf.Min(size, fA.Strength);
			if(!cloud){
				cloud = (Cloud)Instantiate(cloudPrefab, transform.position + 5*Vector3.up, Quaternion.identity);
				cloud.size = 2*Mathf.Min (fA.strength, size);
			}
			else{
				cloud.size += 2*Mathf.Min (fA.strength, size);
			}
			fA.strength -= Mathf.Min (fA.strength, size);
			size -= Mathf.Min (fA.strength, size);

		}
		Fire f = other.gameObject.GetComponent<Fire>();
		if(f&&f.enabled){
			size-= Time.deltaTime;
			General.changeSize (-Time.deltaTime, 100, 0);
			if(!cloud){
				cloud = (Cloud)Instantiate(cloudPrefab, transform.position + 5*Vector3.up, Quaternion.identity);
				cloud.size = 2*Time.deltaTime;
			}
			else{
				cloud.size += 2*Time.deltaTime;
			}

			return;

		}
		Water w = other.gameObject.GetComponent<Water>();
		if(w&&w.enabled){
			
			size-= Time.deltaTime;
			General.changeSize (Time.deltaTime, 100, 0);
			/*if(!cloud){
				cloud = (Cloud)Instantiate(cloudPrefab, transform.position + 5*Vector3.up, Quaternion.identity);
				cloud.size = 2*Time.deltaTime;
			}
			else{
				cloud.size += 2*Time.deltaTime;
			}*/
		}
	}
}
