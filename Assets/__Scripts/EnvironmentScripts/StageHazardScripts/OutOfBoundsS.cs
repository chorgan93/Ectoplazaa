using UnityEngine;
using System.Collections;

public class OutOfBoundsS : MonoBehaviour {

	public Vector3 moveBackToStageAmt;


	void OnTriggerEnter(Collider other){

		if (other.gameObject.tag == "Player"){

			other.transform.position += moveBackToStageAmt;

		}

	}

}
