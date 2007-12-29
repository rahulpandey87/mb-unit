using System;
using System.ComponentModel;

namespace QuickGraph.Layout.Shapes
{
	[Serializable]
	public class PropertyEntry
	{
		private string key;
		private string value;

		/// <summary>
		/// Create a new <see cref="PropertyEntry"/>.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public PropertyEntry(string key, string value)
		{
			if (key==null)
				throw new ArgumentNullException("key");
			if (value==null)
				throw new ArgumentNullException("value");
			this.key = key;
			this.value = value;
		}

		[Category("Data")]
		public string Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		[Category("Data")]
		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}	
	}
}
