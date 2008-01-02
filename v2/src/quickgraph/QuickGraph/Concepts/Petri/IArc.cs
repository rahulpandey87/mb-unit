using System;
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

namespace QuickGraph.Concepts.Petri
{
	/// <summary>
	/// A directed edge of a net which may connect a <see cref="IPlace"/>
	/// to a <see cref="ITransition"/> or a <see cref="ITransition"/> to
	/// a <see cref="IPlace"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Usually represented by an arrow.
	/// </para>
	/// </remarks>
	public interface IArc  : IEdge
	{
		/// <summary>
		/// Gets or sets a value indicating if the <see cref="IArc"/>
		/// instance is a <strong>input arc.</strong>
		/// </summary>
		/// <remarks>
		/// <para>
		/// An arc that leads from an input <see cref="IPlace"/> to a
		/// <see cref="ITransition"/> is called an <em>Input Arc</em> of
		/// the transition.
		/// </para>
		/// </remarks>
		bool IsInputArc {get;}

		/// <summary>
		/// Gets or sets the <see cref="IPlace"/> instance attached to the
		/// <see cref="IArc"/>.
		/// </summary>
		/// <value>
		/// The <see cref="IPlace"/> attached to the <see cref="IArc"/>.
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference (Nothing in Visual Basic).
		/// </exception>
		IPlace Place {get;}

		/// <summary>
		/// Gets or sets the <see cref="ITransition"/> instance attached to the
		/// <see cref="IArc"/>.
		/// </summary>
		/// <value>
		/// The <see cref="ITransition"/> attached to the <see cref="IArc"/>.
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference (Nothing in Visual Basic).
		/// </exception>
		ITransition Transition{get;}

		/// <summary>
		/// Gets or sets the arc annotation.
		/// </summary>
		/// <value>
		/// The <see cref="IExpression"/> annotation instance.
		/// </value>
		/// <remarks>
		/// <para>
		/// An expression that may involve constans, variables and operators
		/// used to annotate the arc. The expression evaluates over the type
		/// of the arc's associated place.
		/// </para>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference (Nothing in Visual Basic).
		/// </exception>
		IExpression Annotation {get;set;}
	}
}
