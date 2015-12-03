using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallDetectS : MonoBehaviour {
	
	public List<GameObject> wallObjs;
	
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Ground" ||
		    other.gameObject.tag == "Wall"){
			wallObjs.Add(other.gameObject);
		}
	}
	
	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Ground" ||
		    other.gameObject.tag == "Wall"){
			wallObjs.Remove(other.gameObject);
		}
	}
	
	public bool WallTouching(){
		if (wallObjs.Count > 0){
			return true;
		}
		else{
			return false;
		}
	}
	
}
