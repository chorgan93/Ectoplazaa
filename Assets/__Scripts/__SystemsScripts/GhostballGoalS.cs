using UnityEngine;
using System.Collections;

public class GhostballGoalS : MonoBehaviour {

	public  float		 	timeDelayUntilActive = 3,
							timePerPointTick = 1;
	ScoreKeeperS 			scoreKeeper;
	GameObject				ghostBall;
	bool 					bIsActivated = false,
							bBallDetected =false;
	float 					delayBirthday = 0,
							pointBirthday = 0;
	public int 				owningPlayerNumber;

	// Use this for initialization
	void Start () 
	{
		scoreKeeper = GameObject.FindObjectOfType<ScoreKeeperS>() as ScoreKeeperS;
		ghostBall = GameObject.FindGameObjectWithTag("Ghostball") as GameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (bBallDetected)							//If I have the ball...
		{
			 if(!bIsActivated)						//If activation time delay remains..
			{
				CheckDelayTimer();					//Countdown the delay!

			}
			else 									//Otherwise, if no more activation time delay
			{
				CheckPointTimer();					//Work on adding points!
			}
		}

	}

	void OnTriggerEnter(Collider c)
	{
		if (c == ghostBall.GetComponent<Collider>())
		{
			bBallDetected = true;			//Flip flag
			delayBirthday = Time.time;		//Start countdown
			print("ball enter"); 			//visual effect hook?
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if (c == ghostBall.GetComponent<Collider>())
		{
			bBallDetected = false;			//End countdown
			bIsActivated = false;			//No longer activated
			print("ball exit"); 			//visual effect hook?
		}
	}

	void CheckDelayTimer()
	{
		if(Time.time - delayBirthday > timeDelayUntilActive)
		{
			Activated();
		}
	}

	void Activated()
	{
		bIsActivated = true; 				//Flip flag
		pointBirthday = Time.time;			//Update point timer
		print("Activate!"); //Visual effect hook?
	}
	
	void CheckPointTimer()
	{
		if(Time.time - pointBirthday > timePerPointTick)
		{
			pointBirthday = Time.time;					//Update timer
			print("add points!");
			scoreKeeper.AddPoints(owningPlayerNumber);	//Talk to ScoreKeeperS;

		}
	}
}
