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

	public Terrain terr;
	public Terrain terrOrig;
	// Use this for initialization
	void Start () {
		terr.terrainData = terrOrig.terrainData;
	
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
	void deform (){
		float[,] heights = terr.terrainData.GetHeights(0,0,300,300);
		int width = heights.GetUpperBound (0);
		int height = heights.GetUpperBound(1);
		for(int i = 0; i<width; ++i){
			for(int j = 0; j<height; ++j){
				heights[i,j] -= 15;
			}
		}
		terr.terrainData.SetHeights (0,0, heights);
	}

	void OnGUI(){
		//GUI.Box (new Rect(0,0,Screen.width*highEnergy, 75), "Energy");
		//GUI.Box (new Rect(0,Screen.height-75,Screen.width*lowEnergy, 75), "Energy");

	}
}
