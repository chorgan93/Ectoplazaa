using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioSnapshotManagerS : MonoBehaviour {

	private int currentSceneNum = -1;

	public AudioMixerSnapshot[] sceneSnapshots;
	
	// Update is called once per frame
	void Update () {
	
		if (Application.loadedLevel != currentSceneNum){
			currentSceneNum = Application.loadedLevel;
			sceneSnapshots[currentSceneNum].TransitionTo(0.1f);
		}

	}
}
