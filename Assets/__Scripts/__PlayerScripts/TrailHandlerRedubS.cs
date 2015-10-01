using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailHandlerRedubS : MonoBehaviour {

	// script that was originally for handling tail position, though that function is out of use
	// remnants remain, feel free to ignore
	// now handles tail length and color based on game need

	public PlayerS playerRef;
	public GameObject buttObj;
	public GameObject buttSprite; 
	public Transform buttAnchor; 
	public GameObject headSprite; 
	private Rigidbody playerRigid;
	private Rigidbody buttRigid;
	public GameObject playerGlob; 

	public GameObject trailRendererGO; // for main color
	public GameObject trailRendererGO2; // for green highlight

	private TrailRenderer trailRendererMain; 
	private TrailRenderer trailRendererGlow; 

	float trailRenderTime, trailRenderTimeMin = 0.2f, trailRenderTimeMax= 1f; // handle tail length

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

	int dotsToBlowUpInto = 2; // num ecto each tail length will spawn on death

	//float newDotCounter, newDotSpawnRate = 5f; //original spawn rate for tail

	//public LineRenderer bodyConnector;

	private Vector3 currentButtVel;

	private float invulnAlpha = 0.5f;



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

		// spawn tail lengths based on initial health
		InitialDotSpawn (); 

		trailRendererMain = trailRendererGO.GetComponent<TrailRenderer>();
		trailRendererGlow = trailRendererGO2.GetComponent<TrailRenderer>();
	}


	// Update is called once per frame
	void FixedUpdate () {


		if (!playerRef.respawning) {

			if(lastRespawnVal == true)
			{
				// make sure to do initial dot spawn on respawn
				InitialDotSpawn(); 
			}

			//PlaceDots (); 
			PlaceDots ();  // spawns new dots and replaces old ones
			RemoveDots (); // removes old dots at end of tail
			UpdateTail (); // handles trail renderer
		} else {
			lastLocation = playerRef.transform.position;
			currentLocation = playerRef.transform.position; 


		}
		
		lastRespawnVal = playerRef.respawning; 

		//print (playerRigid.velocity.magnitude); 

		//DEBUG
		/*
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
			
		}*/

		ManageAlpha();
	}

	void ManageAlpha () {

		// make sure trails are appropriate transparency based on vulnerability
		if (playerRef.respawnInvulnTime > 0){
			Color mainCol = trailRendererMain.materials[0].color;
			Color greenCol = trailRendererGlow.materials[0].color;

			mainCol.a = greenCol.a = invulnAlpha;

			trailRendererMain.materials[0].color = mainCol;
			trailRendererGlow.materials[0].color = greenCol;
		}
		else{
			Color mainCol = trailRendererMain.materials[0].color;
			Color greenCol = trailRendererGlow.materials[0].color;
			
			mainCol.a = greenCol.a = 1;
			
			trailRendererMain.materials[0].color = mainCol;
			trailRendererGlow.materials[0].color = greenCol;
		}

	}

	void InitialDotSpawn()
	{

		// spawns tail length based on player health

		//if(playerRef.playerNum == 1)
			//print ("InitialDotSpawn for P1");

		for (int i = 0; i < playerRef.health; i++) {

			if(spawnedDots.Count < playerRef.health)
			{
				GameObject newNewDot = Instantiate(dotPrefab,playerRef.transform.position,headSprite.transform.rotation) as GameObject; 
				newNewDot.GetComponent<Renderer>().material = playerRef.playerMats[playerRef.characterNum -1] ; 
				newNewDot.GetComponent<DotColliderS>().whoCreatedMe = playerRef;
				spawnedDots.Add(newNewDot); 
				//print ("spawned dot" + spawnedDots.Count);
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

		// used to destroy tail lengths (probably will become redundant due to ecto mode redesign)

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
			spawnedDots[i].transform.localScale  = Vector3.Lerp (originalScale, smallScale, 5f- (float)((float)i)/(float)(spawnedDots.Count-1));

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

		// anything relating to butt sprite is out of use at the moment
		buttSprite.transform.position = newPos; 
		buttSprite.transform.rotation = newRot; 

		
		//print (buttSprite.transform.position + " compare " + spawnedDots[0].transform.position);

		//TRAIL RENDERER UPDATE
		trailRendererMain.time = trailRenderTimeMin + (trailRenderTimeMax * ((float) (float)playerRef.health / (float)playerRef.maxHealth));
		trailRendererGlow.time = trailRenderTimeMin + (trailRenderTimeMax * ((float) (float)playerRef.health / (float)playerRef.maxHealth));




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
	}

	public void DestroyPlayerDots()
	{

		// destroy entire tail

		DestroyPlayerDotsRange (spawnedDots.Count - 1);


	}

	public void DestroyPlayerDotsRange(int startingIndex)
	{

		// destroy a portion of tail

		if (startingIndex < spawnedDots.Count){

			for (int i = startingIndex; i > 0; i--) {


				SpawnGlobs(spawnedDots[i].transform.position,1);
	
				GameObject newParticles = Instantiate (deathParticles,spawnedDots[i].transform.position,Quaternion.identity) as GameObject;
				newParticles.GetComponent<ParticleSystem>().startColor = playerRef.playerParticleMats[playerRef.characterNum - 1].GetColor("_TintColor");
				newParticles.GetComponent<Rigidbody>().velocity = playerRigid.velocity;


				DestroyDot(); 


			}
		}
	}

	public void SpawnGlobs(Vector3 spawnPos, int numberOfGlobs)
	{

		// spawn ecto orbs in ecto mode when tail/player is destroyed


		for (int i = 0; i < numberOfGlobs; i++) {
			
			
			GameObject newGlob = Instantiate (playerGlob, spawnPos, Quaternion.identity) as GameObject; 
			
			// set random new velocity
			Vector3 newGlobVel = Random.insideUnitSphere * 8000f * Time.deltaTime;
			newGlobVel.z = 0;
			newGlob.GetComponentInChildren<SpriteRenderer>().color = playerRef.playerParticleMats[playerRef.characterNum - 1].GetColor("_TintColor");
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
