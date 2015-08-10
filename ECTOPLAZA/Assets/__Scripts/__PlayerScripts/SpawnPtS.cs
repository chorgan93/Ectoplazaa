using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPtS : MonoBehaviour {

	public List<GameObject> playersInRange ;

	void LateUpdate(){
		if (playersInRange.Count > 0){
			for (int i = 0; i < playersInRange.Count; i++){

				if ((playersInRange[i] == null) || (playersInRange[i] != null &&
				                                    playersInRange[i].GetComponent<PlayerS>().health <= 0)){
					playersInRange.RemoveAt(i);
				}

			}
		}
	}

	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "Player"){
			playersInRange.Add (other.gameObject);
		}

	}

	void OnTriggerExit(Collider other){
		
		if (other.gameObject.tag == "Player"){
			playersInRange.Remove (other.gameObject);
		}
		
	}

	public bool playerInRange(){
		if (playersInRange.Count > 0){
			return(true);
		}
		else{
			return(false);
		}
	}
}
