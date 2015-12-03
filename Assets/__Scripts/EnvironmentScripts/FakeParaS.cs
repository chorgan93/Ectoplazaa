using UnityEngine;
using System.Collections;

public class FakeParaS : MonoBehaviour {

	public AdaptiveCameraPtS adaptRef;

	private float xDiffRef;

	public float paraMult = 2;

	private Vector3 startPos;

	// Use this for initialization
	void Start () {

		startPos = transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {

		xDiffRef = adaptRef.GetXDiff();

		Vector3 paraPos = startPos;
		paraPos.x -= xDiffRef*paraMult;
		transform.position = paraPos;
	
	}
}
