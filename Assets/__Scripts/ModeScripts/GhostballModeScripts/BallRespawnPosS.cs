using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallRespawnPosS : MonoBehaviour {

	private List<PlayerS> playersTouching = new List<PlayerS>();

	void OnTriggerEnter(Collider other){

		if (other.GetComponent<PlayerS>()){
			playersTouching.Add(other.GetComponent<PlayerS>());
		}

	}

	void OnTriggerExit(Collider other){

		if (other.GetComponent<PlayerS>()){
			playersTouching.Remove(other.GetComponent<PlayerS>());
		}

	}

	public bool SpotIsFree(){

		return (playersTouching.Count <= 0);

	}
}
