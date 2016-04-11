using UnityEngine;
using System.Collections;

public class StartEffectS : MonoBehaviour {

	public GameObject slashObj;
	private Rigidbody slashRigid;

	public float slashXSpeed;

	public float slashLifetime = 2f;

	public GameObject flashObj;

	// Use this for initialization
	void Start () {

		slashRigid = slashObj.GetComponent<Rigidbody>();

		slashRigid.velocity = new Vector3(slashXSpeed*Time.deltaTime, 0, 0);

		flashObj.SetActive(true);
	
	}
	
	// Update is called once per frame
	void Update () {

		slashLifetime -= Time.deltaTime;
		if (slashLifetime <= 0 && slashObj.activeSelf){
			slashObj.SetActive(false);
		}
	
	}
}
