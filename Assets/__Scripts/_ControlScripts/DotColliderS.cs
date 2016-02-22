using UnityEngine;
using System.Collections;

public class DotColliderS : MonoBehaviour {
	
	public PlayerS whoCreatedMe;
	public PlayerEffectS lightningTrail;
	public PlayerEffectS fireTrail;

	void Start(){
		//lightningTrail.playerRef = whoCreatedMe;
		//fireTrail.playerRef = whoCreatedMe;
	}

	void FixedUpdate () {
		if (!whoCreatedMe){
			Destroy(gameObject);
		}
	}

}
