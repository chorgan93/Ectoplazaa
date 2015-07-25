using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	string platformType; 
	int playerNum = 1; 

	bool started = false; 

	float inputDelay = 2f;

	// the following string is just for playtest
	// will need to connect all of start menu in final version
	public string nextSceneString;

	public GameObject [] postcards; 

	void Start () 
	{
		platformType = PlatformS.GetPlatform (); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!started) 
		{
			if (Input.GetButton ("AButtonPlayer" + playerNum + platformType) || Input.GetKey (KeyCode.KeypadEnter)) 
			{
				started = true;
				foreach (GameObject postcard in postcards) 
				{

					postcard.GetComponent<Rigidbody> ().AddForce (Vector3.right * Random.Range(5000f,10000f)); 
					postcard.GetComponent<Rigidbody> ().AddTorque (Vector3.forward * Random.Range(100000f,200000f)); 
				}
			}
		}
		else{
			inputDelay -= Time.deltaTime;
			if (inputDelay <= 0){
				if (Input.GetButton ("AButtonPlayer" + playerNum + platformType) || Input.GetKey (KeyCode.KeypadEnter)) 
				{
					Application.LoadLevel(nextSceneString);
				}
			}
		}

	}
}
