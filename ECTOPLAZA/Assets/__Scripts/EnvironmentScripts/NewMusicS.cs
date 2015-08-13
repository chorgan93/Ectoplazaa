using UnityEngine;
using System.Collections;

public class NewMusicS : MonoBehaviour {

	public GameObject musicPlayerPrefab;
	public float newMaxVolume = 1;

	// Use this for initialization
	void Start () {
		if (GameObject.Find("MusicPlayer")){
			GameObject.Find("MusicPlayer").GetComponent<BGMS>().ReplaceMusic(GetComponent<AudioSource>().clip, newMaxVolume);
			//print ("change music!");
		}
		else if (GameObject.Find("MusicPlayer(Clone)")){
			GameObject.Find("MusicPlayer(Clone)").GetComponent<BGMS>().ReplaceMusic(GetComponent<AudioSource>().clip, newMaxVolume);
			//print ("change music!");
		}
		else{
			GameObject newMuse = Instantiate(musicPlayerPrefab) as GameObject;
			newMuse.GetComponent<BGMS>().ReplaceMusic(GetComponent<AudioSource>().clip, newMaxVolume);
		}
	}
}
