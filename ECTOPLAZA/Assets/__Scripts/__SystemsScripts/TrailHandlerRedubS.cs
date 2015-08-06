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
	public GameObject playerGlob; 

	public GameObject trailRendererGO;
	public GameObject trailRendererGO2;
	float trailRenderTime, trailRenderTimeMin = 0.05f, trailRenderTimeMax= .85f; 

	private LineRenderer playerLine; 

	public GameObject deathParticles; 

	public Transform startPoint;
	public Transform endPoint;

	public bool separated = false;

	Vector3 originalScale, smallScale = new Vector3(.5f,.5f,1f); 

	public GameObject dotPrefab;
	public List<GameObject> spawnedDots;

	int fastLength=150,medLength=75, slowLength=25, minLength=5; 

	Vector3 lastLocation, currentLocation; 

	bool lastRespawnVal = false; 

	int dotsToBlowUpInto = 2;

	//float newDotCounter, newDotSpawnRate = 5f; //original spawn rate for tail

	//public LineRenderer bodyConnector;

	private Vector3 currentButtVel;



	// Use this for initialization
	void Start () {

		originalScale = dotPrefab.transform.localScale; 

		buttRigid = buttObj.GetComponent<Rigidbody>();
		playerRigid = playerRef.GetComponent<Rigidbody>();

		buttRigid.transform.position = playerRef.transform.position;

		//buttSprite.transform.parent = null;

		//bodyConnector.material = playerRef.GetComponent<Renderer>().material;
		if (playerRef.characterNum != 0)
			SetDotMaterial ();

		playerLine = playerRef.GetComponent<LineRenderer> (); 
		lastLocation = playerRef.transform.position; 
		currentLocation = playerRef.transform.position; 

		InitialDotSpawn (); 
	}


	// Update is called once per frame
	void FixedUpdate () {


		if (!playerRef.respawning) {

			if(lastRespawnVal == true)
			{
				InitialDotSpawn(); 
			}

			//PlaceDots (); 
			PlaceDots (); 
			RemoveDots ();
			UpdateTail ();
		} else {
			lastLocation = playerRef.transform.position;
			currentLocation = playerRef.transform.position; 


		}
		
		lastRespawnVal = playerRef.respawning; 

		//print (playerRigid.velocity.magnitude); 

		//DEBUG
		if (Input.GetKey (KeyCode.Alpha1)) {

			playerRef.health = 10; 

		}
		if (Input.GetKey (KeyCode.Alpha2)) {
			
			playerRef.health = 20; 
			
		}
		if (Input.GetKey (KeyCode.Alpha3)) {
			
			playerRef.health = 30; 
			
		}
		if (Input.GetKey (KeyCode.Alpha4)) {
			
			playerRef.health = 40; 
			
		}
		if (Input.GetKey (KeyCode.Alpha5)) {
			
			playerRef.health = 50; 
			
		}
	}

	void InitialDotSpawn()
	{
		//if(playerRef.playerNum == 1)
			//print ("InitialDotSpawn for P1");

		for (int i = 0; i < playerRef.health; i++) {

			if(spawnedDots.Count < playerRef.health)
			{
				GameObject newNewDot = Instantiate(dotPrefab,playerRef.transform.position,headSprite.transform.rotation) as GameObject; 
				newNewDot.GetComponent<Renderer>().material = playerRef.playerMats[playerRef.characterNum -1] ; 
				newNewDot.GetComponent<DotColliderS>().whoCreatedMe = playerRef;
				spawnedDots.Add(newNewDot); 
				print ("spawned dot" + spawnedDots.Count);
			}
			else
			{
				break;
			}

		}

	}


	void PlaceDots()
	{


		lastLocation = currentLocation;
		currentLocation = playerRef.transform.position; 
		float posDistance = Vector3.Distance (lastLocation, currentLocation); 
		posDistance = Mathf.RoundToInt(posDistance); 


		int newDotNumber = (int)posDistance;
		//print ("Distance: " + posDistance); 
		//print ("newDots: " + newDotNumber); 



		//PLACE DOTS LERPED ALONG THE LAST 2 PLAYER POSITIONS
		for( int i = 0; i < newDotNumber; i++)
		{
			Vector3 newSpawn = Vector3.Lerp (lastLocation, currentLocation, (float)i/newDotNumber);
			GameObject newDot = Instantiate(dotPrefab,newSpawn,headSprite.transform.rotation) as GameObject; 
			newDot.GetComponent<Renderer>().material = playerRef.playerMats[playerRef.characterNum -1] ; 
			newDot.GetComponent<DotColliderS>().whoCreatedMe = playerRef; 
			spawnedDots.Add(newDot); 

			if(spawnedDots.Count > playerRef.health) //keep placing new dots, start deleting old ones
			{
				DestroyDot();
			}
		

		}

		if(newDotNumber == 0 && spawnedDots.Count < playerRef.health)
		{
			//spawn one dot anyway
			GameObject newNewDot = Instantiate(dotPrefab,playerRef.transform.position,headSprite.transform.rotation) as GameObject; 
			newNewDot.GetComponent<Renderer>().material = playerRef.playerMats[playerRef.characterNum -1] ; 
			newNewDot.GetComponent<DotColliderS>().whoCreatedMe = playerRef; 
			spawnedDots.Add(newNewDot); 


		}

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

	public void ChopTail(GameObject dotHit)
	{
		int goID = dotHit.GetInstanceID ();
		int startingIndex = 0; 

		for (int i = 0; i < spawnedDots.Count; i++) {
			if(spawnedDots[i].GetInstanceID() == goID)
			{
				startingIndex = i;
				break;
			}
		}

		playerRef.TakeDamage (startingIndex); 

		DestroyPlayerDotsRange (startingIndex);

	}

	void UpdateTail()
	{
		Vector3 newPos;
		Quaternion newRot; 

		//SCALE DOTS DOWN AS THEY GO DOWN THE LINE
		for (int i = 0; i < spawnedDots.Count -1; i++) {

			//print(originalScale.x+ " " + originalScale.y + " " +originalScale.z);
			//print((float)((float)i)/(float)(spawnedDots.Count-1));
			spawnedDots[i].transform.localScale  = Vector3.Lerp (originalScale, smallScale, 1f- (float)((float)i)/(float)(spawnedDots.Count-1));

		}

		//ROTATE THE FINAL BUTT TO WHAT THE HEAD WAS ROTATED TO, 
		if (spawnedDots.Count > 0) {
			newPos = spawnedDots [0].transform.position;
			newRot = spawnedDots [0].transform.rotation;
			newRot = Quaternion.Euler (newRot.eulerAngles.x, newRot.eulerAngles.y, newRot.eulerAngles.z);
		} else {
			newPos = buttAnchor.position;
			newRot = headSprite.transform.rotation;
			newRot = Quaternion.Euler (newRot.eulerAngles.x, newRot.eulerAngles.y, newRot.eulerAngles.z);
		}

		buttSprite.transform.position = newPos; 
		buttSprite.transform.rotation = newRot; 

		
		//print (buttSprite.transform.position + " compare " + spawnedDots[0].transform.position);

		//TRAIL RENDERER UPDATE
		trailRendererGO.GetComponent<TrailRenderer> ().time = trailRenderTimeMin + (trailRenderTimeMax * ((float) (float)playerRef.health / (float)playerRef.maxHealth));
		trailRendererGO2.GetComponent<TrailRenderer> ().time = trailRenderTimeMin + (trailRenderTimeMax * ((float) (float)playerRef.health / (float)playerRef.maxHealth));


		//SET UP LINERENDERERS //NOT DISPLAYING RIGHT, LINE RENDERER TOO GLITCHY
		/*
		//make last dot invisible so trail can fade, no sphere
		if (spawnedDots.Count > 0) {

			playerLine.SetVertexCount(spawnedDots.Count); 

			//spawnedDots [0].GetComponent<Renderer> ().enabled = false; 
			trailRendererGO.transform.position = spawnedDots[0].transform.position; 

			for (int j = 0; j < spawnedDots.Count -1; j++) {

				playerLine.SetPosition(j, spawnedDots[j].transform.position); 

			}

			playerLine.SetPosition(spawnedDots.Count-1, headSprite.transform.position ); 


		}
		*/

	}

	void RemoveDots()
	{
		if (spawnedDots.Count > 0) {
			DestroyDot ();

		}

		if (spawnedDots.Count > 0 && spawnedDots.Count > playerRef.health) {

			int dotsDestroyNum = (int) spawnedDots.Count - (int) playerRef.health;

			for (int i = 0; i < dotsDestroyNum; i++) {
				DestroyDot(); 
			}

		}
		/*
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
		*/
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

		DestroyPlayerDotsRange (spawnedDots.Count - 1);

		/*
		while(spawnedDots.Count > 0)
		{

			Vector3 spawnPos = spawnedDots[0].transform.position;

			
			GameObject.Destroy (spawnedDots [0].gameObject);
			spawnedDots.RemoveAt (0);

			//SKETCHY CODE, REDOING WHAT DESTROYPLAYERDOTSRANGE IS DOING
			GameObject newGlob = Instantiate(playerGlob, spawnedDots[0].transform.position, Quaternion.identity) as GameObject; 
			newGlob.GetComponentInChildren<GlobS>().SetVelocityMaterial(spawnedDots[0].GetComponent<Rigidbody>().velocity, playerRef.gameObject); 


			GameObject newParticles = Instantiate (deathParticles,spawnPos,Quaternion.identity) as GameObject;
			newParticles.GetComponent<ParticleSystem>().startColor = playerRef.playerParticleMats[playerRef.characterNum - 1].GetColor("_TintColor");
			newParticles.GetComponent<Rigidbody>().velocity = playerRigid.velocity; 

		}
		*/


	}

	public void DestroyPlayerDotsRange(int startingIndex)
	{

		//print ("dot count " + spawnedDots.Count);
		//print ("num dots to destroy" + startingIndex);

		if (startingIndex < spawnedDots.Count){

			for (int i = startingIndex; i > 0; i--) {


				SpawnGlobs(spawnedDots[i].transform.position,1);
	
				GameObject newParticles = Instantiate (deathParticles,spawnedDots[i].transform.position,Quaternion.identity) as GameObject;
				newParticles.GetComponent<ParticleSystem>().startColor = playerRef.playerParticleMats[playerRef.characterNum - 1].GetColor("_TintColor");
				newParticles.GetComponent<Rigidbody>().velocity = playerRigid.velocity;


				DestroyDot(); 

				print ("dot destroyed!");

			}
		}
	}

	public void SpawnGlobs(Vector3 spawnPos, int numberOfGlobs)
	{
		for (int i = 0; i < numberOfGlobs; i++) {
			
			
			GameObject newGlob = Instantiate (playerGlob, spawnPos, Quaternion.identity) as GameObject; 
			
			// set random new velocity
			Vector3 newGlobVel = Random.insideUnitSphere * 6000f * Time.deltaTime;
			newGlobVel.z = 0;
			newGlob.GetComponentInChildren<GlobS> ().SetVelocityMaterial (newGlobVel, playerRef.gameObject); 
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

		for (int i = 0; i < spawnedDots.Count -1; i++) {

			Gizmos.DrawWireSphere (spawnedDots[i].transform.position, spawnedDots[i].transform.localScale.x/2f);
		}


	}

}
