using UnityEngine;
using System.Collections;

public class StageLoaderS : MonoBehaviour {

	public GameObject stagePrefab;

	public void LoadLevel(){
		Instantiate(stagePrefab, transform.position, Quaternion.identity);
	}
}
