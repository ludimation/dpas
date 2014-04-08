using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Earth : MonoBehaviour {

	public float rad = 15;
	public Transform lHand;
	public Transform rHand;
	public Transform lowerBound;
	public Transform upperBound;
	//float lowEnergy = 0;
	//float highEnergy = 0;

	public Platform platform;
	public AudioSource audSrc;
	public float smashChargeTime = 1;
	public float smashCharge = 0;
	public float smashCoolTime = .25f;
	public AudioClip smash;
	public float platformChargeTime = 1;
	public float platformCharge = 0;
	public float platformCoolTime = .25f;
	public AudioClip rise;
	public List<AudioClip> riseSounds;
	//public Transform upperIndicator;
	//public Transform lowerIndicator;
	int terrXBound;
	int terrYBound;
	int terrRes;
	float terrXratio;
	float terrYratio;

	public Texture2D smashIcon;
	public List<Texture2D> indicator1;
	int indicator1Level = 0;
	public Texture2D platformIcon;
	public List<Texture2D> indicator2;
	int indicator2Level = 0;

	//public GameObject earthEffect;
	public List<GameObject> effects;
	//public Texture2D heightMap;
	//int terrYRes;

	public Terrain terr;
	public Terrain terrOrig;
	public bool isAwake = false;
	// Use this for initialization
	void Start () {
		terrXratio = terr.terrainData.heightmapWidth/terr.terrainData.size.x;
		terrYratio = terr.terrainData.heightmapHeight/terr.terrainData.size.z;
		//Debug.Log (terrXratio.ToString ());
		
		//terr.enabled = true;
		terr.gameObject.SetActive (true);
		//terrOrig.enabled = false;
		terrOrig.gameObject.SetActive (false);
		//Debug.Log ("terr.enabled = "+ terr.enabled.ToString());
		terrXBound = terr.terrainData.heightmapWidth;
		terrYBound = terr.terrainData.heightmapHeight;

		terrRes = terr.terrainData.detailResolution;
		//float[,] temp = terrOrig.terrainData.GetHeights (0,0, terrOrig.terrainData.heightmapWidth, terrOrig.terrainData.heightmapHeight);
		//terr.terrainData.SetHeights (0,0, temp);
		reset();
		//Debug.Log ("terr.enabled = "+ terr.enabled.ToString());
		/*GameObject t;
		foreach (Transform j in General.g.effectJoints){

			
			
			t = (GameObject)Instantiate(earthEffect, j.position, Quaternion.identity);
			t.transform.parent = j;
			t.tag = "Earth";
			effects.Add (t);
			

			
		}*/

		
	}
	
	// Update is called once per frame
	void Update () {
		/*foreach (GameObject e in effects){
			Debug.Log (e.transform.position.ToString ());
		}*/
		//if(Input.GetKeyUp(KeyCode.L)){
		//	reset();
		//}
		if(!General.kinectControl){
			/*lowEnergy += Time.deltaTime;
			highEnergy += Time.deltaTime;*/
			//if(Input.GetKey(KeyCode.R)){
			//	highEnergy = lowEnergy = 1;
			//}
			if(Input.GetKey(KeyCode.Alpha1)){
				indicator1Level = 2;
				indicator2Level = 0;
			}

			else if(Input.GetKey(KeyCode.Alpha2)){
				indicator1Level = 0;
				indicator2Level = 2;
			}
			else{
				indicator1Level = 1;
				indicator2Level = 1;
				
			}

			if(Input.GetKeyDown (KeyCode.Q)){
				/*General.screenShake.NewImpact ();
				deform(rad);

				audSrc.PlayOneShot (smash);
				smashCharge = -.5f;*/
				Smash();
				//highEnergy -= .75f;
			}
			if(Input.GetKeyDown (KeyCode.E)){
				/*General.screenShake.NewImpact ();
				audSrc.PlayOneShot (riseSounds[Random.Range(0,riseSounds.Count)]);
				Platform temp = (Platform)Instantiate(platform, transform.position + new  Vector3(0,-2, 0), Quaternion.identity);
				temp.target = transform.position;
				temp.initialTime = 1;
				temp.time = 1;
				platformCharge = - .5f;*/
				Platform();
				//lowEnergy -= .75f;
				//temp.start = transform.position + new  Vector3(0,-2, 0)
			}
		}
		//if((.5f*(lHand.position + rHand.position)).y<lowerBound.position.y){
			//if(highEnergy>.75f){
				
			//	audSrc.PlayOneShot (smash);
			//	highEnergy -= .5f;
			//}
			//lowEnergy += Time.deltaTime;
		//}
		//else if((.5f*(lHand.position + rHand.position)).y>upperBound.position.y){
		//	if(lowEnergy>.75f){
		if(Gestures.ArmsUp ()){
			if(platformCharge > platformChargeTime){
				/*General.screenShake.NewImpact ();
				audSrc.PlayOneShot (riseSounds[Random.Range(0,riseSounds.Count)]);
				Platform temp = (Platform)Instantiate(platform, transform.position + new  Vector3(0,-2, 0), Quaternion.identity);
				temp.target = transform.position;
				temp.initialTime = 1;
				temp.time = 1;
				//platformCharge = 0;
				smashCharge = 0;
				platformCharge = -platformCoolTime;*/
				Platform();
				//lowEnergy-=.5f;
			}
			//if(platformCharge <0){
			smashCharge += Time.deltaTime;
			indicator2Level = 0;
			indicator1Level = 2;
			//}
		}
			//highEnergy += Time.deltaTime;

		else if(Gestures.ArmsDown()){
			if(smashCharge > smashChargeTime){
				/*General.screenShake.NewImpact ();
				deform(rad);
				audSrc.PlayOneShot (smash);
				smashCharge = -smashCoolTime;
				platformCharge = 0;*/
				Smash();
				//smashCharge = 0;
			}
			//if(smashCharge < 0){
			platformCharge += Time.deltaTime;
			indicator2Level = 2;
			indicator1Level = 0;
			//}


		}
		else{
			if(platformCharge <0){
				platformCharge += Time.deltaTime;
			}
			else{
				platformCharge -= Time.deltaTime;
				platformCharge = Mathf.Max (0, platformCharge);
				platformCharge = Mathf.Min (1.5f, platformCharge);

			}
			if(smashCharge <0){
				smashCharge += Time.deltaTime;
			}
			else{
				smashCharge -= Time.deltaTime;
				smashCharge = Mathf.Max (0, smashCharge);
				smashCharge = Mathf.Min (1.5f, smashCharge);
			}
			
			indicator2Level = 1;
			indicator1Level = 1;
		}
		//highEnergy = Mathf.Max (highEnergy, 0);
		//lowEnergy = Mathf.Max (lowEnergy, 0);
		//highEnergy = Mathf.Min (highEnergy, 1);
		//lowEnergy = Mathf.Min (lowEnergy, 1);
		
		//lowerIndicator.localScale = (1+platformCharge)*Vector3.one;
		//upperIndicator.localScale = (1+smashCharge)*Vector3.one;
	}
	void Smash(){
		General.screenShake.NewImpact ();
		deform(rad);
		audSrc.PlayOneShot (smash);
		smashCharge = -smashCoolTime;
		platformCharge = 0;
	}
	void Platform(){
		General.screenShake.NewImpact ();
		audSrc.PlayOneShot (riseSounds[Random.Range(0,riseSounds.Count)]);
		Platform temp = (Platform)Instantiate(platform, transform.position + new  Vector3(0,-2, 0), Quaternion.identity);
		temp.target = transform.position;
		temp.initialTime = 1;
		temp.time = 1;
		//platformCharge = 0;
		smashCharge = 0;
		platformCharge = -platformCoolTime;
		Debug.Log (smashCharge.ToString ()+", "+platformCharge.ToString ());

	}
	void deform (float radius){
		float x = transform.position.x-terr.transform.position.x;
		float y = transform.position.z-terr.transform.position.z;


		int tX = (int)(x*terrXratio);
		int tY = (int)(y*terrYratio);
		int tR = (int)(radius*Mathf.Max ((float)terrYratio, (float)terrXratio));

		int x0 = Mathf.Max (tX-tR, 0);
		int y0 = Mathf.Max (tY-tR, 0);
		int dX = Mathf.Min (tR*2, terr.terrainData.heightmapWidth-x0);
		//x0--;
		int dY = Mathf.Min (tR*2, terr.terrainData.heightmapHeight-y0);
		//y0--;
		Debug.Log ("x0 = "+x0.ToString ()+", y0 = "+y0.ToString ()+", dX = "+dX.ToString ()+", dY = "+dY.ToString ());
		//float[,] heights = terr.terrainData.GetHeights (Mathf.Max (tX-tR,0), Mathf.Max (tY-tR, 0), Mathf.Min (tR*2, terr.terrainData.heightmapWidth-(tX+tR)), Mathf.Min (tR*2, terr.terrainData.heightmapHeight-(tY+tR)));
		float[,] heights = terr.terrainData.GetHeights (x0, y0, dX, dY);
		int width = heights.GetUpperBound (0);
		int height = heights.GetUpperBound(1);
		Vector2 temp = Vector2.zero;
		Vector2 p =  tR*Vector2.one;
		float min = float.MaxValue;
		float t;
		for(int i = 0; i<width; ++i){
			for(int j = 0; j<height; ++j){
				temp.x = i;
				temp.y = j;
				t = Vector2.Distance (temp, p);
				if (heights[i,j] < min){
					min = heights[i,j];
				}
				if(t<tR){


				
					t= (t*Mathf.PI)/(radius);
					t = Mathf.Cos (t);
					t *= .02f;
					//Debug.Log (t.ToString());
				

					//heights[i,j] *= 1-t;
					//heights[i,j]-=.2f;
					heights[i,j] *= 1-(.03f*(((tR-t)/tR)*(tR-t)/tR));
				}
				//heights[i,j] -= .01f;
				//Debug.Log (temp.ToString ()+", "+p.ToString ()+": "+Vector2.Distance (temp,p).ToString());
			}
		}
		//Debug.Log("min = "+min.ToString ());
		terr.terrainData.SetHeights (Mathf.Max (tX-tR,0),Mathf.Max (tY-tR, 0), heights);
		//terr.collider.
	}
	void reset(){
		float[,] temp = terrOrig.terrainData.GetHeights (0,0, terrOrig.terrainData.heightmapWidth, terrOrig.terrainData.heightmapHeight);
		terr.terrainData.SetHeights (0,0, temp);
		//terr.enabled= true;
	}
	public void Sleep(){
		//called before deactivating script
		isAwake = false;
		
	}
	public void UnSleep(){
		//called when activating the script
		isAwake = true;
	}

	void OnGUI(){
		if(smashCharge < 0){
			GUI.DrawTexture (new Rect(0, 0, 256, 256), smashIcon);
		}
		else{
			for (int i = 0; i<Mathf.Min(indicator1.Count, indicator1Level); ++i){
				//GUI.Box (new Rect(0, i*256, 256, 256), indicator1[i]);
				GUI.DrawTexture (new Rect(0, i*256, 256, 256), indicator1[i]);
			}
		}
		if(platformCharge <0){
			GUI.DrawTexture (new Rect(256, 0, 256, 256), platformIcon);

		}
		else{
			for (int i = 0; i<Mathf.Min(indicator2.Count, indicator2Level); ++i){
				//GUI.Box (new Rect(256, i*256, 256, 256), indicator2[i]);
				GUI.DrawTexture (new Rect(256, i*256, 256, 256), indicator2[i]);
			}
		}
		//GUI.Box (new Rect(0,0,Screen.width*highEnergy, 75), "Energy");
		//GUI.Box (new Rect(0,Screen.height-75,Screen.width*lowEnergy, 75), "Energy");

	}
}
