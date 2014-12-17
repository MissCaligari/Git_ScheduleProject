using UnityEngine;
using System.Collections;

//some globals used everywhere, also some misc global functions that can be called by anyone


public class G : MonoBehaviour 
{
	public Texture2D m_cellTex;
	public Texture2D m_cellTex_x;
	public Texture2D m_cellTex_o;
	
	public static GameObject GameLogic = null;
	public static G m_instance;
	
	public static int CELL_EMPTY = 0;
	public static int CELL_X = 1;
	public static int CELL_O = 2;

	public G ()
	{
			m_instance = this;
	}

	public static G Get() 
	{
		if (m_instance == null)
		{
			Debug.LogWarning("Instance not set");
		}
		return m_instance;
	}
	

	public void Msg(string text)
	{
		Debug.Log(text);
		
		//kill any existing messages
		GameObject oldMsg = GameObject.Find("msg");
		
		if (oldMsg != null)
		{
			Destroy(oldMsg);	
		}
	
		GameObject o = new GameObject("msg");
		
		ShowMsg msg;
		
		msg = o.AddComponent("ShowMsg") as ShowMsg;
		msg.text = text;
	}

}
