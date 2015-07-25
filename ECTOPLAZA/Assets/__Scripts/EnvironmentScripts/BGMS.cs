using UnityEngine;
using System.Collections;

public class BGMS : MonoBehaviour {

	// script for background music player
	private static BGMS instance;

	private bool alreadyCreated = false;

	private AudioSource ownSource;

	public float fadeInRate = 1f;
	public float maxVolume = 1f;

	// Use this for initialization
	void Awake () {

		if (instance != null && instance != this){
			Destroy(gameObject);
		}
		else{
			instance = this;
			//startSceneName = Application.loadedLevelName;
			DontDestroyOnLoad(gameObject);
		}

	}

	void Start () {

		ownSource = GetComponent<AudioSource>();
		//maxVolume = ownSource.volume;

		// in future, allow for multiple bgm players that fade out when going into new scene
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
