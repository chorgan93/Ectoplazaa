using UnityEngine;
using System.Collections;

public class WiiUControllerManagerS : MonoBehaviour {

	// for non-gamepad (player 2+) players

	// only for in-game: character select will have its own (similar) class

	private int myChannel;

	private PlayerS playerRef;

	public bool flingButtonDown = false;
	public bool dashButtonDown = false;
	public bool jumpButtonDown = false;

	public bool pauseButtonDown = false;

	public float horizontalAxis = 0;
	public float verticalAxis = 0;



#if UNITY_WIIU

	// Use this for initialization
	void Start () {

		playerRef = GetComponent<PlayerS>();
		playerRef.AddWiiUInputManger(this);

		myChannel = playerRef.playerNum-2; // player 2's channel will be 0, 3 will be 1, etc.
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

#endif
}
