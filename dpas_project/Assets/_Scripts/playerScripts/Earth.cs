using UnityEngine;
using System.Collections;

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
	public AudioClip smash;
	public float platformChargeTime = 1;
	public float platformCharge = 0;
	public AudioClip rise;
	public Transform upperIndicator;
	public Transform lowerIndicator;
	int terrXBound;
	int terrYBound;
	int terrRes;
	float terrXratio;
	float terrYratio;

	public Texture2D heightMap;
	//int terrYRes;

	public Terrain terr;
	public Terrain terrOrig;
	public bool isAwake = false;
	// Use this for initialization
	void Start () {
		terrXratio = terr.terrainData.heightmapWidth/terr.terrainData.size.x;
		terrYratio = terr.terrainData.heightmapHeight/terr.terrainData.size.z;
		//Debug.Log (terrXratio.ToString ());
		
		terr.enabled = true;
		terrOrig.enabled = false;
		//Debug.Log ("terr.enabled = "+ terr.enabled.ToString());
		terrXBound = terr.terrainData.heightmapWidth;
		terrYBound = terr.terrainData.heightmapHeight;

		terrRes = terr.terrainData.detailResolution;
		//float[,] temp = terrOrig.terrainData.GetHeights (0,0, terrOrig.terrainData.heightmapWidth, terrOrig.terrainData.heightmapHeight);
		//terr.terrainData.SetHeights (0,0, temp);
		reset();
		//Debug.Log ("terr.enabled = "+ terr.enabled.ToString());

		
	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.GetKeyUp(KeyCode.L)){
		//	reset();
		//}
		if(!General.kinectControl){
			/*lowEnergy += Time.deltaTime;
			highEnergy += Time.deltaTime;*/
			//if(Input.GetKey(KeyCode.R)){
			//	highEnergy = lowEnergy = 1;
			//}
			if(Input.GetKeyDown (KeyCode.Q)){
				deform(rad);
				audSrc.PlayOneShot (smash);
				//highEnergy -= .75f;
			}
			if(Input.GetKeyDown (KeyCode.E)){
				audSrc.PlayOneShot (rise);
				Platform temp = (Platform)Instantiate(platform, transform.position + new  Vector3(0,-2, 0), Quaternion.identity);
				temp.target = transform.position;
				temp.initialTime = 1;
				temp.time = 1;
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
				audSrc.PlayOneShot (rise);
				Platform temp = (Platform)Instantiate(platform, transform.position + new  Vector3(0,-2, 0), Quaternion.identity);
				temp.target = transform.position;
				temp.initialTime = 1;
				temp.time = 31;
				platformCharge = 0;
				//lowEnergy-=.5f;
			}
			smashCharge += Time.deltaTime;
		}
			//highEnergy += Time.deltaTime;

		else if(Gestures.ArmsDown()){
			if(smashCharge > smashChargeTime){
				audSrc.PlayOneShot (smash);
				smashCharge = 0;
			}
			platformCharge += Time.deltaTime;


		}
		else{
			platformCharge -= Time.deltaTime;
			smashCharge -= Time.deltaTime;
			platformCharge = Mathf.Max (1.5f, platformCharge);
			smashCharge = Mathf.Max (1.5f, platformCharge);
		}
		//highEnergy = Mathf.Max (highEnergy, 0);
		//lowEnergy = Mathf.Max (lowEnergy, 0);
		//highEnergy = Mathf.Min (highEnergy, 1);
		//lowEnergy = Mathf.Min (lowEnergy, 1);
		
		lowerIndicator.localScale = (1+platformCharge)*Vector3.one;
		upperIndicator.localScale = (1+smashCharge)*Vector3.one;
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
		//GUI.Box (new Rect(0,0,Screen.width*highEnergy, 75), "Energy");
		//GUI.Box (new Rect(0,Screen.height-75,Screen.width*lowEnergy, 75), "Energy");

	}
}
