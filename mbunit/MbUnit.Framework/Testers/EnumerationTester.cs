// created on 30/01/2004 at 17:17

namespace MbUnit.Framework.Testers 
{
	using MbUnit.Framework;
	using MbUnit.Core;
	using System.Diagnostics;
	using System.Collections;
	using System;
	
	/// <summary>
	/// Tests for the <seealso cref="IEnumerable"/> and <seealso cref="IEnumerator"/>.
	/// </summary>
	public class EnumerationTester
	{
		[Test("Tests the GetEnumerator method")]
		public void GetEnumerator(Object sender, IEnumerable source, IEnumerable tested)
		{
			IEnumerator en = tested.GetEnumerator();
			Assert.IsNotNull(en);			
		}
		
		[Test("Enumerates elements")]
		public void Enumerate(Object sender, IEnumerable source, IEnumerable tested)
		{			
			IEnumerator enSource = source.GetEnumerator();
			IEnumerator en = tested.GetEnumerator();
			while(enSource.MoveNext())
			{
				Assert.IsTrue(en.MoveNext());				
			}
			Assert.IsFalse(en.MoveNext());
		}

		[Test("Enumerates elements and does element-wise comparaison")]
		public void ElementWiseEquality(Object sender, IEnumerable source, IEnumerable tested)
		{
            if (source is IDictionary)
                return;

            IEnumerator enSource = source.GetEnumerator();
			IEnumerator en = tested.GetEnumerator();
			while(enSource.MoveNext())
			{
				Assert.IsTrue(en.MoveNext());
                Assert.AreEqual(enSource.Current,en.Current);				
			}
			Assert.IsFalse(en.MoveNext());
		}

		[Test("Calls the Current property")]
		public void Current(Object sender, IEnumerable source, IEnumerable tested)
		{			
			IEnumerator en = tested.GetEnumerator();
			Object o;
			while(en.MoveNext())
			{
				o = en.Current;
			}
		}

		[Test("Checks that Current without MoveNext fails")]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CurrentWithoutMoveNext(Object sender, IEnumerable source, IEnumerable tested)
		{			
			IEnumerator en = tested.GetEnumerator();
			Object o = en.Current;
		}

		[Test("Checks that Current while MoveNext return false fails")]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CurrentPastEnd(Object sender, IEnumerable source, IEnumerable tested)
		{			
			IEnumerator en = tested.GetEnumerator();
			while(en.MoveNext())
			{}
			
			Object o = en.Current;
		}

		[Test("Test the reseting of the enumrator")]
		public void Reset(Object sender, IEnumerable source, IEnumerable tested)
		{			
			IEnumerator en = tested.GetEnumerator();
			while(en.MoveNext())
			{}
			en.Reset();
			Assert.IsTrue(en.MoveNext());
		}
	}
}
