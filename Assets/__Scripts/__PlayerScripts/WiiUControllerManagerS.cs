﻿using UnityEngine;
using System.Collections;

#if UNITY_WIIU
using WiiU = UnityEngine.WiiU;
#endif

public class WiiUControllerManagerS : MonoBehaviour {

	// for non-gamepad (player 2+) players

	// only for in-game: character select will have its own (similar) class

	private int myChannel;

	private PlayerS playerRef;

	public bool flingButtonDown = false;
	public bool dashButtonDown = false;
	public bool jumpButtonDown = false;
	public bool specialButtonDown = false;

	public bool pauseButtonDown = false;

	public float horizontalAxis = 0;
	public float verticalAxis = 0;



#if UNITY_WIIU

	// Use this for initialization
	void Start () {

		playerRef = GetComponent<PlayerS>();
		playerRef.AddWiiUInputManger(this);

		if (playerRef.playerNum > 1){

			myChannel = playerRef.playerNum-2; // player 2's channel will be 0, 3 will be 1, etc.

		}
		else{
		
			myChannel = -1;
		
		}
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// check that remote is connected
		WiiU.Remote rem = WiiU.Remote.Access(myChannel);

		WiiU.RemoteState state = rem.state;

		switch (state.devType){
		
			// IF WII REMOTE & NUNCHUCK

			case WiiU.RemoteDevType.Nunchuk:


				flingButtonDown = state.IsTriggered(WiiU.RemoteButton.B);
			    jumpButtonDown = state.IsTriggered(WiiU.RemoteButton.A);
			
				dashButtonDown = state.IsTriggered(WiiU.RemoteButton.NunchukZ);
				specialButtonDown = state.IsTriggered(WiiU.RemoteButton.NunchukC);

				if (state.IsTriggered(WiiU.RemoteButton.Minus) ||
				    state.IsTriggered(WiiU.RemoteButton.Plus)){
					pauseButtonDown = true;
				}
				else{
					pauseButtonDown = false;
				}
			
				horizontalAxis = state.nunchuck.stick.x;
				verticalAxis = state.nunchuck.stick.y;


				break;

			// IF MOTION PLUS & NUNCHUCK

			case WiiU.RemoteDevType.MotionPlusNunchuk:
				
				
				flingButtonDown = state.IsTriggered(WiiU.RemoteButton.B);
				jumpButtonDown = state.IsTriggered(WiiU.RemoteButton.A);
				
				dashButtonDown = state.IsTriggered(WiiU.RemoteButton.NunchukZ);
				specialButtonDown = state.IsTriggered(WiiU.RemoteButton.NunchukC);
				
				if (state.IsTriggered(WiiU.RemoteButton.Minus) ||
				    state.IsTriggered(WiiU.RemoteButton.Plus)){
					pauseButtonDown = true;
				}
				else{
					pauseButtonDown = false;
				}
				
				horizontalAxis = state.nunchuck.stick.x;
				verticalAxis = state.nunchuck.stick.y;
			
			
				break;

			// IF PRO CONTROLLER

			case WiiU.RemoteDevType.ProController:

				if (state.pro.IsTriggered(WiiU.ProControllerButton.B) || 
			                          state.pro.IsTriggered(WiiU.ProControllerButton.R) ||
				                      state.pro.IsTriggered(WiiU.ProControllerButton.ZR)){
					flingButtonDown = true;
				}
				else{
					flingButtonDown = false;
				}

				if (state.pro.IsTriggered(WiiU.ProControllerButton.Y) || 
				    state.pro.IsTriggered(WiiU.ProControllerButton.L) ||
				    state.pro.IsTriggered(WiiU.ProControllerButton.ZL)){
					dashButtonDown = true;
				}
				else{
					dashButtonDown = false;
				}
			
				specialButtonDown = state.pro.IsTriggered(WiiU.ProControllerButton.X);
				jumpButtonDown = state.pro.IsTriggered(WiiU.ProControllerButton.B);
				pauseButtonDown = state.pro.IsTriggered(WiiU.ProControllerButton.Plus);

				horizontalAxis = state.pro.leftStick.x;
				verticalAxis = state.pro.leftStick.y;

				break;


			default:
				// error or not connected, set to default
				SetAllToDefault();
				break;

		}
	
	}


#endif

	private void SetAllToDefault(){

		bool flingButtonDown = false;
		bool dashButtonDown = false;
		bool jumpButtonDown = false;
		bool specialButtonDown = false;
		
		bool pauseButtonDown = false;
		
		float horizontalAxis = 0;
		float verticalAxis = 0;

	}
}
