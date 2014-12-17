using UnityEngine;
using System.Collections;

public class MeshFlasher : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( Time.time%1 > 0.5)
		{
			gameObject.renderer.material.SetColor("_Color", Color.white);
		} else
		{
			gameObject.renderer.material.SetColor("_Color", Color.red);
		}
		Debug.Log(gameObject.name+" is flashing");
	}
}
