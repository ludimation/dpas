using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutScene : MonoBehaviour {

	public bool dbg = false;
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
		//General.cS = this;
		spot = new Rect(0,0, .5f * Screen.width, .75f * Screen.height);
		//spot2 = new Rect(0,0, Screen.width/2, Screen.height/2);
		screenRect = new Rect(0,0, Screen.width, Screen.height);
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (
		if(textures.Count == 0 || currentPanel > textures.Count){
			//General.cS = null;
			//Destroy(this);
			currentPanel = 0;
			//enabled = false;
		}
		//t += Time.deltaTime;
		if(!dbg){
			//rArm = Gestures.RForearm().normalized;
			rArm = Gestures.RArmDir().normalized;
			flatArm = new Vector3(rArm.x*Screen.width, -rArm.y*Screen.height);
		}
		else{
			//rArm = new Vector3(Input.mousePosition.x - Screen.width, -Input.mousePosition.y - Screen.height, 0);
			rArm = new Vector3(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2, 0);
			flatArm = new Vector3(rArm.x, -rArm.y);
			//flatArm =new Vector2(Input.mousePosition.x;// .25f * new Vector3(rArm.x*Screen.width, -rArm.y*Screen.height);
		}
		if(state == 0){
			//spot.y = flatArm.y;
			spot.y = 0;
			spot.x = Screen.width/2;

			//spot2.y = flatArm.y;
			//spot2.x = (Screen.width/180)*(Vector3.Angle(rArm, Vector3.right)) - t*Screen.height/5;
			//string wtf = 
			//Debug.Log (flatArm.ToString ()+", "+ (.25f * Screen.width).ToString ());
			//if(Vector3.Angle (rArm, Vector3.right)< 60 &&Vector3.Angle (rArm, Vector3.right) > 45){
			if(flatArm.x > .25f * Screen.width){
				++state;
			}
		}
		if (state == 1){
			//spot.x = (Screen.width/180)*(Vector3.Angle(rArm, Vector3.right));
			spot.x = Mathf.Min (Screen.width/2, flatArm.x + Screen.height/2);
			spot.y = flatArm.y;
			//spot.y = Mathf.Clamp (spot.y,0,Screen.heght/foo);
			//spot2.y = flatArm.y;
			//spot2.x = (Screen.width/180)*(Vector3.Angle(rArm, Vector3.right)) - t*Screen.height/5;

			//if(Vector3.Angle(rArm, Vector3.right)<45){
			//	--state;
			//}
			//if(Vector3.Angle(rArm, Vector3.right)>120){
			if(flatArm.x < -.25f * Screen.width){
				--state;
				++currentPanel;
				t = 0;
			}
		}
		//else if (sate == 2){

		//}
		//else{
		//	Debug.LogError("invalid state in cutscene");
		//}
		Debug.Log ("rArm = "+rArm.ToString ()+", flat = "+flatArm.ToString ()+", state = "+state.ToString ());
	}
	void OnGUI(){
		//Debug.Log (spot.ToString ()+",  "+spot2.ToString());
		GUI.DrawTexture (screenRect, background);
		if(currentPanel == 0){
			GUI.DrawTexture (spot, textures[0]);

		}
		else if(currentPanel == textures.Count){
			//GUI.DrawTexture (spot2, textures[currentPanel-1]);

		}
		else if (currentPanel > textures.Count){
			//Destroy(this);
		}
		else{
			//GUI.DrawTexture (spot2, textures[currentPanel-1]);
			GUI.DrawTexture (spot, textures[currentPanel]);

		}
		//GUI.Box (new Rect(Screen.width/2, Screen.height/2, flatArm.x, flatArm.y), state.ToString());
	}
}
