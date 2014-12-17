using UnityEngine;
using System.Collections;

public class ShowMsg : MonoBehaviour
{
	public string text;
	float dieTimer;
	
	// Use this for initialization
	void Start () 
	{
		dieTimer = Time.time+4;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (dieTimer < Time.time)
		{
			Destroy(gameObject);	
		}
	}
	
	void OnGUI ()
	{

		//scale the font down on smaller screensizes
		float fontMod = Mathf.Min(1.0f, (Screen.width/1024)+0.5f);

		// Make a group on the center of the screen
		GUI.BeginGroup (new Rect (20*fontMod, 20*fontMod, Screen.width-(40*fontMod),100*fontMod));
		// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.
		
		GUI.skin.box.wordWrap = true;
		GUI.skin.box.richText = true;
		GUI.skin.box.fontSize = (int) (40.0f*fontMod);


		//GUI.color.a = 0.1f;
		// We'll make a box so you can see where the group is on-screen.
		GUI.Box (new Rect (0,0,Screen.width-(40*fontMod),800), text);
		//GUI.Button (new Rect (10,40,80,30), "Click me");

		// End the group we started above. This is very important to remember!
		GUI.EndGroup ();
	}
}
