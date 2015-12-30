using UnityEngine;
using System.Collections;

public class GameEndTextS : MonoBehaviour {

	public GameObject newRoundText;

	public GameObject newGameText;

	// Use this for initialization
	void Start () {

		if (CurrentModeS.DoAnotherRound()){
			newRoundText.SetActive(true);
		}
		else{
			newGameText.SetActive(true);
		}
	
	}
}
