using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundDetectS : MonoBehaviour {

	public List<GameObject> groundObjs;

	void LateUpdate(){

		if (groundObjs.Count > 0){
			for (int i = 0; i < groundObjs.Count; i++){

				if (!groundObjs[i].activeSelf){
					groundObjs.RemoveAt(i);
				}

			}
		}

	}


	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Ground"){
			groundObjs.Add(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Ground"){
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
