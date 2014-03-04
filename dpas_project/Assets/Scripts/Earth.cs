using UnityEngine;
using System.Collections;

public class Earth : MonoBehaviour {

	public Transform lHand;
	public Transform rHand;
	public Transform lowerBound;
	public Transform upperBound;
	float lowEnergy = 0;
	float highEnergy = 0;
	public Platform platform;
	public AudioSource audSrc;
	public AudioClip smash;
	public AudioClip rise;
	public Transform upperIndicator;
	public Transform lowerIndicator;
	int terrXBound;
	int terrYBound;
	int terrRes;
	//int terrYRes;

	public Terrain terr;
	public Terrain terrOrig;
	// Use this for initialization
	void Start () {
		terrXBound = terr.terrainData.heightmapWidth;
		terrYBound = terr.terrainData.heightmapHeight;
		terrRes = terr.terrainData.detailResolution;
		float[,] temp = terrOrig.terrainData.GetHeights (0,0, terrOrig.terrainData.heightmapWidth, terrOrig.terrainData.heightmapHeight);
		terr.terrainData.SetHeights (0,0, temp);
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!General.kinectControl){
			/*lowEnergy += Time.deltaTime;
			highEnergy += Time.deltaTime;*/
			if(Input.GetKey(KeyCode.R)){
				highEnergy = lowEnergy = 1;
			}
			if(Input.GetKeyDown (KeyCode.Q)){
				deform();
				audSrc.PlayOneShot (smash);
				highEnergy -= .75f;
			}
			if(Input.GetKeyDown (KeyCode.E)){
				audSrc.PlayOneShot (rise);
				Platform temp = (Platform)Instantiate(platform, transform.position + new  Vector3(0,-2, 0), Quaternion.identity);
				temp.target = transform.position;
				temp.initialTime = 1;
				temp.time = 1;
				lowEnergy -= .75f;
				//temp.start = transform.position + new  Vector3(0,-2, 0)
			}
		}
		if((.5f*(lHand.position + rHand.position)).y<lowerBound.position.y){
			if(highEnergy>.75f){
				
				audSrc.PlayOneShot (smash);
				highEnergy -= .5f;
			}
			lowEnergy += Time.deltaTime;
		}
		else if((.5f*(lHand.position + rHand.position)).y>upperBound.position.y){
			if(lowEnergy>.75f){
				audSrc.PlayOneShot (rise);
				Platform temp = (Platform)Instantiate(platform, transform.position + new  Vector3(0,-2, 0), Quaternion.identity);
				temp.target = transform.position;
				temp.initialTime = 1;
				temp.time = 31;
				lowEnergy-=.5f;
			}
			highEnergy += Time.deltaTime;
		}
		else {
			highEnergy -= Time.deltaTime;
			lowEnergy -= Time.deltaTime;

		}
		highEnergy = Mathf.Max (highEnergy, 0);
		lowEnergy = Mathf.Max (lowEnergy, 0);
		highEnergy = Mathf.Min (highEnergy, 1);
		lowEnergy = Mathf.Min (lowEnergy, 1);
		
		lowerIndicator.localScale = (1+lowEnergy)*Vector3.one;
		upperIndicator.localScale = (1+highEnergy)*Vector3.one;
	}
	void deform (float radius = 50){
		float x = transform.position.x-terr.transform.position.x;
		float y = transform.position.z-terr.transform.position.z;
		x /= terr.terrainData.size.x;
		y /= terr.terrainData.size.z;
		radius /= terr.terrainData.size.magnitude;
		//Debug.Log (x.ToString() +", "+ y.ToString ());
		x *= terr.terrainData.heightmapWidth;
		y *= terr.terrainData.heightmapHeight;
		radius *= Mathf.Sqrt ((terr.terrainData.heightmapWidth*terr.terrainData.heightmapWidth) + (terr.terrainData.heightmapHeight*terr.terrainData.heightmapHeight));
		//Debug.Log (x.ToString() +", "+ y.ToString ());

		int tX = (int)x;
		//tX = Mathf.Max (tX, 0);
		//tX = Mathf.Min (tX, terr.terrainData.heightmapWidth);
		int tY = (int)y;
		int tR = (int)radius;
		//tY = Mathf.Max (tY, 0);
		//tY = Mathf.Min (tY, terr.terrainData.heightmapHeight);

		//Debug.Log (tX.ToString() +", "+ tY.ToString ());
		float[,] heights = terr.terrainData.GetHeights(tX-tR,tY-tR,tR*2,tR*2);
		int width = heights.GetUpperBound (0);
		int height = heights.GetUpperBound(1);
		Vector2 temp = Vector2.zero;
		Vector2 p =  tR*Vector2.one;
		for(int i = 0; i<width; ++i){
			for(int j = 0; j<height; ++j){
				temp.x = i;
				temp.y = j;
				if(i == tR && j == tR){
					heights[i,j] =.2f;
				}
				else{
				heights[i,j] =.1f;
				}

				Debug.Log (temp.ToString ()+", "+p.ToString ()+": "+Vector2.Distance (temp,p).ToString());
			}
		}
		terr.terrainData.SetHeights (tX-5,tY-5, heights);
	}

	void OnGUI(){
		//GUI.Box (new Rect(0,0,Screen.width*highEnergy, 75), "Energy");
		//GUI.Box (new Rect(0,Screen.height-75,Screen.width*lowEnergy, 75), "Energy");

	}
}
