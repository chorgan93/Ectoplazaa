using UnityEngine;
using System.Collections;

public class StartMovementS : MonoBehaviour {

	public float yDistanceToTravel;

	private float startY;
	private float endY;

	private float yDifference;

	public float travelTimeMax;
	private float travelTime;

	private bool stopMoving = false;

	// Use this for initialization
	void Start () {

		endY = transform.position.y;
		startY = transform.position.y - yDistanceToTravel;

		yDifference = endY - startY;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (!stopMoving){

			float t = travelTime/travelTimeMax;

			float mult = Mathf.Sin(t * Mathf.PI * 0.5f);

			float currentY = startY + yDifference*mult;

			transform.position = new Vector3(transform.position.x, currentY, transform.position.z);

			travelTime += Time.deltaTime;
			if (travelTime >= travelTimeMax){
				stopMoving = true;
			}

		}
	
	}
}
