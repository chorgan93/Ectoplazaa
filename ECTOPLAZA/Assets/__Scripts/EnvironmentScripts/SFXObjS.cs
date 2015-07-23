using UnityEngine;
using System.Collections;

public class SFXObjS : MonoBehaviour {

	private AudioSource ownSource;

	// Use this for initialization
	void Start () {

		ownSource = GetComponent<AudioSource>();
		ownSource.Play();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!ownSource.isPlaying){
			Destroy(gameObject);
		}
	}
}
