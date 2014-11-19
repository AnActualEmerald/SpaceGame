/*
 * Created by SharpDevelop.
 * User: kgauthier16
 * Date: 11/14/2014
 * Time: 10:21 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace FileManager
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class DataBit
	{
		public const String NULL_TYPE = "null"; // this is just an example member, replace it with your own struct members!
		public const String NUM_TYPE = "num";
		public const String TILE_TYPE = "tile";
		public const String VERT_TYPE = "vert";
		public const String STRING_TYPE = "string";
		public const String NAME_PROPERTY = "name";
		public const String DATA_PROPERTY = "dat";
		
		
		private String _DataType = DataBit.NULL_TYPE;
		private String _Data = "null";
	
		public DataBit()
		{
			
		}
		
		public DataBit(String DataType, String Data)
		{
			this._DataType = DataType;
			this._Data = Data;
		}
		
		public String Data()
		{
			return _Data;
		}
		
		public void SetData(String data)
		{
			this._Data = data;
		}
		
		public String DataType()
		{
			return _DataType;
		}
		
		public void SetDataType(String _type)
		{
			this._DataType = _type;
		}
		
		public override string ToString()
		{
			return string.Format("[DataBit DataType={0}, Data={1}]", _DataType, _Data);
		}

	}
}
