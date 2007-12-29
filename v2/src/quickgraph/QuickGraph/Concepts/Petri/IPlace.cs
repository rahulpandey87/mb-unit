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


using System;

namespace QuickGraph.Concepts.Petri
{
	using QuickGraph.Concepts.Petri.Collections;
	/// <summary>
	/// A Place in the HLPN framework
	/// </summary>
	/// <remarks>
	/// <para>
	/// A <see cref="Place"/> is characterized by a set of tokens, called the
	/// <see cref="Marking"/> of the place. The place is <strong>typed</strong>
	/// by the <see cref="StrongType"/> instance. This means only object
	/// of <see cref="Type"/> assignable to <see cref="StrongType"/> can reside
	/// in the place.
	/// </para>
	/// <para>
	/// Usually represented by an ellipses (often circles).
	/// </para>
	/// </remarks>
	public interface IPlace  : IPetriVertex
	{
		ITokenCollection Marking {get;}
	}
}
