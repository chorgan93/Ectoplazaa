using UnityEngine;
using System.Collections;

public class DotColliderS : MonoBehaviour {
	
	public PlayerS whoCreatedMe;
	
	void Start () {
		//GetComponent<Renderer>().material = whoCreatedMe.GetComponent<Renderer>().material;
	}
	
	/*void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player" && other.GetComponent<PlayerS>() != whoCreatedMe){
			if (other.GetComponent<PlayerS>().isDangerous){
				whoCreatedMe.TakeDamage(whoCreatedMe.health);
			}
		}
	}*/
}
