using UnityEngine;
using System.Collections;

public class PlayerScoreDisplayS : MonoBehaviour {

	public PlayerS playerRef;

	private TextMesh ownText;

	void Start () {

		ownText = GetComponent<TextMesh>();

	}
	
	// Update is called once per frame
	void Update () {

		ownText.text = "P" + playerRef.playerNum + ": " + playerRef.score + "";
	
	}
}
