using System;
using System.Collections;

using MbUnit.Framework;

namespace MbUnit.Demo 
{
	public class HashtableFactory
	{
		public Hashtable Empty
		{
			get
			{
				return new Hashtable();
			}
		} 
		public Hashtable RandomFilled
		{
			get
			{
				Hashtable list = new Hashtable();
				Random rnd = new Random((int)DateTime.Now.Ticks);
				for(int i=0;i<15;++i) 
				{
					list.Add(Guid.NewGuid(),rnd.Next());
				}
				return list;
			}
		} 
	}
}