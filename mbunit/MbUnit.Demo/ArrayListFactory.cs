using System;
using System.Collections;

using MbUnit.Framework;

namespace MbUnit.Demo 
{
	public class ArrayListFactory
	{
		public ArrayList Empty
		{
			get
			{
				return new ArrayList();
			}
		} 
		public ArrayList RandomFilled()
		{
			ArrayList list = new ArrayList();
			Random rnd = new Random((int)DateTime.Now.Ticks);
			for(int i=0;i<15;++i) 
				list.Add(rnd.Next());
			return list;
		} 
	}
}