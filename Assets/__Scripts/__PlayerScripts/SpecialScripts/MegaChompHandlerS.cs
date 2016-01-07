using UnityEngine;
using System.Collections;

public class MegaChompHandlerS : MonoBehaviour {

	private float topStartY = 50;
	private float bottomStartY = -50;
	private float startY;

	private float moveSpeed = 5f;
	private float chompAccel = 100f; 

	private float stopMovingTimeMax = 0.1f;
	private float stopMovingTimeCountdown;

	private bool playerSet = false;

	public DamageS topChomp;
	public DamageS bottomChomp;

	public PlayerS playerRef;


	// Update is called once per frame
	void FixedUpdate () {

		if (playerRef){

			if (!playerSet){
				topChomp.MakeSpecial(playerRef, this);
				bottomChomp.MakeSpecial(playerRef, this);

				transform.position = playerRef.transform.position;
				startY = transform.position.y;

				topChomp.transform.position = new Vector3(transform.position.x, startY+topStartY, 0);
				bottomChomp.transform.position = new Vector3(transform.position.x, startY+bottomStartY, 0);

				playerSet = true;

			}
			else{



				if (stopMovingTimeCountdown <= 0){

					Vector3 topPos = topChomp.transform.position;
					Vector3 bottomPos = bottomChomp.transform.position;

					topPos.y -= moveSpeed*Time.deltaTime*TimeManagerS.timeMult;
					bottomPos.y += moveSpeed*Time.deltaTime*TimeManagerS.timeMult;

					moveSpeed += chompAccel*Time.deltaTime*TimeManagerS.timeMult;

					topChomp.transform.position = topPos;
					bottomChomp.transform.position = bottomPos;

				}
				else{
					stopMovingTimeCountdown -= Time.deltaTime*TimeManagerS.timeMult;
				}

			}
				                                 

		}
	
	}

	public void EndMega(){

		playerRef.EndSpecialCooldown();
		Destroy(gameObject);

	}

	public void PauseMega(){

		stopMovingTimeCountdown = stopMovingTimeMax;

	}
}
