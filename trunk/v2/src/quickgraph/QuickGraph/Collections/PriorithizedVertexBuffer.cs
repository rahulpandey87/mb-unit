// QuickGraph Library 
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		QuickGraph Library HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux


namespace QuickGraph.Collections
{
	using System;
	using System.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// A Priorithized (with respect to distance) vertex buffer.
	/// </summary>
	public sealed class PriorithizedVertexBuffer : 
		VertexBuffer
	{
		private VertexDoubleDictionary distances;
		private DistanceComparer comparer;

		/// <summary>
		/// Builds a priorithzied vertex buffer and fills a vertex distance map.
		/// </summary>
		/// <param name="distances">vertex distance map</param>
		public PriorithizedVertexBuffer(VertexDoubleDictionary distances)
			: base()
		{
			if ( distances == null)
				throw new ArgumentNullException("Distance map is null");
			this.distances = distances;
			this.comparer = new DistanceComparer(this.distances);
		}

		/// <summary>
		/// Updates the buffer order
		/// </summary>
		/// <param name="v">modified vertex</param>
		public void Update(IVertex v)
		{
			// sort queue
			Sort();
		}

		/// <summary>
		/// Push a new vertex on the buffer.
		/// </summary>
		/// <param name="v">new vertex</param>
		public override void Push(IVertex v)
		{
			// add to queue
			base.Push(v);
			// sort queue
			Sort(this.comparer);
		}

        public void Sort()
        {
            this.Sort(this.comparer);
        }
    }
}
