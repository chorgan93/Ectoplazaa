using UnityEngine;
using System.Collections;

public class SlowingAreaS : MonoBehaviour {

	private PlatformSoundS mySoundObj;

	void Start(){

		if (!CurrentModeS.allowHazards){
			gameObject.SetActive(false);

		}

		mySoundObj = GetComponent<PlatformSoundS>();

	}

	// Update is called once per frame
	void OnTriggerEnter (Collider other) {

		if (other.gameObject.GetComponent<PlayerS>()){
			PlayerS playerRef = other.gameObject.GetComponent<PlayerS>();

			if (!playerRef.GetSlowedState()){
				playerRef.TriggerSlow();
			}

			mySoundObj.PlayPlatformSounds();
		}
	
	}

	// Update is called once per frame
	void OnTriggerExit (Collider other) {
		
		if (other.gameObject.GetComponent<PlayerS>()){
			PlayerS playerRef = other.gameObject.GetComponent<PlayerS>();
			
			if (playerRef.GetSlowedState()){
				playerRef.DisableSlow();
			}

			mySoundObj.PlayPlatformSounds();
		}
		
	}
}
