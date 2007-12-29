using System;
using System.Data;
using System.Data.SqlClient;

namespace TestFu.Data
{
	/// <summary>
	/// Enumeration of available SQL backup devices
	/// </summary>
	/// <include 
	///		file='Data/TestFu.Data.Doc.xml' 
	///		path='//example[contains(descendant-or-self::*,"DbBackupDevice")]'
	///		/>
	public enum DbBackupDevice
	{
		/// <summary>
		/// DISK device
		/// </summary>
		Disk,
		/// <summary>
		/// TAPE device
		/// </summary>
		Tape,
		/// <summary>
		/// Output to named dump
		/// </summary>
		Dump
	}
}
