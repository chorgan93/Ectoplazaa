using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundDetectS : MonoBehaviour {

	public List<GameObject> groundObjs;


	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Player"){
			groundObjs.Add(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Player"){
			groundObjs.Remove(other.gameObject);
		}
	}

	public bool Grounded(){
		if (groundObjs.Count > 0){
			return true;
		}
		else{
			return false;
		}
	}

}
