using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailHandlerRedubS : MonoBehaviour {

	public PlayerS playerRef;
	public GameObject buttObj;
	public GameObject buttSprite; 
	public Transform buttAnchor; 
	public GameObject headSprite; 
	private Rigidbody playerRigid;
	private Rigidbody buttRigid;

	public GameObject deathParticles; 

	public Transform startPoint;
	public Transform endPoint;

	public bool separated = false;

	public float buttDelayCountdown;

	public GameObject dotPrefab;
	public List<GameObject> spawnedDots;

	int fastLength=150,medLength=25, slowLength=10, minLength=5; 

	Vector3 lastLocation, currentLocation; 

	//float newDotCounter, newDotSpawnRate = 5f; //original spawn rate for tail

	//public LineRenderer bodyConnector;

	private Vector3 currentButtVel;



	// Use this for initialization
	void Start () {

		buttRigid = buttObj.GetComponent<Rigidbody>();
		playerRigid = playerRef.GetComponent<Rigidbody>();

		buttRigid.transform.position = playerRef.transform.position;

		//bodyConnector.material = playerRef.GetComponent<Renderer>().material;
		if (playerRef.characterNum != 0)
			SetDotMaterial ();

		lastLocation = playerRef.transform.position; 
		currentLocation = playerRef.transform.position; 
	}


	// Update is called once per frame
	void FixedUpdate () {

		if (!playerRef.respawning) {
			//PlaceDots (); 
			PlaceDots (); 
			RemoveDots ();
			UpdateTail ();
		}

		//print (playerRigid.velocity.magnitude); 
	}


	void PlaceDots()
	{
		if (playerRef.playerNum == 1) {
			lastLocation = currentLocation;
			currentLocation = playerRef.transform.position; 
			float posDistance = Vector3.Distance (lastLocation, currentLocation); 
			posDistance = Mathf.RoundToInt(posDistance); 


			int newDotNumber = (int)posDistance;
			//print ("Distance: " + posDistance); 
			//print ("newDots: " + newDotNumber); 

			for( int i = 0; i < newDotNumber; i++)
			{
				Vector3 newSpawn = Vector3.Lerp (lastLocation, currentLocation, (float)i/newDotNumber);
				GameObject newDot = Instantiate(dotPrefab,newSpawn,headSprite.transform.rotation) as GameObject; 
				newDot.GetComponent<Renderer>().material = playerRef.playerMats[playerRef.characterNum -1] ; 
				newDot.GetComponent<DotColliderS>().whoCreatedMe = playerRef; 
				spawnedDots.Add(newDot); 
			}

			//spawn one dot anyway

			GameObject newNewDot = Instantiate(dotPrefab,playerRef.transform.position,headSprite.transform.rotation) as GameObject; 
			newNewDot.GetComponent<Renderer>().material = playerRef.playerMats[playerRef.characterNum -1] ; 
			newNewDot.GetComponent<DotColliderS>().whoCreatedMe = playerRef; 
			spawnedDots.Add(newNewDot); 

			//print (playerVel); 

			//newDotCounter -= 1; 
			//newDotCounter -= playerVel / maxVel;  
		
			//newDotCounter = newDotSpawnRate;

			/*
		GameObject newDot = Instantiate(dotPrefab,playerRef.transform.position,playerRef.transform.rotation) as GameObject; 
		newDot.GetComponent<Renderer>().material = playerRef.playerMats[playerRef.characterNum -1] ; 
		newDot.GetComponent<DotColliderS>().whoCreatedMe = playerRef; 
		spawnedDots.Add(newDot); 
		*/
		}
	}


	void UpdateTail()
	{
		Vector3 newPos;
		Quaternion newRot; 

		if (spawnedDots.Count > 0) {
			newPos = spawnedDots [0].transform.position;
			newRot = spawnedDots [0].transform.rotation;
			newRot = Quaternion.Euler (newRot.eulerAngles.x, newRot.eulerAngles.y, newRot.eulerAngles.z + 180f);
		} else {
			newPos = buttAnchor.position;
			newRot = headSprite.transform.rotation;
			newRot = Quaternion.Euler (newRot.eulerAngles.x, newRot.eulerAngles.y, newRot.eulerAngles.z + 180f);
		}

		buttSprite.transform.position = newPos; 
		buttSprite.transform.rotation = newRot; 


	}

	void RemoveDots()
	{
		if (spawnedDots.Count > 0) {
			if (spawnedDots.Count > minLength) {
				DestroyDot();
			}
					
			if (playerRigid.velocity.magnitude > 200 && spawnedDots.Count > fastLength) {

				int startingCount = spawnedDots.Count;

				for (int i = 0; i < (startingCount - fastLength); i++) {
					DestroyDot(); 
				}

			}
			else if(playerRigid.velocity.magnitude > 100 && spawnedDots.Count > medLength)
			{
				int startingCount = spawnedDots.Count;
				
				for (int i = 0; i < (startingCount - medLength); i++) {
					DestroyDot(); 
				}
			}
			else if(playerRigid.velocity.magnitude > 25 && spawnedDots.Count > slowLength)
			{
				int startingCount = spawnedDots.Count;
				
				for (int i = 0; i < (startingCount - slowLength); i++) {
					DestroyDot(); 
				}
			}

			if (playerRigid.velocity.magnitude < 5) {

				DestroyDot(); 
			}
		}

	}

	void DestroyDot()
	{
		
		GameObject.Destroy (spawnedDots [0].gameObject);
		spawnedDots.RemoveAt (0);
	}

	public void SetDotMaterial()
	{


		if (playerRef.characterNum > 0) {
			foreach (GameObject d in spawnedDots) {
				d.GetComponent<Renderer> ().material = playerRef.playerMats [playerRef.characterNum - 1];
			}
		}
			//playerRef.characterNum
	}

	public void DestroyPlayerDots()
	{


		while(spawnedDots.Count > 0)
		{

			Vector3 spawnPos = spawnedDots[0].transform.position;

			
			GameObject.Destroy (spawnedDots [0].gameObject);
			spawnedDots.RemoveAt (0);


			GameObject newParticles = Instantiate (deathParticles,spawnPos,Quaternion.identity) as GameObject;
			newParticles.GetComponent<ParticleSystemRenderer>().material = playerRef.playerParticleMats [playerRef.characterNum - 1];

		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white; 
		Gizmos.DrawWireSphere (currentLocation, .25f);
		Gizmos.DrawWireSphere (lastLocation, .25f);
		Gizmos.DrawLine(currentLocation,lastLocation);
		Vector3 newPos = Vector3.Lerp (currentLocation, lastLocation, 0.5f);  
		Gizmos.color = Color.cyan; 

		Gizmos.DrawWireSphere (newPos, .5f); 
		//print (newPos.x + " " + newPos.y); 
	}

}
