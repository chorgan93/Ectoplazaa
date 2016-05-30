using UnityEngine;
using System.Collections;

public class ConfettiSpewer : MonoBehaviour {

	public int numToSpawn = 3;
	public float xLocalRange = 20f;
	private float yLocalPos = 20f;

	public float spawnRate = 0.08f;
	private float currentSpawnRate;

	public GameObject confettiObj;
	private GameObject poi;

	// Use this for initialization
	void Start () {

		currentSpawnRate
			= spawnRate;

	}
	
	// Update is called once per frame
	void Update () {

		if (ScoreKeeperS.gameEnd){

		currentSpawnRate -= Time.unscaledDeltaTime;



		if (currentSpawnRate <= 0){

				poi = CameraFollowS.F.focusCharacter;
				if (poi == null){
					poi = CameraFollowS.F.poi;
				}

			currentSpawnRate = spawnRate;

			Vector3 spawnPos = new Vector3(0, yLocalPos, -1f);

			for (int i = 0; i < numToSpawn; i++){

				spawnPos.x = Random.Range(-xLocalRange, xLocalRange);

				Instantiate(confettiObj, spawnPos+poi.transform.position, Quaternion.identity);

			}

		}
		}
	
	}
}
