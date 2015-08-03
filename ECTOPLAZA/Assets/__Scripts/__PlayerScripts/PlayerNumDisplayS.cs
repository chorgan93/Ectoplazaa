using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerNumDisplayS : MonoBehaviour {

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

		if (playerRef.health > 0){

			ownText.text = thisIsMyString;

		}
		else{
			ownText.text = "";
		}

	}
}
