using UnityEngine;
using System.Collections;

public class SFXObjS : MonoBehaviour {

	private AudioSource ownSource;

	public bool duckMusic = false;
	public bool longDuck = false;

	public static float sfxVolumeMult = 1;

	// Use this for initialization
	void Start () { 

		ownSource = GetComponent<AudioSource>();
		// multiply volume by sfx volume mult
		ownSource.volume *= sfxVolumeMult;
		ownSource.Play();

		if (duckMusic){
			if (GameObject.Find("MusicPlayer")){
				if(longDuck){
					GameObject.Find("MusicPlayer").GetComponent<BGMS>().DuckVolumeLonger(ownSource.clip.length);
					//print ("long duck!!");
				}
				else{
					GameObject.Find("MusicPlayer").GetComponent<BGMS>().DuckVolume();
				}
			}
			else if (GameObject.Find("MusicPlayer(Clone)")){
				if(longDuck){
					GameObject.Find("MusicPlayer(Clone)").GetComponent<BGMS>().DuckVolumeLonger(ownSource.clip.length);
					//print ("long duck!!");
				}
				else{
					GameObject.Find("MusicPlayer(Clone)").GetComponent<BGMS>().DuckVolume();
				}
			}
			else{
				
				//print ("didn't find it");
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
