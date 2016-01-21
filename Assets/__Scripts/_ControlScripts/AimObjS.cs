using UnityEngine;
using System.Collections;

public class AimObjS : MonoBehaviour {

	public float aimRadius = 2.5f;

	private float lv2Min;
	private float lv3Min;

	private float startSize;
	public float lv2SizeMult = 1.5f;
	public float lv3SizeMult = 2.25f;


	private PlayerS playerRef;

	private string platformType;

	private SpriteRenderer chargeBarSpriteRender;

	public Sprite [] chargeBarSprites; 
	public GameObject chargeBarSprite; 

	private float maxShakeOffset = 0.3f;
	private float maxChargeHoldTime = 2f;
	private Vector3 chargeShakeOffset;

	// Use this for initialization
	void Start () {

		platformType = PlatformS.GetPlatform();

		playerRef = transform.parent.GetComponent<PlayerS>();
		transform.parent = null;

		startSize = transform.localScale.x;

		lv2Min = playerRef.GetChargeLv2Min();
		lv3Min = playerRef.GetChargeLv3Min();

		chargeBarSpriteRender = chargeBarSprite.GetComponent<SpriteRenderer>();
		chargeBarSpriteRender.enabled = false; 
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (playerRef){

			if (playerRef.spriteObject.GetComponent<SpriteRenderer>().enabled && playerRef.health > 0){

				chargeBarSprite.SetActive(true);
	
		if (playerRef.charging || (playerRef.attacking && playerRef.attackToPerform == 1)){
					chargeBarSpriteRender.enabled = true; 
		}
		else{
					chargeBarSpriteRender.enabled = false; 
		}

				if (chargeBarSpriteRender.enabled){

			// set size


			if (playerRef.GetChargeTime() > lv3Min){

						chargeBarSpriteRender.sprite = chargeBarSprites[2]; 
						transform.localScale = new Vector3(startSize*lv3SizeMult,startSize*lv3SizeMult,1);
			}
			else if (playerRef.GetChargeTime() > lv2Min){
				
						chargeBarSpriteRender.sprite = chargeBarSprites[1]; 
				transform.localScale = new Vector3(startSize*lv2SizeMult,startSize*lv2SizeMult,1);
			}
			else{
						chargeBarSpriteRender.sprite = chargeBarSprites[0]; 

				transform.localScale = new Vector3(startSize,startSize,1);
			}

			// set pos

			Vector3 aimDir = Vector3.zero;
			aimDir.x = Input.GetAxis("HorizontalPlayer"+playerRef.playerNum+platformType);
			aimDir.y = Input.GetAxis("VerticalPlayer"+playerRef.playerNum+platformType);


			transform.position = playerRef.transform.position + aimDir.normalized*aimRadius;

					// shake sprite pos
					chargeShakeOffset = Random.insideUnitSphere* 
						(maxShakeOffset*Mathf.Pow((playerRef.GetChargeTime()/maxChargeHoldTime), 2));
					chargeShakeOffset.z = transform.position.z;
					chargeBarSprite.transform.localPosition = chargeShakeOffset;

			
			float newAngle = 0; 

			if(aimDir.x == 0 && aimDir.y == 0)
			{
				newAngle = 90 ; 
			}
			else if(aimDir.x >= 0 && aimDir.y >= 0)
			{
				newAngle += 45 +((45f*aimDir.x) - (45f*aimDir.y));
			}
			else if(aimDir.x >= 0 && aimDir.y <= 0)
			{
				newAngle += 135 +((-45f*aimDir.x) - (45f*aimDir.y));
			}
			else if(aimDir.x <= 0 && aimDir.y <=0)
			{
				newAngle += 225 +((-45f*aimDir.x) + (45f*aimDir.y));
			}
			else if(aimDir.x <= 0 && aimDir.y >=0)
			{
				newAngle += 315 +((45f*aimDir.x) + (45f*aimDir.y));
			}

			//print(aimDir); 
			/*
			float newAngle = 0; 

			if(aimDir.x >= 0)
			{
				newAngle += 90 - (aimDir.y *90); 
			}
			else
			{
				newAngle += 270 + (aimDir.y *90); 
			}
*/
			//print(newAngle); 
			chargeBarSprite.transform.rotation = Quaternion.Euler(0,0,-newAngle); 
				}	
				else{
					chargeBarSprite.SetActive(false);
				}


		}
		}
		else{
			chargeBarSprite.SetActive(false);
		}

	}
}
