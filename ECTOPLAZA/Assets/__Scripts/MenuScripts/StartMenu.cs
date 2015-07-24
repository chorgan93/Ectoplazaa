using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	string platformType; 
	int playerNum = 1; 

	bool started = false; 
	// Use this for initialization

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
			if (Input.GetButtonDown ("AButtonPlayer" + playerNum + platformType) || Input.GetKey (KeyCode.KeypadEnter)) 
			{
				started = true;
				foreach (GameObject postcard in postcards) 
				{

					postcard.GetComponent<Rigidbody> ().AddForce (Vector3.right * Random.Range(5000f,10000f)); 
					postcard.GetComponent<Rigidbody> ().AddTorque (Vector3.forward * Random.Range(100000f,200000f)); 
				}
			}
		}

	}
}
