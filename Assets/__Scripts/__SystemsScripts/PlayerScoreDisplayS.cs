using UnityEngine;
using System.Collections;

public class PlayerScoreDisplayS : MonoBehaviour {

	// for early debug purposes, no longer in use
	// displays player score through simple attached text

	public PlayerS playerRef;

	// fkasjdlksjald

	private TextMesh ownText;

	void Start () {

		ownText = GetComponent<TextMesh>();

	}
	
	// Update is called once per frame
	void Update () {

		ownText.text = "P" + playerRef.playerNum + ": " + playerRef.score + "";
	
	}
}
