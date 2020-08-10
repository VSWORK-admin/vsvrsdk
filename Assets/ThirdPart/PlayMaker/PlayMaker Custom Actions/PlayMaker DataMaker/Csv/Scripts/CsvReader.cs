// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.
// Original script by Francois GUIBERT, Frozax Games, @Frozax
// https://raw.githubusercontent.com/frozax/fgCSVReader/master/fgCSVReader.cs


using UnityEngine;

using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Ecosystem.DataMaker.CSV
{
	public class CsvData 
	{
		#region References

		/// <summary>
		/// RunTime memory References to work on csv data in various Actions.
		/// 
		/// Notes: to avoid using Unity Objects and garbage Collection issues, it's preffered to use such runtime References for better Memory Management.
		/// </summary>
		public static Dictionary<string,CsvData> References;


		/// <summary>
		/// Remove all runtime references of Csv Data.
		/// </summary>
		public static void RemoveAllReferences()
		{
			References = null;
		}

		/// <summary>
		/// Adds a Csv Data into runtime Memory using a Reference. Use this reference in the various csv related Actions to work with that particular Data.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="reference">Reference.</param>
		public static void AddReference(CsvData data,string reference)
		{
			if (References==null)
			{
				References = new Dictionary<string, CsvData>();
			}

			References[reference] = data;
		}

		/// <summary>
		/// Adds a Csv Data into runtime Memory using a Reference. Use this reference in the various csv related Actions to work with that particular Data.
		/// </summary>
		/// <returns><c>true</c>, if reference was removed, <c>false</c> otherwise.</returns>
		/// <param name="reference">Reference.</param>
		public static bool RemoveReference(string reference)
		{
			if (References!=null)
			{
				return References.Remove(reference);
			}

			return false;
		}

		/// <summary>
		/// Determines if there is this reference in memory.
		/// </summary>
		/// <returns><c>true</c> if has reference in memory; otherwise, <c>false</c>.</returns>
		/// <param name="reference">Reference.</param>
		public static bool HasReference(string reference)
		{
			return References!=null && References.ContainsKey(reference);
		}

		/// <summary>
		/// Gets the related reference csv Data.
		/// </summary>
		/// <returns>The csv data linked to that reference.</returns>
		/// <param name="reference">Reference.</param>
		public static CsvData GetReference(string reference)
		{
			if (References!=null && References.ContainsKey(reference))
			{
				return References[reference];
			}else{
				return null;
			}
		}

		#endregion

		#region Class

		public List<List<String>> Data = new List<List<string>>();

		public List<String> HeaderKeys = new List<string>();

		public int RecordCount;

		public int ColumnCount;

		public bool HasHeader = false;

		bool headerDone = false;

		public CsvData(bool hasHeader= false)
		{
			HasHeader = hasHeader;
		}


		public void AddRecord(List<string> items)
		{

			ColumnCount = Mathf.Max(ColumnCount,items.Count);
			if (HasHeader && !headerDone)
			{
				headerDone = true;
			//	Debug.Log("Add Header keys:"+string.Join(",",items.ToArray()));
				HeaderKeys = new List<string>(items);
			}else{

			//	Debug.Log("Add Entries: "+items.Count+" Items :"+string.Join(",",items.ToArray()));
				Data.Add(new List<string>(items));
			}

			RecordCount = Data.Count;
		}

		public void OnParseEnded()
		{
			foreach(List<string> _row in Data)
			{
				if (_row.Count<ColumnCount)
				{
					_row.AddRange(Enumerable.Repeat(string.Empty, ColumnCount-_row.Count));
				//	Debug.Log("addrange :"+_row.Count);
				}

			}

		}


		public string GetFieldAt(int record,int column,bool logErrors = true)
		{
			if (Data.Count<=record)
			{
				Debug.LogError("GetFieldAt record out of range. Data.Count="+Data.Count+" record ="+record);
				return String.Empty;
			}
			if (Data[record].Count<=column)
			{
				Debug.LogError("GetFieldAt column out of range.  Data.Count="+Data.Count+" record ="+record+" Data[record].Count="+Data[record].Count+" column="+column);
				return String.Empty;
			}

			return Data[record][column];
		}

		public string GetFieldAt(int record,string key)
		{
			if (!HasHeader)
			{
				Debug.LogError("GetFieldAt trying to access csvData with no header");
				return String.Empty;
			}

			if (Data.Count<=record)
			{
				Debug.LogError("GetFieldAt record out of range. Data.Count="+Data.Count+" record ="+record);
				return String.Empty;
			}

			int _column = HeaderKeys.IndexOf(key);
			if (Data[record].Count<=_column)
			{
				Debug.LogError("GetFieldAt column out of range.  Data.Count="+Data.Count+" record ="+record+" Data[record].Count="+Data[record].Count+" column="+_column);
				return String.Empty;
			}

			return Data[record][_column];
		}

		public string[] GetRecordAt(int record)
		{
			if (Data.Count<=record)
			{
				Debug.LogError("GetRecordAt record index out of range. Data.Count="+Data.Count+" record ="+record);
				return new string[ColumnCount];
			}

			return Data[record].ToArray();
		}

		#endregion
	}
	
	public class CsvReader
	{
		public static CsvData LoadFromString(string file_contents, bool hasHeader = false,char delimiter = ',')
		{
			CsvData _result = new CsvData(hasHeader);

			int file_length = file_contents.Length;
			
			// read char by char and when a , or \n, perform appropriate action
			int cur_file_index = 0; // index in the file
			List<string> cur_line = new List<string>(); // current line of data

		
			StringBuilder cur_item = new StringBuilder("");
			bool inside_quotes = false; // managing quotes
			while (cur_file_index < file_length)
			{
				char c = file_contents[cur_file_index++];

				if (c == '"') {
					if (!inside_quotes) {
						inside_quotes = true;
					} else {
						if (cur_file_index == file_length) {
							// end of file
							inside_quotes = false;
							//goto case '\n';
							cur_line.Add (cur_item.ToString ());
							cur_item.Length = 0;
							if (c == '\n' || cur_file_index == file_length) {
								_result.AddRecord (cur_line);
								cur_line.Clear ();
							}

						} else if (file_contents [cur_file_index] == '"') {
							// double quote, save one
							cur_item.Append ("\"");
							cur_file_index++;
						} else {
							// leaving quotes section
							inside_quotes = false;
						}
					}
				} else if (c == '\r') {
					// ignore it completely
					Debug.Log ("OUPPS found a \\r");
				} else if (c == '\n' || c == delimiter) {

					if (inside_quotes) {
						// inside quotes, this characters must be included
						cur_item.Append (c);
					} else {
						// end of current item
						cur_line.Add (cur_item.ToString ());
						cur_item.Length = 0;
						if (c == '\n' || cur_file_index == file_length) {
							_result.AddRecord (cur_line);
							cur_line.Clear ();
						}
					}
				} else {

					// other cases, add char
					cur_item.Append (c);
				}

			}

			// fix for last item of last line not using quotes
			if (cur_item.Length!=0)
			{
				// end of current item
				cur_line.Add(cur_item.ToString());
				cur_item.Length = 0;
				_result.AddRecord(cur_line);
				cur_line.Clear();
			}

			_result.OnParseEnded();

			return _result;
		}
	}
}