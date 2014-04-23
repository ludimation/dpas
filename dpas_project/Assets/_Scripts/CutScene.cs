using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutScene : MonoBehaviour {

	public List<Texture2D> textures;
	public Texture2D background;
	int currentPanel = 0;
	int state = 0;
	Rect spot;
	Rect spot2;
	Rect screenRect;
	float t = 0;
	//public List<Vector3> dirs;
	// Use this for initialization
	Vector3 rArm;
	Vector2 flatArm;
	void Start () {
		spot = new Rect(0,0, Screen.width/2, Screen.height/2);
		spot2 = new Rect(0,0, Screen.width/2, Screen.height/2);
		screenRect = new Rect(0,0, Screen.width, Screen.height);
	
	}
	
	// Update is called once per frame
	void Update () {
		t+= Time.deltaTime;
		rArm = Gestures.RForearm().normalized;
		flatArm = .25f * new Vector3(rArm.x*Screen.width, -rArm.y*Screen.height);
		if(state == 0){
			if(Vector3.Angle (rArm, Vector3.right)< 60){
				++state;
				spot.y = flatArm.y;
				spot.x = Screen.width/2;
				spot2.y = flatArm.y;
				spot2.x = flatArm.x + t*Screen.height/5;
			}
		}
		else if (state == 1){
			spot.x = flatArm.x;
			spot.y = flatArm.x;
			spot2.y = flatArm.y;
			spot2.x = flatArm.x + t*Screen.height/5;

			if(Vector3.Angle(rArm, Vector3.right)<45){
				--state;
			}
			if(Vector3.Angle(rArm, Vector3.right)>120){
				--state;
				++currentPanel;
				t = 0;
			}
		}
		//else if (sate == 2){

		//}
		else{
			Debug.LogError("invalid state in cutscene");
		}
	}
	void OnGUI(){
		GUI.DrawTexture (screenRect, background);
		if(currentPanel == 0){
			GUI.DrawTexture (spot, textures[0]);

		}
		else if(currentPanel == textures.Count){
			GUI.DrawTexture (spot2, textures[currentPanel-1]);

		}
		else if (currentPanel > textures.Count){
			//Destroy(this);
		}
		else{
			GUI.DrawTexture (spot2, textures[currentPanel-1]);
			GUI.DrawTexture (spot, textures[currentPanel]);

		}

	}
}
