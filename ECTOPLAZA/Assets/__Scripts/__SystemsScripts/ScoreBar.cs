using UnityEngine;
using System.Collections;

public class ScoreBar : MonoBehaviour {



	//set these vars on spawn
	public int playerNum; 
	public int scoreThreshold; 
	public float scoreNumber; //scoreboard position, which scoreboard it is 

	public GameObject headObj, barObj, barOutlineObj; 

	public GameObject startTransform, endTransform; 

	Vector3 fullScale = new Vector3 (60f, 1f, .5f); 

	float animSpeed = 0.1f; 
	float scaleSpeed = 0.1f; 


	PlayerS playerRef; 

	void Start()
	{
		Update (); 
	}

	// Update is called once per frame
	void Update () 
	{
		GameObject [] playerGO = GameObject.FindGameObjectsWithTag ("Player"); 

		foreach (GameObject player in playerGO) {

			if(player.GetComponent<PlayerS>().playerNum == playerNum)
			{
				playerRef = player.GetComponent<PlayerS>(); 
				UpdateScoreboard(); 
			}
		}
	}

	void UpdateScoreboard()
	{
		startTransform.GetComponent<Renderer> ().material = playerRef.playerMats [playerRef.playerNum - 1];
		headObj.GetComponentInChildren<SpriteRenderer> ().sprite = playerRef.spriteObject.GetComponent<SpriteRenderer>().sprite;
		barObj.GetComponentInChildren<Renderer> ().material = playerRef.playerMats [playerRef.playerNum - 1];


		float health = Mathf.Clamp (playerRef.health, 0f, (float)scoreThreshold); 

		float lerpVal =  health/ (float)scoreThreshold;
		//print ("LerpVal = " + lerpVal); 

		Vector3 newHeadPos = Vector3.Lerp (startTransform.transform.position, endTransform.transform.position, lerpVal);
		Vector3 headAnim = Vector3.Lerp (headObj.transform.position, newHeadPos, animSpeed); 

		headObj.transform.position = headAnim; 

		Vector3 newBarScale = new Vector3 (0f, barObj.transform.localScale.y, barObj.transform.localScale.z) ; 

		newBarScale.x =  lerpVal; 

		Vector3 scaleAnim = Vector3.Lerp ( barObj.transform.localScale, newBarScale, scaleSpeed); 
		barObj.transform.localScale = scaleAnim;

		newBarScale.y = barOutlineObj.transform.localScale.y; 
		newBarScale.z = barOutlineObj.transform.localScale.z; 

		scaleAnim = Vector3.Lerp(barOutlineObj.transform.localScale,  newBarScale, scaleSpeed); 

		//barOutlineObj.transform.localScale = scaleAnim; 
	}




}
