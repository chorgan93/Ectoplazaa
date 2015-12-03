using UnityEngine;
using System.Collections;

public class PlatformS : MonoBehaviour {

	// reference in any script that takes controller input
	// in order to take input for the correct platform

	public static string GetPlatform(){

		// PS4/Wii U will need to be added once those dev kits come in

		// if windows...
		if (Application.platform == RuntimePlatform.WindowsEditor ||
		    Application.platform == RuntimePlatform.WindowsPlayer ||
		    Application.platform == RuntimePlatform.WindowsWebPlayer){
			return ("PC");
		}

		// if mac...
		else if (Application.platform == RuntimePlatform.OSXEditor ||
		         Application.platform == RuntimePlatform.OSXPlayer ||
		         Application.platform == RuntimePlatform.OSXWebPlayer){
			return ("Mac");
		}

		// if neither...
		else{
			return ("Linux");
		}

	}
}
