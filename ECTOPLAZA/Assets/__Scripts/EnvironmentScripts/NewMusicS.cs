using UnityEngine;
using System.Collections;

public class NewMusicS : MonoBehaviour {

	public GameObject musicPlayerPrefab;

	// Use this for initialization
	void Start () {
		if (GameObject.Find("MusicPlayer")){
			GameObject.Find("MusicPlayer").GetComponent<BGMS>().ReplaceMusic(GetComponent<AudioSource>().clip);
			//print ("change music!");
		}
		else{
			GameObject newMuse = Instantiate(musicPlayerPrefab) as GameObject;
			newMuse.GetComponent<BGMS>().ReplaceMusic(GetComponent<AudioSource>().clip);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
