﻿using UnityEngine;
using System.Collections;

public class ScoreBar : MonoBehaviour {



	//set these vars on spawn
	public int scoreThreshold; 
	public float scoreNumber; //scoreboard position, which scoreboard it is 

	bool isInitialized; 

	PlayerS [] playerRefs = new PlayerS[4];
	public GameObject [] barObjs= new GameObject[4]; 
	GameObject[] heads = new GameObject[4];

	int totalPlayers; 

	public GameObject barObj; 

	public GameObject startTransform, endTransform; 

	Vector3 fullScale = new Vector3 (60f, 1f, .5f); 

	float animSpeed = 0.1f; 
	float scaleSpeed = 0.1f; 

	public GameObject headPrefab;


	void Start()
	{
	
		//Update (); 
	}

	// Update is called once per frame
	void Update () 
	{

		if (isInitialized) 
		{
			UpdateScoreboard (); 
		}
	}

	public void SpawnScoreboard()
	{
		isInitialized = true; 

		GameObject [] existingPlayers = GameObject.FindGameObjectsWithTag ("Player"); 

		for(int i = 0; i < 4; i++)
		{
			if(i < existingPlayers.Length)
			{
				totalPlayers+=1; 
				playerRefs[i] = existingPlayers[i].GetComponent<PlayerS>(); 
				
				Vector3 spawnPos = barObjs[i].transform.position;
				spawnPos.z = -8f; 
				GameObject newHead = Instantiate(headPrefab, spawnPos, Quaternion.identity) as GameObject; 
				playerRefs[i].GetComponent<PlayerS>().SetSkin(); 
				newHead.GetComponentInChildren<SpriteRenderer>().sprite = playerRefs[i].spriteObject.GetComponent<SpriteRenderer>().sprite; 
				newHead.transform.parent = this.transform; 
				heads[i] = newHead; 
				if (barObjs[i].GetComponentInChildren<Renderer>() != null){
					barObjs[i].GetComponentInChildren<Renderer> ().material = playerRefs[i].playerMats [playerRefs[i].characterNum - 1];
				}

			}
			else
			{
				barObjs[i].SetActive(false); 
			}
		}

	}


	void UpdateScoreboard()
	{ 


		float mostHealth = -1f; 
		float leastHealth = scoreThreshold+1f; 

		for (int i = 0; i < totalPlayers; i++) {

			Vector3 resetBarPos = barObjs[i].transform.position; 
			resetBarPos.z = -8f; 
			barObjs[i].transform.position = resetBarPos; 

			Vector3 resetHeadPos = heads[i].transform.position; 
			resetHeadPos.z = -8f; 
			heads[i].transform.position = resetHeadPos; 

			//MOVE HEADS
			
			float health = Mathf.Clamp (playerRefs[i].score, 0f, (float)scoreThreshold); //make sure health doesnt exceed max
			
			float lerpVal = health / (float)scoreThreshold;
			//print ("LerpVal = " + lerpVal); 
			
			Vector3 newHeadPos = Vector3.Lerp (startTransform.transform.position, endTransform.transform.position, lerpVal);
			newHeadPos.z = heads[i].transform.position.z; 
			Vector3 headAnim = Vector3.Lerp (heads[i].transform.position, newHeadPos, animSpeed); 
			
			heads[i].transform.position = headAnim; 
			heads[i].GetComponentInChildren<SpriteRenderer>().sprite = playerRefs[i].spriteObject.GetComponent<SpriteRenderer>().sprite; 

			barObjs[i].GetComponentInChildren<Renderer> ().material = playerRefs[i].playerMats [playerRefs[i].characterNum - 1];
			//CHANGE BAR SCALE
			Vector3 newBarScale = new Vector3 (0f, barObjs[i].transform.localScale.y, barObjs[i].transform.localScale.z); 
			
			newBarScale.x = lerpVal; 
			
			Vector3 scaleAnim = Vector3.Lerp (barObjs[i].transform.localScale, newBarScale, scaleSpeed); 
			barObjs[i].transform.localScale = scaleAnim;

			//CHECK FOR LEADER AND CHANGE END CIRCLE COLOR 

			if(playerRefs[i].score >= mostHealth)
			{
				mostHealth = playerRefs[i].score; 
			}
			if (playerRefs[i].score <= leastHealth)
			{
				leastHealth = playerRefs[i].score;
				startTransform.GetComponent<Renderer> ().material = playerRefs[i].playerMats [playerRefs[i].characterNum - 1];
				
			}

			for(int j = 0; j < totalPlayers; j++)
			{
				if(playerRefs[i].score < playerRefs[j].score || ((playerRefs[i].score == playerRefs[j].score) && (i > j) ))
				{
					Vector3 newBarPos = barObjs[i].transform.position; 
					newBarPos.z -= .2f; 
					barObjs[i].transform.position = newBarPos; 

					Vector3 newNewHeadPos = heads[i].transform.position; 
					newNewHeadPos.z -=0.1f; 
					heads[i].transform.position = newNewHeadPos; 
				}
				if((playerRefs[i].score == playerRefs[j].score) && (i < j) )
				{
					Vector3 newNewHeadPos = heads[i].transform.position; 
					newNewHeadPos.x +=0.175f; 
					heads[i].transform.position = newNewHeadPos; 
				}
			}

		}



				
	}
	




}
