/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 11/14/2014
 * Time: 10:37 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileManager
{
	/// <summary>
	/// Description of DataFile.
	/// </summary>
	public class DataFile
	{
		private ArrayList bits = new ArrayList();
		private String path = "";
		private bool tile = false;
		
		public DataFile(String path)
		{
			this.path = path;
		}
		
		public bool ReadFile(out String res)
		{
			StreamReader fs = null;
			try{
				fs = new StreamReader(path);
			} catch(FileNotFoundException e) {
				Console.WriteLine("Could not find file at" + path);
				Console.WriteLine(e.Message);
				res = null;
				
				return false;
			}
			
			String file;
			
			try{
				
				file = fs.ReadToEnd();
				
			} catch(Exception e) {
				
				Console.WriteLine("Error reading file " + path);
				Console.WriteLine(e.Message);
				
				res = null;
				
				return false;
			}
			
			res = file;
			
			
			fs.Close();
			return true;
		}
		
		public bool Parse(String s, out List<TileData> dataBits)
		{
			
			
			String[] lines = s.Split(':');
			List<DataBit> bits = new List<DataBit>();
			List<TileData> tiles = new List<TileData>();
			TileData current = new TileData();
			
			foreach(String sl in lines)
			{
				String ln;
				ln = sl.Replace(' ', ':');
				ln = ln.Replace('\"', ' ');
				String[] lns = ln.Split(':');
				int i = 0;
				String val = "null";
				String ty = DataBit.NULL_TYPE;
				foreach(String l in lns)
				{
					String x = l.Trim();
					if(x.Equals("s"))
						tile = true;
					if(x.Equals("e")){
						tile = false;
						tiles.Add(current);
						current = new TileData();
					}
					String xx;
					i++;
					if(x.StartsWith("type"))
					{
						xx = x.Substring(6);
						ty = xx;
					}
					
					if(x.StartsWith("val"))
					{
						xx = x.Substring(5);
						val = xx;
					}
					
					
					
				}
				
				DataBit b = new DataBit(ty.Trim(' '), val.Trim(' '));
				bits.Add(b);
				if(tile){
					if(b.DataType().Equals(DataBit.TILE_TYPE))
						current.SetProperty("name", b.Data());
					else if(b.DataType().Equals(DataBit.NAME_PROPERTY))
						current.SetProperty("name", b.Data());
					else if(b.DataType().Equals(DataBit.DATA_PROPERTY))
						current.SetProperty("data", b.Data());
					else if(b.DataType().Equals(DataBit.VERT_TYPE))
						current.SetProperty("vert", b.Data());
				}
			}
			
			dataBits = tiles;
			foreach(TileData t in tiles)
				Console.WriteLine(t.ToString());
			return true;
		}
		
		
		public bool SaveTileData(TileData dat, bool append)
		{
			StreamWriter writer;
			try{
				writer = new StreamWriter(path, append);
			} catch(FileNotFoundException){
				Console.WriteLine("Could not find file at "+ path + " to write to. Creating...");
				FileStream f = new FileStream(path, FileMode.CreateNew);
				f.Dispose();
				writer = new StreamWriter(path, append);
			}
			writer.WriteLine("\"s:\"");
			writer.WriteLine("\"type\"=\"" + DataBit.TILE_TYPE + 
			                 "\" \"val\"=\"" + dat.GetProperty("name") + "\":");
			foreach(String s in dat.getKeys())
			{
				if(s.Equals("vert"))
					writer.WriteLine("\"type\"=\"" + DataBit.VERT_TYPE + 
			                 		"\" \"val\"=\"" + dat.GetProperty(s) + "\":");
				if(s.Equals("data"))
					writer.WriteLine("\"type\"=\"" + DataBit.DATA_PROPERTY + 
					                 "\" \"val\"=\"" + dat.GetProperty(s) + "\":");
					
			}
			
			writer.WriteLine("\"e\":");
			
			writer.Close();
			return true;
			                                       
		}
		
		public void SetPath(string p)
		{
			path = p;
		}
	}
	
}
