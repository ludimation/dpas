using UnityEngine;
using System.Collections;

public class Kinectalogue : MonoBehaviour {
	
	public int mode = 0;
	//public float posMin  = -1f;
	public float posMax = 1f;
	//public float rotMin = -90f;
	public float rotMax = 45f;
	
	public Transform target;
	Vector3 reference;
	//Vector3 reference = Vector3.zero;
	public Transform rShoulder;
	public Transform lShoulder;
	public Transform lHip;
	public Transform rHip;
	public Transform lHand;
	public Transform rHand;
	public Transform lElbow;
	public Transform rElbow;
	
	
	public bool debugGUI = false;
	string debugMSG;
	public bool calibrateGUI = false;
	float yaw = 0;
	float pitch = 0;
	float roll = 0;
	float posMagnitude = 0;
	//float posMagnitude;
	Vector3 diff;
	//Vector3 rot;
	// Use this for initialization
	void Start () {
		reference = new Vector3(0,0,0);
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (mode == 1){//shoulders relative to hips, position for direction.
			
			Vector3 shoulderDiff = rShoulder.transform.position - lShoulder.transform.position;
			Vector3 hipDiff = rHip.transform.position - lHip.transform.position;
			
			Vector3 flatHips = new Vector3 (1, 0, 1);
			Vector3 flatShoulders = new Vector3(1,0,1);
			flatHips.Scale(hipDiff);
			flatHips = Vector3.right;//depth on hips is attrocious, assume hips are always straight on for now
			flatShoulders.Scale(shoulderDiff);
			
			
			diff = target.transform.position-reference;
			diff.Scale (new Vector3(1,0,1));
			posMagnitude = diff.magnitude;
			diff.y = (lHand.transform.position.y-lShoulder.transform.position.y)+(rHand.transform.position.y-rShoulder.transform.position.y);
			if(diff.magnitude > posMax){
				diff.Normalize();
				diff *= posMax;

			}

			diff.x *= -1;
			
			Vector3 shoulders = rShoulder.transform.position + lShoulder.transform.position;
			Vector3 hips =rHip.transform.position + lHip.transform.position;
			shoulders *=.5f;
			hips *= .5f;
			
			yaw = Vector3.Angle (flatHips,flatShoulders);
			
			if(hipDiff.z+shoulderDiff.z<0){//should be true iff angle needs to be negated
				yaw *= -1;
			}
			if(Vector3.Angle (flatHips, Vector3.right) < Vector3.Angle (flatShoulders, Vector3.right)){//should be true iff angle needs to be negated
				yaw *=-1;
			}
			roll = -Vector3.Angle (flatShoulders, shoulderDiff);
			if(lShoulder.transform.position.y<rShoulder.transform.position.y){//should be true iff angle needs to be negated
				roll *= -1;
			}
			pitch = -Vector3.Angle (hips-shoulders, Vector3.down);
			if((hips-shoulders).z<0){
				pitch *= -1;
			}
			
			debugMSG = "PITCH = "+pitch.ToString ("#.00")+"\nROLL = "+roll.ToString ("#.00")+"\nYAW = "+yaw.ToString ("#.00")+"\nDIFF = " + diff.ToString ("#.00")+"\nREFF = "+reference.ToString ("#.00");
			
		
		}
		if (mode == 3)//suparman v2, hands relative to shoulders, difference for direction.
		{
			Vector3 handDiff = rHand.transform.position - lHand.transform.position;
			Vector3 shoulderDiff = rShoulder.transform.position - lShoulder.transform.position;
			Vector3 hands = rHand.transform.position + lHand.transform.position;
			Vector3 shoulders = rShoulder.transform.position + lShoulder.transform.position;
			Vector3 hips =rHip.transform.position + lHip.transform.position;
			hands *=.5f;
			shoulders *=.5f;
			hips *= .5f;
			//handDiff.Normalize();
			//shoulderDiff.Normalize();
			
			
			Vector3 flatHands = new Vector3 (1, 0, 1);
			Vector3 flatShoulders = new Vector3(1,0,1);
			flatHands.Scale(handDiff);
			flatShoulders.Scale(shoulderDiff);
			
			pitch = -Vector3.Angle (hips-shoulders, Vector3.down);
			if((hips-shoulders).z<0){
				pitch *= -1;
			}
			roll = -Vector3.Angle (flatHands, handDiff);
			if(lHand.transform.position.y<rHand.transform.position.y){//should be true iff angle needs to be negated
				roll *= -1;
			}
			
			yaw = -Vector3.Angle (flatHands,flatShoulders);
			
			if(handDiff.z+shoulderDiff.z<0){//should be true iff angle needs to be negated
				yaw *= -1;
			}
			if(Vector3.Angle (flatHands, Vector3.right) < Vector3.Angle (flatShoulders, Vector3.right)){//should be true iff angle needs to be negated
				yaw *=-1;
			}
			//float roll = Vector3.Angle (Vector3(handDiff.x, 0, handiff.z),Vector3(shoulderDiff.x, 0, shoulderDiff.z));
			diff = hands - shoulders;
			float tempAngle = Vector3.Angle (lHand.transform.position-lElbow.transform.position, lShoulder.transform.position-lElbow.transform.position);
			tempAngle += Vector3.Angle (rHand.transform.position-rElbow.transform.position, rShoulder.transform.position-rElbow.transform.position);
			float tA = tempAngle;
			if(tempAngle<=120&&tempAngle>=-120){
				diff = Vector3.zero;
			}
			tempAngle -= 90;
			tempAngle /=270;
			diff.Normalize ();
			diff *= tempAngle;
			debugMSG = "PITCH = "+pitch.ToString ("#.00")+"\nROLL = "+roll.ToString ("#.00")+"\nYAW = "+yaw.ToString ("#.00")+"\nDIFF = " + diff.ToString ("#.00")+"\nREFF = "+reference.ToString ("#.00")+"\nElbowAngle = "+tA.ToString ("#.00");
			//sensor loses hands near body, leads to weird jumpiness, loss of control
		}
	}
	public Vector3 getDiff(){
		return diff;
	}
	public Vector3 getRot(){
		return new Vector3(pitch, yaw, roll);
	}
	public float getYaw(){
		return yaw;
	}
	public float getRoll(){
		return roll;
	}
	public float getPitch(){
		return pitch;
	}
	public void calibratePosition(){
		reference = target.transform.position;
	}
	public float getMagnitudeRaw(){
		return posMagnitude;
	}

	
	void OnGUI(){
		if (debugGUI){
			GUIStyle blarg = new GUIStyle();
			blarg.fontSize = 32;
			GUI.TextArea(new Rect(10, 100, 500, 200), debugMSG, blarg);
		}
		if (calibrateGUI){
			if(GUI.Button (new Rect(10,10, 50, 50), "click to calibrate position")){
				calibratePosition();
			}
		}
		
	}
		
}
