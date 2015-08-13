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

	private float duckRecover = 0.5f;

	private float targetVolume;
	private float currentVolume;

	private float duckVolumeMult = 0.8f;

	private AudioClip nextMusic;
	private float changeMusicRate = 10f;

	public float delayChange = 0;

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

		
		ownSource = GetComponent<AudioSource>();
	}

	void Start () {

		//maxVolume = ownSource.volume;

		if (GameObject.Find("NewMusic")){
			ReplaceMusic(GameObject.Find("NewMusic").GetComponent<AudioSource>().clip, maxVolume);
			print ("change music!");
		}

		// in future, allow for multiple bgm players that fade out when going into new scene
	
	}
	
	// Update is called once per frame
	void Update () {

		if (delayChange > 0){
			delayChange -= Time.deltaTime;

		}
		else{
		if (nextMusic){
			if (ownSource.volume > 0){
				ownSource.volume -= fadeInRate*bgmVolumeMult*Time.deltaTime;
			}
			else{
				print ("play new thing");
				ownSource.clip = nextMusic;
				ownSource.Play();
				nextMusic = null;
				ownSource.volume = maxVolume*bgmVolumeMult;
			}
		}
		else{
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
		}
	
	}

	public void DuckVolume(){
		if (!nextMusic){
			currentVolume = targetVolume*duckVolumeMult;
			ownSource.volume = currentVolume;
		}
		//print ("duck volume");
	}

	public void DuckVolumeLonger(float delayTime){
		if (!nextMusic){
			currentVolume = 0;
			ownSource.volume = currentVolume;
			delayChange = delayTime;
		}
		//print ("duck volume");
	}

	public void ReplaceMusic(AudioClip muse, float newVolume){
		if (ownSource.clip != muse){
			nextMusic = muse;
			maxVolume = newVolume;
		}
	}
}
