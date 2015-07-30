using UnityEngine;
using System.Collections;

public class SlashEffectS : MonoBehaviour {

	private float effectSpeed = 75000f;
	public Vector3 moveDir;
	public float lifeTime = 2f;

	private Rigidbody ownRigid;

	// Use this for initialization
	void Start () {
	
		ownRigid = GetComponent<Rigidbody>();


	}
	
	// Update is called once per frame
	void FixedUpdate () {

		ownRigid.velocity = moveDir*effectSpeed*TimeManagerS.timeMult*Time.deltaTime;

		lifeTime -= Time.deltaTime*TimeManagerS.timeMult;

		if (lifeTime <= 0){
			Destroy(gameObject);
		}
	
	}
}
