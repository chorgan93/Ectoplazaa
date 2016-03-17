using UnityEngine;
using System.Collections;

#if UNITY_WIIU
using WiiU = UnityEngine.WiiU;
#endif

public class WiiUControllerManagerS : MonoBehaviour {
	
	// for non-gamepad (player 2+) players
	
	
	
	public int myChannel;
	
	public bool flingButtonDown = false;
	public bool dashButtonDown = false;
	public bool jumpButtonDown = false;
	public bool specialButtonDown = false;
	
	public bool pauseButtonDown = false;
	
	public float horizontalAxis = 0;
	public float verticalAxis = 0;

	private float spitUpdateMax = 10;
	private float spitUpdateCountdown;
	
	
	
	#if UNITY_WIIU
	
	// Use this for initialization
	void Start () {
		
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		// check that remote is connected
		WiiU.Remote rem = WiiU.Remote.Access(myChannel);
		
		WiiU.RemoteState state = rem.state;
		
		spitUpdateCountdown -= Time.deltaTime;

		switch (state.devType){
			
			// IF WII REMOTE & nunchuk
			
		case WiiU.RemoteDevType.Nunchuk:
			
			
			specialButtonDown = state.IsTriggered(WiiU.RemoteButton.B);
			dashButtonDown = state.IsTriggered(WiiU.RemoteButton.A);
			
			flingButtonDown = state.IsTriggered(WiiU.RemoteButton.NunchukZ);
			jumpButtonDown = state.IsTriggered(WiiU.RemoteButton.NunchukC);
			
			if (state.IsTriggered(WiiU.RemoteButton.Minus) ||
			    state.IsTriggered(WiiU.RemoteButton.Plus)){
				pauseButtonDown = true;
			}
			else{
				pauseButtonDown = false;
			}
			
			horizontalAxis = state.nunchuk.stick.x;
			verticalAxis = state.nunchuk.stick.y;


			//Debug.Log(myChannel + " : " + state.devType);

			
			
			break;
			
			// IF MOTION PLUS & nunchuk
			
		case WiiU.RemoteDevType.MotionPlusNunchuk:
			
			
			specialButtonDown = state.IsTriggered(WiiU.RemoteButton.B);
			dashButtonDown = state.IsTriggered(WiiU.RemoteButton.A);
			
			flingButtonDown = state.IsTriggered(WiiU.RemoteButton.NunchukZ);
			jumpButtonDown = state.IsTriggered(WiiU.RemoteButton.NunchukC);
			
			if (state.IsTriggered(WiiU.RemoteButton.Minus) ||
			    state.IsTriggered(WiiU.RemoteButton.Plus)){
				pauseButtonDown = true;
			}
			else{
				pauseButtonDown = false;
			}
			
			horizontalAxis = state.nunchuk.stick.x;
			verticalAxis = state.nunchuk.stick.y;

			
			//Debug.Log(myChannel + " : " + state.devType);
			
			
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

			
			//Debug.Log(myChannel + " : " + state.devType);
			
			break;

			case WiiU.RemoteDevType.Classic:
			
				if (state.classic.IsTriggered(WiiU.ClassicButton.B) || 
			    	state.classic.IsTriggered(WiiU.ClassicButton.R) ||
			    	state.classic.IsTriggered(WiiU.ClassicButton.ZR)){
					flingButtonDown = true;
				}
				else{
					flingButtonDown = false;
				}
				
				if (state.classic.IsTriggered(WiiU.ClassicButton.Y) || 
				    state.classic.IsTriggered(WiiU.ClassicButton.L) ||
			    	state.classic.IsTriggered(WiiU.ClassicButton.ZL)){
					dashButtonDown = true;
				}
				else{
					dashButtonDown = false;
				}
				
				specialButtonDown = state.classic.IsTriggered(WiiU.ClassicButton.X);
				jumpButtonDown = state.classic.IsTriggered(WiiU.ClassicButton.B);
				pauseButtonDown = state.classic.IsTriggered(WiiU.ClassicButton.Plus);
				
				horizontalAxis = state.classic.leftStick.x;
				verticalAxis = state.classic.leftStick.y;

			
				//Debug.Log(myChannel + " : " + state.devType);
				
				break;

			case WiiU.RemoteDevType.MotionPlusClassic:
			
				if (state.classic.IsTriggered(WiiU.ClassicButton.B) || 
				    state.classic.IsTriggered(WiiU.ClassicButton.R) ||
				    state.classic.IsTriggered(WiiU.ClassicButton.ZR)){
					flingButtonDown = true;
				}
				else{
					flingButtonDown = false;
				}
				
				if (state.classic.IsTriggered(WiiU.ClassicButton.Y) || 
				    state.classic.IsTriggered(WiiU.ClassicButton.L) ||
				    state.classic.IsTriggered(WiiU.ClassicButton.ZL)){
					dashButtonDown = true;
				}
				else{
					dashButtonDown = false;
				}
			
				specialButtonDown = state.classic.IsTriggered(WiiU.ClassicButton.X);
				jumpButtonDown = state.classic.IsTriggered(WiiU.ClassicButton.B);
				pauseButtonDown = state.classic.IsTriggered(WiiU.ClassicButton.Plus);
				
				horizontalAxis = state.classic.leftStick.x;
				verticalAxis = state.classic.leftStick.y;

			
				//Debug.Log(myChannel + " : " + state.devType);
				
				break;
			
			
		default:
			// error or not connected, set to default
			SetAllToDefault();
			break;
			
		}

		if (spitUpdateCountdown <= 0){
		
		
			Debug.Log("Channel " + myChannel + " REPORT:" + "\nflingButtonDown: " + flingButtonDown
			          + "\ndashButtonDown: " + dashButtonDown + "\njumpButtonDown: " + jumpButtonDown
			          "\nspecialButtonDown: " + specialButtonDown "\npauseButtonDown: " + pauseButtonDown);

			spitUpdateCountdown = spitUpdateMax;

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
