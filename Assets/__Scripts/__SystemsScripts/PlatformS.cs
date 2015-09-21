using UnityEngine;
using System.Collections;

public class PlatformS : MonoBehaviour {

	public static string GetPlatform(){

		if (Application.platform == RuntimePlatform.WindowsEditor ||
		    Application.platform == RuntimePlatform.WindowsPlayer ||
		    Application.platform == RuntimePlatform.WindowsWebPlayer){
			return ("PC");
		}

		else if (Application.platform == RuntimePlatform.OSXEditor ||
		         Application.platform == RuntimePlatform.OSXPlayer ||
		         Application.platform == RuntimePlatform.OSXWebPlayer){
			return ("Mac");
		}
		else{
			return ("Linux");
		}

	}
}
