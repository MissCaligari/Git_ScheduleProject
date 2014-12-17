using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RTDB 
{
	Dictionary<string, object> m_database = new Dictionary<string, object>();
	
	public RTDB(string key1, object value1)
	{
		m_database[key1] = value1;
	}
	
	public RTDB(string key1, object value1, string key2, object value2)
	{
		m_database[key1] = value1;
		m_database[key2] = value2;
	}

	public RTDB(string key1, object value1, string key2, object value2, string key3, object value3)
	{
		m_database[key1] = value1;
		m_database[key2] = value2;
		m_database[key3] = value3;
	}
	
	public object Get(string key)
	{
		return m_database[key];	
	}
	
	public string GetString(string key)
	{
		if (m_database[key].GetType() != typeof(string))
		{
			Debug.LogWarning(key+" should be string but is "+m_database[key].GetType());
		}
		return m_database[key] as string;	
	}
	
	public Vector3 GetVector3(string key)
	{
		if (m_database[key].GetType() != typeof(Vector3))
		{
			Debug.LogWarning(key+" should be vector3 but is "+m_database[key].GetType());
		}
		return (Vector3)m_database[key];	
	}
	
	public float GetFloatWithDefault(string key, float v)
	{
		if (m_database.ContainsKey(key))
		{
			return (float)m_database[key];
		}
		
		//create it and set the default
		
		m_database[key] = v;
		return v;
	}
	
	public void Set(string key, object v)
	{
		m_database[key] = v;	
	}
	
	public override string ToString()
	{
		string s = "";
		s += "RTDB contains "+m_database.Count+" pairs: ";
		
		foreach (KeyValuePair<string, object> pair in m_database)
		{
		    s += pair.Key + "=" + pair.Value+" ("+pair.Value.GetType()+")\n";
		}	
		
		return s;
	}
}
