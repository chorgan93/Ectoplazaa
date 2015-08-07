using UnityEngine;
using System.Collections;

public class BGMS : MonoBehaviour {

	// script for background music player
	private static BGMS instance;

	public static float bgmVolumeMult = 1f;

	private bool alreadyCreated = false;

	private AudioSource ownSource;

	public float fadeInRate = 1f;
	public float maxVolume = 1f;

	private float duckRecover = 0.8f;

	private float targetVolume;
	private float currentVolume;

	private float duckVolumeMult = 0.5f;

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

		targetVolume = maxVolume * bgmVolumeMult;

		currentVolume = ownSource.volume;

		if (currentVolume < targetVolume){
			currentVolume += Time.deltaTime*TimeManagerS.timeMult*fadeInRate;
		}
		else{
			currentVolume = targetVolume;
		}

		ownSource.volume = currentVolume;
	
	}

	public void DuckVolume(){
		currentVolume = targetVolume*duckVolumeMult;
		print ("duck volume");
	}
}
