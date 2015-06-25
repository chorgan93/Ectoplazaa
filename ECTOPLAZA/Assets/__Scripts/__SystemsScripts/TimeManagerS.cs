using UnityEngine;
using System.Collections;

public class TimeManagerS : MonoBehaviour {
	
	public static float timeMult = 1;

	public static bool paused = false;
	
	public static void SetTimeScale (float newScale){
		timeMult = newScale;
	}

	public static void PauseOn(){
		paused = true;
	}
	public static void PausedOff(){
		paused = false;
	}
}
