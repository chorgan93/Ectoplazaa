﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailHandlerS : MonoBehaviour {

	public PlayerS playerRef;
	public GameObject buttObj;
	private Rigidbody playerRigid;
	private Rigidbody buttRigid;

	public bool separated = false;

	public float buttDelayCountdown;

	public GameObject dotPrefab;
	public List<GameObject> spawnedDots;

	public float timeBetweenDotsMax;
	private float dropTimeCountdown;
	private float nextFollowVelCountdown;

	public LineRenderer bodyConnector;

	private Vector3 currentButtVel;



	// Use this for initialization
	void Start () {

		buttRigid = buttObj.GetComponent<Rigidbody>();
		playerRigid = playerRef.GetComponent<Rigidbody>();

		//bodyConnector.material = playerRef.GetComponent<Renderer>().material;

	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (buttDelayCountdown > 0){
			if (!separated){
				separated = true;
				dropTimeCountdown = timeBetweenDotsMax;
			}
		}
	
		DropDots();
		FollowHead();

		LineHandler();

	}

	void LineHandler () {


		// two vertexes for head and butt and an additional for each joint
		int lineHandlerLength = 2+spawnedDots.Count;

		bodyConnector.SetVertexCount(lineHandlerLength);

		// set head and butt as first and last vertext
		bodyConnector.SetPosition(0,transform.position);
		bodyConnector.SetPosition(lineHandlerLength-1,buttObj.transform.position);

		// set joints as each other vertex
		if (spawnedDots.Count > 0){
			for (int i=0; i < spawnedDots.Count; i++){
				bodyConnector.SetPosition(i+1,spawnedDots[spawnedDots.Count-1-i].transform.position);
			}
		}
	}

	void DropDots () {

		if (separated){

			// only drop when player is moving
			if (playerRigid.velocity.x != 0 || playerRigid.velocity.y != 0){
				dropTimeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
			}
			if (dropTimeCountdown <= 0){
				dropTimeCountdown = timeBetweenDotsMax;

				GameObject newDot = Instantiate(dotPrefab,transform.position,Quaternion.identity)
					as GameObject;

				spawnedDots.Add(newDot);
			}

		}
		else{
			if (spawnedDots.Count > 0){
				for (int i = 0; i < spawnedDots.Count; i++){
					Destroy(spawnedDots[i]);
				}
				spawnedDots.Clear();
			}
		}

	}

	void FollowHead () {

		buttDelayCountdown -= Time.deltaTime*TimeManagerS.timeMult;

		if (buttDelayCountdown > 0){
			print (buttDelayCountdown);
			nextFollowVelCountdown = timeBetweenDotsMax;

			// make sure butt is not moving
			buttRigid.velocity = Vector3.zero;
		}

		if (buttDelayCountdown <= 0 && separated){
			// start following head if there's a point to reach

			nextFollowVelCountdown -= Time.deltaTime*TimeManagerS.timeMult;

			if (nextFollowVelCountdown <= 0){

				nextFollowVelCountdown = timeBetweenDotsMax;

				if (spawnedDots.Count > 1){
					Destroy(spawnedDots[0]);
					spawnedDots.RemoveAt(0);
				}
			
				if (spawnedDots.Count > 0){
	
					// set velocity
	
					currentButtVel = (spawnedDots[0].transform.position-buttObj.transform.position)/timeBetweenDotsMax;
					//print ("SetVel!");
				}
			}

			// set butt velocity

			buttRigid.velocity = currentButtVel;
			//print (buttRigid.velocity);
		}

	}

	public void SetButtDelay(float newDelay){

		buttDelayCountdown = newDelay;

	}
}