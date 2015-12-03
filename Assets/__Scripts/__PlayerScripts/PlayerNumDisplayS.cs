using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerNumDisplayS : MonoBehaviour {

	// script that handles player number dispay in game (P1, P2, etc. over character head)
	// sets appropriate color and text according to character and player num respectively

	private TextMesh ownText;
	public PlayerS playerRef;
	public PlayerAnimS playerAnimRef;

	private string thisIsMyString;

	public List<Material> colorsToPick;

	// Use this for initialization
	void Start () {

		ownText = GetComponent<TextMesh>();
		ownText.text = "P" + playerRef.playerNum;
		thisIsMyString = ownText.text;
		ownText.color = colorsToPick [playerAnimRef.myCharNum - 1].GetColor ("_TintColor");
	
	}

	void FixedUpdate () {

		// only display while player is alive
		if (playerRef.health > 0){

			ownText.color = colorsToPick [playerAnimRef.myCharNum - 1].GetColor ("_TintColor");
			ownText.text = thisIsMyString;

		}
		else{
			ownText.text = "";
		}

	}
}
