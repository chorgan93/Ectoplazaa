using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManagerS : MonoBehaviour {

	public static SpawnManagerS Instance;

	private List<DustS> smokeObjs = new List<DustS>();
	public GameObject smokePrefab;
	private List<DustS> electrictyObjs = new List<DustS>();
	public GameObject elecPrefab;

	private List<GlobS> globObjs = new List<GlobS>();
	public GameObject globPrefab;

	private List<KOAnimObjS> koObjs = new List<KOAnimObjS>();
	public GameObject koPrefab;
	
	// Use this for initialization
	void Awake () {

		Instance = this;
	
	}

	public void SpawnSmoke (Vector3 pos, Quaternion rot) {

		if (smokeObjs.Count > 0){
			
			smokeObjs[0].TurnOn(pos, rot);
			smokeObjs.RemoveAt(0);
			
		}
		else{
			
			Instantiate(smokePrefab, pos, rot);
		}
	
	}

	public void ReturnSmoke(DustS smoke){

		smokeObjs.Add(smoke);

	}

	// Update is called once per frame
	public GlobS SpawnOrb (Vector3 pos, Quaternion rot) {

		GlobS newGlob;
		
		if (globObjs.Count > 0){
			
			globObjs[0].TurnOn(pos, rot);
			newGlob = globObjs[0];
			globObjs.RemoveAt(0);
			
		}
		else{
			
			GameObject newOrb = Instantiate(globPrefab, pos, rot) as GameObject;
			newGlob = newOrb.GetComponentInChildren<GlobS>();
		}

		return newGlob;
		
	}
	
	public void ReturnOrb(GlobS glob){
		
		globObjs.Add(glob);
		
	}

	public void SpawnKO (Vector3 pos, Quaternion rot) {

		if (koObjs.Count > 0){
			
			koObjs[0].TurnOn(pos, rot);
			koObjs.RemoveAt(0);
			
		}
		else{
			
			Instantiate(koPrefab, pos, rot);
		}
		
	}
	
	public void ReturnKO(KOAnimObjS smoke){
		
		koObjs.Add(smoke);
		
	}

	public void SpawnElectricity (Vector3 pos, Quaternion rot) {

		if (electrictyObjs.Count > 0){
			
			electrictyObjs[0].TurnOn(pos, rot);
			electrictyObjs.RemoveAt(0);
			
		}
		else{
			
			Instantiate(elecPrefab, pos, rot);
		}

	}

	public void ReturnElec(DustS elec){
		electrictyObjs.Add(elec);
	}
}
