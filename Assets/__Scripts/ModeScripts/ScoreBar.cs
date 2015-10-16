using UnityEngine;
using System.Collections;

public class ScoreBar : MonoBehaviour {

	// manages white score bar ui at top of screen based on player score
	// currently only works with ecto mode, will need to be repurposed for multiple modes

	//set these vars on spawn
	public int scoreThreshold; 
	public float scoreNumber; //scoreboard position, which scoreboard it is 

	bool isInitialized; 

	public PlayerS [] playerRefs = new PlayerS[4];
	public GameObject [] barObjs= new GameObject[4]; 
	GameObject[] heads = new GameObject[4];
	GameObject[] ghostHeads = new GameObject[4]; // for use in ecto mode

	GameObject[] playerScores = new GameObject[4];

	int totalPlayers; 

	public GameObject barObj; 

	public GameObject startTransform, endTransform; 

	Vector3 fullScale = new Vector3 (60f, 1f, .5f); 

	float animSpeed = 0.1f; 
	float scaleSpeed = 0.1f; 

	public GameObject headPrefab;
	public GameObject ghostHeadPrefab; // for health count in ecto mode

	// this stuff is for stock mode, will need some revision to be better

	// four player positions
	public GameObject P4firstPlayerPos;
	public GameObject P4secondPlayerPos;
	public GameObject P4PthirdPlayerPos;
	public GameObject P4fourthPlayerPos;

	// three player positions
	public GameObject P3firstPlayerPos;
	public GameObject P3secondPlayerPos;
	public GameObject P3PthirdPlayerPos;

	// two player positions
	public GameObject P2firstPlayerPos;
	public GameObject P2secondPlayerPos;

	public GameObject stockModeDisplayPrefab;


	void Start()
	{
	
		//Update (); 
	}

	// Update is called once per frame
	void Update () 
	{

		if (isInitialized) 
		{
			if (CurrentModeS.currentMode == 0){
				UpdateScoreboard (); 
			}
		}
	}

	public void SpawnScoreboard()
	{
		isInitialized = true; 
		
		GameObject [] existingPlayers = GameObject.FindGameObjectsWithTag ("Player"); 

		// set player script references from existing players
		for (int j = 0; j < existingPlayers.Length; j++){

			if (existingPlayers[j].GetComponent<PlayerS>()){

				playerRefs[j] = existingPlayers[j].GetComponent<PlayerS>();
			}
		}

		for(int i = 0; i < 4; i++)
		{
			if(i < existingPlayers.Length)
			{
				totalPlayers+=1; 
				//print (existingPlayers[i]); 

				// ECTO MODE

				if (CurrentModeS.currentMode == 0){
				
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

					Vector3 spawnPosGhostHead = barObjs[i].transform.position;
					spawnPosGhostHead.z = -8f; 
					GameObject newGhostHead = Instantiate(ghostHeadPrefab, spawnPosGhostHead, Quaternion.identity) as GameObject; 
					//playerRefs[i].GetComponent<PlayerS>().SetSkin(); 
					newGhostHead.GetComponentInChildren<SpriteRenderer>().sprite = playerRefs[i].spriteObject.GetComponent<SpriteRenderer>().sprite; 
					Color ghostCol = newGhostHead.GetComponentInChildren<SpriteRenderer>().color;
					ghostCol.a = 0.5f;
					newGhostHead.GetComponentInChildren<SpriteRenderer>().color = ghostCol;
					newGhostHead.transform.parent = this.transform; 
					ghostHeads[i] = newGhostHead; 
				}

				// STOCK MODE

				if (CurrentModeS.currentMode == 1){

					print ("INSTANTIATE STOCK MODE OBJECTS");
					
					// spawn display objects
					
					barObjs[i].SetActive(false); 
					GameObject playerScore = Instantiate (stockModeDisplayPrefab, transform.position,Quaternion.identity)
						as GameObject;
					playerScores[i] = playerScore;
					playerScore.GetComponent<StockDisplayS>().myPlayer = playerRefs[i];



				}

			}
			else
			{
				barObjs[i].SetActive(false); 
			}
		}

		// now, if in stock mode, place player score positions
		if (CurrentModeS.currentMode == 1){
		for (int i = 0; i < totalPlayers; i++){
			if (totalPlayers == 4){
				if (i==0){
					playerScores[i].transform.position = P4firstPlayerPos.transform.position;
					playerScores[i].transform.parent = P4firstPlayerPos.transform.parent;
				}
				if (i==1){
					playerScores[i].transform.position = P4secondPlayerPos.transform.position;
					playerScores[i].transform.parent = P4secondPlayerPos.transform.parent;
				}
				if (i==2){
					playerScores[i].transform.position = P4PthirdPlayerPos.transform.position;
					
					playerScores[i].transform.parent = P4PthirdPlayerPos.transform.parent;
				}
				if (i==3){
					playerScores[i].transform.position = P4fourthPlayerPos.transform.position;
					
					playerScores[i].transform.parent = P4fourthPlayerPos.transform.parent;
				}
			}
			if (totalPlayers == 3){
				if (i==0){
					playerScores[i].transform.position = P3firstPlayerPos.transform.position;
					
					playerScores[i].transform.parent = P3firstPlayerPos.transform.parent;
				}
				if (i==1){
					playerScores[i].transform.position = P3secondPlayerPos.transform.position;
					
					playerScores[i].transform.parent = P3secondPlayerPos.transform.parent;
				}
				if (i==2){
					playerScores[i].transform.position = P3PthirdPlayerPos.transform.position;
					playerScores[i].transform.parent = P3PthirdPlayerPos.transform.parent;
				}
			}
			if (totalPlayers == 2){
				if (i==0){
					//print ("SET PARENT!");
					playerScores[i].transform.position = P2firstPlayerPos.transform.position;
					playerScores[i].transform.parent = P2secondPlayerPos.transform.parent;
				}
				if (i==1){
					playerScores[i].transform.position = P2secondPlayerPos.transform.position;
					playerScores[i].transform.parent = P2secondPlayerPos.transform.parent;
				}
			}
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

			// in ecto mode, repeat head move for ghost head if health
			if (CurrentModeS.currentMode == 0){
			float ghostHealth = Mathf.Clamp (playerRefs[i].health-playerRefs[i].startEctoNum, 0f, (float)scoreThreshold); //make sure health doesnt exceed max
			
			float lerpValGhost = ghostHealth / (float)scoreThreshold;
			//print ("LerpVal = " + lerpVal); 
			
			Vector3 newGhostHeadPos = Vector3.Lerp (startTransform.transform.position, endTransform.transform.position, lerpValGhost);
			newGhostHeadPos.z = ghostHeads[i].transform.position.z; 
			Vector3 ghostHeadAnim = Vector3.Lerp (ghostHeads[i].transform.position, newGhostHeadPos, animSpeed); 
			
			ghostHeads[i].transform.position = ghostHeadAnim; 
			ghostHeads[i].GetComponentInChildren<SpriteRenderer>().sprite = playerRefs[i].spriteObject.GetComponent<SpriteRenderer>().sprite; 
			}


			//CHANGE BAR SCALE
			Vector3 newBarScale = new Vector3 (0f, barObjs[i].transform.localScale.y, barObjs[i].transform.localScale.z); 
			
			newBarScale.x = lerpVal; 
			
			Vector3 scaleAnim = Vector3.Lerp (barObjs[i].transform.localScale, newBarScale, scaleSpeed); 
			barObjs[i].transform.localScale = scaleAnim;

			//CHECK FOR LEADER AND CHANGE END CIRCLE COLOR 
			// this will need to be changed per mode

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
	

	public void UpdateScoreboardStockMode(PlayerS p)
	{
		//do things?

		print("Updating Scoreboard for Stock Mode");
	}


}
