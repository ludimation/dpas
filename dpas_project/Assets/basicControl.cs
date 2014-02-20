using UnityEngine;
using System.Collections;

public class basicControl : MonoBehaviour {
	public Kinectalogue inPut;
	public float sensetivity = 10;
	public float pitchSensetivity = 1;
	public float yawSensetivity = 1;
	public float rollSensetivity = 1;
	public float floor = 0;
	public int mode = 0;
	public float deadzone = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (mode == 0){
			//pitch, yaw, roll
			transform.Translate (inPut.getDiff()*Time.deltaTime*sensetivity);
			transform.Rotate (inPut.getPitch()*Time.deltaTime,inPut.getYaw()*Time.deltaTime, inPut.getRoll()*Time.deltaTime);
		}
		else if(mode == 1){
			//pitch, yaw,
			transform.Translate (inPut.getDiff()*Time.deltaTime*sensetivity);
			transform.Rotate (inPut.getPitch()*Time.deltaTime,inPut.getYaw()*Time.deltaTime, 0);
		}
		else if(mode == 2){
			//pitch only
			transform.Translate (inPut.getDiff()*Time.deltaTime*sensetivity);
			transform.Rotate (inPut.getPitch()*Time.deltaTime,0, 0);
		}
		
		else if(mode == 3){
			//pitch, roll
			transform.Translate (inPut.getDiff()*Time.deltaTime*sensetivity);
			transform.Rotate (inPut.getPitch()*Time.deltaTime,0, inPut.getRoll ()*Time.deltaTime);
		}
		else if(mode == 4){
			//yaw only
			Vector3 temp =(inPut.getDiff()*Time.deltaTime*sensetivity);
			if(temp.magnitude<deadzone){
				temp = Vector3.zero;
			}
			transform.Translate (temp);
			transform.Rotate (0,inPut.getYaw()*Time.deltaTime, 0);
		}
		else if (mode == 5){
			//no rotation
			transform.Translate (inPut.getDiff()*Time.deltaTime*sensetivity);

		}
		/*if (transform.position.y < floor){
			transform.Translate (new Vector3(0, - transform.position.y, 0));
		}*/
	}
}
