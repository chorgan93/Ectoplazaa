using UnityEngine;
using System.Collections;

public class NewMusicS : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (GameObject.Find("MusicPlayer")){
			GameObject.Find("MusicPlayer").GetComponent<BGMS>().ReplaceMusic(GetComponent<AudioSource>().clip);
			//print ("change music!");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
