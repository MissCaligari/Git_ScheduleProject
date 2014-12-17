using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
	
Created by Seth A. Robinson - rtsoft.com 2013
	
 TO SETUP:
 
 Create a GameObject in unity called RTAudioManager and attach this script 
 
To call from any script:

//to play a sfx.. must be less then 8 seconds for no good reason

RTAudioManager.Get().Play("sfxFilename");

//to schedule it using RTEventManager to play in X seconds:
			
RTEventManager.Get().Schedule(RTAudioManager.GetName(), "Play", 1, "sfxFilename");

//to play a mp3 song (song or its folders must be placed off of Assets/Resources)
//this will replace any existing music
RTAudioManager.Get().PlayMusic("audio/wiz", 1.0f, 1.0f, true);

//To use with RTEventManager to play the same song 2 seconds later   
RTEventManager.Get().Schedule(RTAudioManager.GetName(), "PlayMusic", 2, "musicFileName");

 
  //Example of sending multiple parms with RTAudioManager's PlayEx function:
RTEventManager.Get().Schedule(RTAudioManager.GetName(), "PlayEx", 2, new RTDB("fileName", "chalk",
			"volume", 1.0f, "pitch", 2.0f));
			
Note: If a parm is missing with PlayEx the default is used instead

Note: .wav or .mp3/ogg etc must be located in the Assets/Resources so unity includes them automatically.
They must NOT be marked as 3d sounds, if you hear nothing but don't see an error, this is what's wrong.
 
*/ 


public class RTAudioManager : MonoBehaviour 
{
	static RTAudioManager m_instance = null;
	public AudioSource m_activeMusic = null;
	
	
	RTAudioManager()
	{
			m_instance = this;
	}
	

	public void Start()
	{
		gameObject.name = "RTAudioManager";
		Debug.Log("RTAudioManager initted, gameobject we're in renamed to RTAudioManager");
	}
	
	public void PlayEx(RTDB db)
	{
		PlayEx(db.GetString("fileName"),
			db.GetFloatWithDefault("volume", 1.0f),
			db.GetFloatWithDefault("pitch", 1.0f)
			);
	}
	
	public void PlayMusic(string fileName)
	{
		PlayMusic(fileName, 1, 1, true);
	}
	
	public void PlayMusic(string fileName, float vol, float pitch, bool loop)
	{
		Debug.Log ("Playing music "+fileName);
		AudioClip clip = Resources.Load(fileName) as AudioClip;
 	 	
		if (clip == null)
		{
			Debug.LogWarning ("Couldn't find "+fileName);
			return;
		}
		if (m_activeMusic != null)
		{
			m_activeMusic.Stop();
			m_activeMusic = null;
			
		}
		m_activeMusic = gameObject.AddComponent("AudioSource") as AudioSource;
		audio.volume = vol;
		audio.pitch = pitch;
		audio.loop = true;
		audio.PlayOneShot(clip);
	}
	
	
	public void PlayEx(string fileName, float vol, float pitch)
	{
		//Debug.Log ("Playing sfx "+fileName);
		AudioClip clip = Resources.Load(fileName) as AudioClip;
 	 	
		if (clip == null)
		{
			Debug.LogWarning ("Couldn't find "+fileName);
			return;
		}
		GameObject o = new GameObject("sfx");
		o.AddComponent("AudioSource");
		o.audio.volume = vol;
		o.audio.pitch = pitch;
		o.audio.PlayOneShot(clip);
		GameObject.Destroy(o, 10); //kill it in a bit.. todo: this time should be the actual time the clip takes to play..
	}
	
	public void Play(string fileName)
	{
		PlayEx(fileName, 1, 1);
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public static RTAudioManager Get()
	{
		if (!m_instance)
		{
			Debug.LogWarning("RTAudioManager: Huh?  You need to attach this script to a gameobject first.");
		}
		return m_instance;
	}
	
	public static string GetName()
	{
		return Get ().name;
	}
	
	
}
