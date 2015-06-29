using UnityEngine;
using System.Collections;

public class LineDrawerTest : MonoBehaviour {

	LineRenderer lineRender; 
	Transform[] allChildren;
	// Use this for initialization
	void Start () 
	{
		lineRender = this.GetComponent<LineRenderer>();

		allChildren = GetComponentsInChildren<Transform>();

		//print(allChildren[1]);
	}
	
	// Update is called once per frame
	void Update () 
	{
		int childCount = allChildren.Length ; 

		lineRender.SetVertexCount(childCount);

		for(int i = 0; i < childCount; i++)
		{
			if(i==0)
			{
				lineRender.SetPosition(i, allChildren[i+1].transform.position); 
			}
			else
			{
				lineRender.SetPosition(i, allChildren[i].transform.position); 
			}
		}

	}
}
