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

	public Sprite [] chargeBarSprites; 
	public GameObject chargeBarSprite; 

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
			//ownRender.enabled = true;
			chargeBarSprite.GetComponent<SpriteRenderer>().enabled = true; 
		}
		else{
			//ownRender.enabled = false;
			chargeBarSprite.GetComponent<SpriteRenderer>().enabled = false; 
		}

		if (chargeBarSprite.GetComponent<SpriteRenderer>().enabled){

			// set size

			if(!(playerRef.GetChargeTime() >  playerRef.GetChargeLv3Min()*1.5f))
			{
				float newScaleValue = playerRef.GetChargeTime()/ playerRef.GetChargeLv3Min();
				Vector3 newScale = new Vector3 (newScaleValue,newScaleValue,1f); 
				this.transform.localScale = newScale;
			}

			if (playerRef.GetChargeTime() > lv3Min){

				float lv3Size = startSize*lv3SizeMult;
				chargeBarSprite.GetComponent<SpriteRenderer>().sprite = chargeBarSprites[2]; 
				//transform.localScale = new Vector3(lv3Size,lv3Size,1);
			}
			else if (playerRef.GetChargeTime() > lv2Min){
				
				float lv2Size = startSize*lv2SizeMult;
				chargeBarSprite.GetComponent<SpriteRenderer>().sprite = chargeBarSprites[1]; 
				//transform.localScale = new Vector3(lv2Size,lv2Size,1);
			}
			else{
				chargeBarSprite.GetComponent<SpriteRenderer>().sprite = chargeBarSprites[0]; 

				//transform.localScale = new Vector3(startSize,startSize,1);
			}

			// set pos

			Vector3 aimDir = Vector3.zero;
			aimDir.x = Input.GetAxis("HorizontalPlayer"+playerRef.playerNum+platformType);
			aimDir.y = Input.GetAxis("VerticalPlayer"+playerRef.playerNum+platformType);

			transform.position = playerRef.transform.position + aimDir.normalized*aimRadius;

			//print(aimDir); 

			float newAngle = 0; 

			if(aimDir.x >= 0)
			{
				newAngle += 90 - (aimDir.y *90); 
			}
			else
			{
				newAngle += 270 + (aimDir.y *90); 
			}

			//print(newAngle); 
			chargeBarSprite.transform.rotation = Quaternion.Euler(0,0,-newAngle); 

		}

	}
}
