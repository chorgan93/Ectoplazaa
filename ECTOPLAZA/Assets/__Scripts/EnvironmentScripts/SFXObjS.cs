using UnityEngine;
using System.Collections;

public class SFXObjS : MonoBehaviour {

	private AudioSource ownSource;

	public bool duckMusic = false;

	public static float sfxVolumeMult = 1;

	// Use this for initialization
	void Start () {

		ownSource = GetComponent<AudioSource>();
		// multiply volume by sfx volume mult
		ownSource.volume *= sfxVolumeMult;
		ownSource.Play();

		if (duckMusic){
			if (GameObject.Find("MusicPlayer")){
				GameObject.Find("MusicPlayer").GetComponent<BGMS>().DuckVolume();
			}
		}
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!ownSource.isPlaying){
			Destroy(gameObject);
		}
	}
}
