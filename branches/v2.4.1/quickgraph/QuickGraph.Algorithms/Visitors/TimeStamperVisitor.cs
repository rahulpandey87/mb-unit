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
//		QuickGraph Library HomePage: http://mbunit.tigris.org
//		Author: Jonathan de Halleux


namespace QuickGraph.Algorithms.Visitors
{
	using System;
	using QuickGraph.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Visitors;

	/// <summary>
	/// Description résumée de TimeStamperVisitor.
	/// </summary>
	public class TimeStamperVisitor :
		ITimeStamperVisitor
	{
		private VertexIntDictionary m_DiscoverTimes;
		private VertexIntDictionary m_FinishTimes;
		private int m_Time;

		/// <summary>
		/// Default constructor
		/// </summary>
		public TimeStamperVisitor()
		{
			m_DiscoverTimes = new VertexIntDictionary();
			m_FinishTimes = new VertexIntDictionary();
			m_Time = 0;
		}

		/// <summary>
		/// Vertex discover time dictionary
		/// </summary>
		public VertexIntDictionary DiscoverTimes
		{
			get
			{
				return m_DiscoverTimes;
			}
		}

		/// <summary>
		/// Vertex finish time dictionary
		/// </summary>
		public VertexIntDictionary FinishTimes
		{
			get
			{
				return m_FinishTimes;
			}
		}

		/// <summary>
		/// Current time
		/// </summary>
		public int Time
		{
			get
			{
				return m_Time;
			}
			set
			{
				m_Time = value;
			}
		}

		/// <summary>
		/// Store the current time in the discover dictionary and increment
		/// the current time.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void DiscoverVertex(Object sender, VertexEventArgs args)
		{
			m_DiscoverTimes[args.Vertex] = m_Time++;
		}

		/// <summary>
		/// Store the current time in the finish dictionary and increment
		/// the current time.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void FinishVertex(Object sender, VertexEventArgs args)
		{
			m_FinishTimes[args.Vertex] = m_Time++;
		}
	}
}
