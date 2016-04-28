using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManagerS : MonoBehaviour {

	public static SpawnManagerS Instance;

	private List<DustS> smokeObjs = new List<DustS>();
	public GameObject smokePrefab;
	private List<DustS> electrictyObjs = new List<DustS>();
	public GameObject elecPrefab;

	// Use this for initialization
	void Awake () {

		Instance = this;
	
	}
	
	// Update is called once per frame
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
