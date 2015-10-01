using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformSoundS : MonoBehaviour {

	private float delaySound;

	public List<GameObject> impactSoundObjs;

	void Update () {
		if (delaySound > 0){
			delaySound -= Time.deltaTime*TimeManagerS.timeMult;
		}
	}

	public void PlayPlatformSounds(){

		if (delaySound <= 0){

			int soundToPlay = Mathf.FloorToInt(Random.Range(0,impactSoundObjs.Count));
	
			Instantiate(impactSoundObjs[soundToPlay]);
	
			//print ("played!!");
	
			delaySound = 0.1f;

		}

	}

}
