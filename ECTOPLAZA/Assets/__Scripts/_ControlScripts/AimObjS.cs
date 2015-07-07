using UnityEngine;
using System.Collections;

public class AimObjS : MonoBehaviour {

	public float aimRadius = 2.5f;

	private float lv2Min;
	private float lv3Min;

	public float lv2SizeMult = 1.5f;
	public float lv3SizeMult = 2.25f;

	private float startSize;

	private Renderer ownRender;

	private PlayerS playerRef;

	private string platformType;

	// Use this for initialization
	void Start () {

		platformType = PlatformS.GetPlatform();

		playerRef = transform.parent.GetComponent<PlayerS>();
		transform.parent = null;

		startSize = transform.localScale.x;
		ownRender = GetComponent<Renderer>();

		lv2Min = playerRef.GetChargeLv2Min();
		lv3Min = playerRef.GetChargeLv3Min();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if (playerRef.charging){
			ownRender.enabled = true;
		}
		else{
			ownRender.enabled = false;
		}

		if (ownRender.enabled){

			// set size

			if (playerRef.GetChargeTime() > lv3Min){

				float lv3Size = startSize*lv3SizeMult;

				transform.localScale = new Vector3(lv3Size,lv3Size,1);
			}
			else if (playerRef.GetChargeTime() > lv2Min){
				
				float lv2Size = startSize*lv2SizeMult;
				
				transform.localScale = new Vector3(lv2Size,lv2Size,1);
			}
			else{
				transform.localScale = new Vector3(startSize,startSize,1);
			}

			// set pos

			Vector3 aimDir = Vector3.zero;
			aimDir.x = Input.GetAxis("HorizontalPlayer"+playerRef.playerNum+platformType);
			aimDir.y = Input.GetAxis("VerticalPlayer"+playerRef.playerNum+platformType);

			transform.position = playerRef.transform.position + aimDir.normalized*aimRadius;
		}

	}
}
