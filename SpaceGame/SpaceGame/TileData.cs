/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 11/17/2014
 * Time: 3:22 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace FileManager
{
	/// <summary>
	/// Description of TileData.
	/// </summary>
	public class TileData
	{
		
		private Dictionary<String, Object> properties = new Dictionary<String, Object>();
		
		public TileData()
		{
		}
		
		public void SetProperty(String name, Object val)
		{
			if(properties.ContainsKey(name))
				properties[name] = val;
			else
				properties.Add(name, val);
		}
		
		public Object GetProperty(String name)
		{
			return properties[name];
		}
		
		public String[] getKeys()
		{
			String[] keys = new String[properties.Keys.Count];
			properties.Keys.CopyTo(keys, 0);
			return keys;
		}
		
		public override string ToString()
		{
			String desc = "";
			String[] keys = new String[properties.Keys.Count];
			properties.Keys.CopyTo(keys, 0);
			int i = 0;
			do{
				try{
					desc += keys[i] + ", " + properties[keys[i]] + "; ";
					i++;
				} catch(Exception){
					return desc;
				}
			}while(true);
		}

	}
}
