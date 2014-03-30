using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainManager : MonoBehaviour {

	// Use this for initialization
	public Terrain baseTerr;
	public List <Terrain> terrs;
	public int currentTerr = 0;
	void Start () {
		foreach (Terrain t in terrs){
			//t.terrainData.se
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
