using UnityEngine;
using System.Collections;

public class PlatformStartEffectS : MonoBehaviour {

	private float delayEffectTime = 0.75f;

	private float startYPos;
	private float newMinYPos;

	private float newYConcierge = -22f;
	private float newYRoof = -22f;
	private float newYDiningHall = -35f;
	private float newYBeach = -35f;
	private float newYHell = -45f;

	private float maxLerpTime = 1.3f;
	private float currentLerpTime = 0; 

	private bool effectOn = false;

	private Rigidbody myRigid;
	private SpringJoint mySpring;
	private float sprintAmt = 0;

	// Use this for initialization
	void Start () {

		startYPos = transform.position.y;

		if (CurrentModeS.firstGame){
			effectOn = true;
			Vector3 setPos = transform.position;

			if (Application.loadedLevelName == "4Concierge"){
				setPos.y = newYConcierge;
			}
			else if (Application.loadedLevelName == "5DiningHall"){
				setPos.y = newYDiningHall;
			}
			else if (Application.loadedLevelName == "6Beach"){
				setPos.y = newYBeach;
			}
			else if (Application.loadedLevelName == "8Hell"){
				setPos.y = newYHell;
			}
			else if (Application.loadedLevelName == "9Roof"){
				setPos.y = newYRoof;
			}
			else{
				effectOn = false;
			}

			newMinYPos = setPos.y;
			transform.position = setPos;

			if (GetComponent<Rigidbody>()){
				
				myRigid = GetComponent<Rigidbody>();
				GetComponent<BoxCollider>().enabled = false;
				print (GetComponent<BoxCollider>());
				BoxCollider[] childrenObjs = gameObject.GetComponentsInChildren<BoxCollider>();
				foreach (BoxCollider child in childrenObjs){
					child.enabled = false;
				}
			}
			if (GetComponent<SpringJoint>()){
				mySpring = GetComponent<SpringJoint>();
				sprintAmt = mySpring.spring;
				mySpring.spring = 0;
			}
		}
		else{
			effectOn = false;
		}
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (delayEffectTime > 0){
			delayEffectTime -= Time.deltaTime;
		}
		else{
		if (effectOn){

				Vector3 resetPos = transform.position;

				currentLerpTime += Time.deltaTime;

				// ease out
					float t = currentLerpTime / maxLerpTime;

				if (currentLerpTime != 0){
					if (currentLerpTime >= maxLerpTime){
						resetPos.y = startYPos;
						effectOn = false;

						if (myRigid){
							
							myRigid = GetComponent<Rigidbody>();
							GetComponent<BoxCollider>().enabled = true;
							BoxCollider[] childrenObjs = gameObject.GetComponentsInChildren<BoxCollider>();
							foreach (BoxCollider child in childrenObjs){
								child.enabled = true;
							}
							
							myRigid.AddForce(Random.insideUnitSphere.normalized*10000f*Time.deltaTime, ForceMode.Impulse);
						}

						if (mySpring){
							mySpring.spring = sprintAmt;
						}
					}
					else{
						
						float posDiff = (startYPos-newMinYPos) * Mathf.Pow(2, 10 * ( t - 1 ));
						resetPos.y = newMinYPos+posDiff;
					}
				}



				transform.position = resetPos;

			}
		
		}
	
	}
}
