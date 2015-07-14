using UnityEngine;
using System.Collections;

public class TrailHandlerS : MonoBehaviour {

	public PlayerS playerRef;
	public GameObject buttObj;

	public float buttDelayCountdown;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetButtDelay(float newDelay){

		buttDelayCountdown = newDelay;

	}
}
