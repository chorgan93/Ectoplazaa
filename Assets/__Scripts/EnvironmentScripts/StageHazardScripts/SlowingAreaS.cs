using UnityEngine;
using System.Collections;

public class SlowingAreaS : MonoBehaviour {

	private PlatformSoundS mySoundObj;

	public Sprite[] jelloSprites;

	private Sprite defaultSprite;
	private SpriteRenderer myRender;

	private int currentSprite;
	public float animRate;
	private float animRateCountdown;

	private int blobsToSpawn = 5;
	private float blobSpeed = 1000f;

	private bool animating = true;

	public GameObject blobPrefab;

	void Start(){

		if (!CurrentModeS.allowHazards){
			gameObject.SetActive(false);

		}

		mySoundObj = GetComponent<PlatformSoundS>();

		myRender = GetComponent<SpriteRenderer>();
		defaultSprite = myRender.sprite;

	}

	void Update(){

		if (animating){

			animRateCountdown -= Time.deltaTime;
			if (animRateCountdown <= 0){
				animRateCountdown = animRate;
				currentSprite++;
				if (currentSprite > jelloSprites.Length-1){
					animating = false;
					myRender.sprite = defaultSprite;
				}
				else{
					myRender.sprite = jelloSprites[currentSprite];
				}
			}

		}

	}

	// Update is called once per frame
	void OnTriggerEnter (Collider other) {

		if (other.gameObject.GetComponent<PlayerS>()){
			PlayerS playerRef = other.gameObject.GetComponent<PlayerS>();

			if (!playerRef.GetSlowedState()){
				playerRef.TriggerSlow();
			}

			
			SpawnBlobs(playerRef.transform.position);

			StartAnimation();
			mySoundObj.PlayPlatformSounds();
		}
	
	}

	private void StartAnimation(){

		currentSprite = 0;
		myRender.sprite = jelloSprites[currentSprite];
		animRateCountdown = animRate;
		animating = true;


	}

	private void SpawnBlobs(Vector3 spawnPos){

		GameObject blobSpawn;

		Vector3 blobForce;

		for (int i = 0; i < blobsToSpawn; i++){

			blobSpawn = (GameObject)Instantiate(blobPrefab, spawnPos, Quaternion.Euler(new Vector3(0,0,Random.Range(0,360))));

			blobForce = Random.insideUnitSphere;
			blobForce.z = 0;
			blobForce *= blobSpeed;

			blobSpawn.GetComponent<Rigidbody>().AddForce(blobForce*Time.deltaTime, ForceMode.Impulse);

		}

	}

	// Update is called once per frame
	void OnTriggerExit (Collider other) {
		
		if (other.gameObject.GetComponent<PlayerS>()){
			PlayerS playerRef = other.gameObject.GetComponent<PlayerS>();
			
			if (playerRef.GetSlowedState()){
				playerRef.DisableSlow();
			}
			
			SpawnBlobs(playerRef.transform.position);

			StartAnimation();
			mySoundObj.PlayPlatformSounds();
		}
		
	}
}
