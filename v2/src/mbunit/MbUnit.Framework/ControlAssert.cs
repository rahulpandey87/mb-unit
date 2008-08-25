// MbUnit Test Framework
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
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux

using MbUnit.Framework;
using System.Windows.Forms;

namespace MbUnit.Framework
{
    /// <summary>
    /// Class containing generic assert methods for the comparison of Windows Form <see cref="Control"/>s.
    /// </summary>
	public sealed class ControlAssert
	{
		#region Private constructor
		private ControlAssert()
		{}		
		#endregion		
			
		/// <summary>
		/// Verifies that the property value <see cref="Control.AccessibleDefaultActionDescription"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAccessibleDefaultActionDescriptionEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreAccessibleDefaultActionDescriptionEqual(expected.AccessibleDefaultActionDescription,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.AccessibleDefaultActionDescription"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAccessibleDefaultActionDescriptionEqual(
			System.String expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.AccessibleDefaultActionDescription);
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.AccessibleDescription"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAccessibleDescriptionEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreAccessibleDescriptionEqual(expected.AccessibleDescription,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.AccessibleDescription"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAccessibleDescriptionEqual(
			System.String expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.AccessibleDescription);
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.AccessibleName"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAccessibleNameEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreAccessibleNameEqual(expected.AccessibleName,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.AccessibleName"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAccessibleNameEqual(
			System.String expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.AccessibleName);			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.AccessibleRole"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAccessibleRoleEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreAccessibleRoleEqual(expected.AccessibleRole,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.AccessibleRole"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAccessibleRoleEqual(
			System.Windows.Forms.AccessibleRole expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.AccessibleRole,
						"Property AccessibleRole not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.AllowDrop"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void AllowDrop(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.AllowDrop,
						  "Property AllowDrop is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.AllowDrop"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotAllowDrop(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.AllowDrop,
						  "Property AllowDrop is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.AllowDrop"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAllowDropEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreAllowDropEqual(expected.AllowDrop,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.AllowDrop"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAllowDropEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.AllowDrop,
						"Property AllowDrop not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Anchor"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAnchorEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreAnchorEqual(expected.Anchor,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Anchor"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreAnchorEqual(
			System.Windows.Forms.AnchorStyles expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Anchor,
						"Property Anchor not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.BackColor"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreBackColorEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreBackColorEqual(expected.BackColor,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.BackColor"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreBackColorEqual(
			System.Drawing.Color expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.BackColor,
						"Property BackColor not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.BackgroundImage"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreBackgroundImageEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreBackgroundImageEqual(expected.BackgroundImage,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.BackgroundImage"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreBackgroundImageEqual(
			System.Drawing.Image expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.BackgroundImage,
						"Property BackgroundImage not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.DataBindings"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreDataBindingsEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreDataBindingsEqual(expected.DataBindings,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.DataBindings"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreDataBindingsEqual(
			System.Windows.Forms.ControlBindingsCollection expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.DataBindings);
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.BindingContext"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreBindingContextEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreBindingContextEqual(expected.BindingContext,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.BindingContext"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreBindingContextEqual(
			System.Windows.Forms.BindingContext expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.BindingContext);
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Bottom"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreBottomEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreBottomEqual(expected.Bottom,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Bottom"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreBottomEqual(
			System.Int32 expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Bottom,
						"Property Bottom not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Bounds"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreBoundsEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreBoundsEqual(expected.Bounds,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Bounds"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreBoundsEqual(
			System.Drawing.Rectangle expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Bounds,
						"Property Bounds not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.CanFocus"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void CanFocus(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.CanFocus,
						  "Property CanFocus is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.CanFocus"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotCanFocus(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.CanFocus,
						  "Property CanFocus is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.CanFocus"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCanFocusEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreCanFocusEqual(expected.CanFocus,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.CanFocus"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCanFocusEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.CanFocus,
						"Property CanFocus not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.CanSelect"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void CanSelect(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.CanSelect,
						  "Property CanSelect is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.CanSelect"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotCanSelect(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.CanSelect,
						  "Property CanSelect is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.CanSelect"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCanSelectEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreCanSelectEqual(expected.CanSelect,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.CanSelect"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCanSelectEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.CanSelect,
						"Property CanSelect not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Capture"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void Capture(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Capture,
						  "Property Capture is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.Capture"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotCapture(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.Capture,
						  "Property Capture is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Capture"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCaptureEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreCaptureEqual(expected.Capture,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Capture"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCaptureEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Capture,
						"Property Capture not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.CausesValidation"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void CausesValidation(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.CausesValidation,
						  "Property CausesValidation is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.CausesValidation"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotCausesValidation(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.CausesValidation,
						  "Property CausesValidation is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.CausesValidation"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCausesValidationEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreCausesValidationEqual(expected.CausesValidation,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.CausesValidation"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCausesValidationEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.CausesValidation,
						"Property CausesValidation not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.ClientRectangle"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreClientRectangleEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreClientRectangleEqual(expected.ClientRectangle,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.ClientRectangle"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreClientRectangleEqual(
			System.Drawing.Rectangle expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.ClientRectangle,
						"Property ClientRectangle not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.ClientSize"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreClientSizeEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreClientSizeEqual(expected.ClientSize,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.ClientSize"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreClientSizeEqual(
			System.Drawing.Size expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.ClientSize,
						"Property ClientSize not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.CompanyName"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCompanyNameEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreCompanyNameEqual(expected.CompanyName,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.CompanyName"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCompanyNameEqual(
			System.String expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.CompanyName);
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.ContainsFocus"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void ContainsFocus(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.ContainsFocus,
						  "Property ContainsFocus is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.ContainsFocus"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotContainsFocus(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.ContainsFocus,
						  "Property ContainsFocus is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.ContainsFocus"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreContainsFocusEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreContainsFocusEqual(expected.ContainsFocus,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.ContainsFocus"/>
        /// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreContainsFocusEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.ContainsFocus,
						"Property ContainsFocus not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.ContextMenu"/>
        /// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreContextMenuEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreContextMenuEqual(expected.ContextMenu,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.ContextMenu"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreContextMenuEqual(
			System.Windows.Forms.ContextMenu expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.ContextMenu,
						"Property ContextMenu not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Controls"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreControlsEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreControlsEqual(expected.Controls,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Controls"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreControlsEqual(
			System.Windows.Forms.Control.ControlCollection expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Controls);
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Created"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void Created(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Created,
						  "Property Created is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.Created"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotCreated(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.Created,
						  "Property Created is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Created"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCreatedEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreCreatedEqual(expected.Created,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Created"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCreatedEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Created,
						"Property Created not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Cursor"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCursorEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreCursorEqual(expected.Cursor,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Cursor"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreCursorEqual(
			System.Windows.Forms.Cursor expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Cursor,
						"Property Cursor not equal");
			
		}		

		/// <summary>
		/// Verifies that the property value <see cref="Control.DisplayRectangle"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreDisplayRectangleEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreDisplayRectangleEqual(expected.DisplayRectangle,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.DisplayRectangle"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreDisplayRectangleEqual(
			System.Drawing.Rectangle expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.DisplayRectangle,
						"Property DisplayRectangle not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.IsDisposed"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void IsDisposed(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.IsDisposed,
						  "Property IsDisposed is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.IsDisposed"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void IsNotisposed(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.IsDisposed,
						  "Property IsDisposed is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.IsDisposed"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreIsDisposedEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreIsDisposedEqual(expected.IsDisposed,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.IsDisposed"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreIsDisposedEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.IsDisposed,
						"Property IsDisposed not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Disposing"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void Disposing(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Disposing,
						  "Property Disposing is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.Disposing"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotDisposing(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.Disposing,
						  "Property Disposing is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Disposing"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreDisposingEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreDisposingEqual(expected.Disposing,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Disposing"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreDisposingEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Disposing,
						"Property Disposing not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Dock"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreDockEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreDockEqual(expected.Dock,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Dock"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreDockEqual(
			System.Windows.Forms.DockStyle expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Dock,
						"Property Dock not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Enabled"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void Enabled(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Enabled,
						  "Property Enabled is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.Enabled"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotEnabled(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.Enabled,
						  "Property Enabled is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Enabled"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreEnabledEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreEnabledEqual(expected.Enabled,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Enabled"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreEnabledEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Enabled,
						"Property Enabled not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Focused"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void Focused(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Focused,
						  "Property Focused is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.Focused"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotFocused(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.Focused,
						  "Property Focused is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Focused"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreFocusedEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreFocusedEqual(expected.Focused,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Focused"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreFocusedEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Focused,
						"Property Focused not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Font"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreFontEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreFontEqual(expected.Font,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Font"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreFontEqual(
			System.Drawing.Font expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Font,
						"Property Font not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.ForeColor"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreForeColorEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreForeColorEqual(expected.ForeColor,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.ForeColor"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreForeColorEqual(
			System.Drawing.Color expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.ForeColor,
						"Property ForeColor not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Handle"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreHandleEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreHandleEqual(expected.Handle,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Handle"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreHandleEqual(
			System.IntPtr expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Handle,
						"Property Handle not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.HasChildren"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void HasChildren(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.HasChildren,
						  "Property HasChildren is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.HasChildren"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotHasChildren(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.HasChildren,
						  "Property HasChildren is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.HasChildren"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreHasChildrenEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreHasChildrenEqual(expected.HasChildren,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.HasChildren"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreHasChildrenEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.HasChildren,
						"Property HasChildren not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Height"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreHeightEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreHeightEqual(expected.Height,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Height"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreHeightEqual(
			System.Int32 expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Height,
						"Property Height not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.IsHandleCreated"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void IsHandleCreated(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.IsHandleCreated,
						  "Property IsHandleCreated is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.IsHandleCreated"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void IsNotandleCreated(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.IsHandleCreated,
						  "Property IsHandleCreated is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.IsHandleCreated"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreIsHandleCreatedEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreIsHandleCreatedEqual(expected.IsHandleCreated,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.IsHandleCreated"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreIsHandleCreatedEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.IsHandleCreated,
						"Property IsHandleCreated not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.ImeMode"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreImeModeEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreImeModeEqual(expected.ImeMode,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.ImeMode"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreImeModeEqual(
			System.Windows.Forms.ImeMode expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.ImeMode,
						"Property ImeMode not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.InvokeRequired"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void InvokeRequired(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.InvokeRequired,
						  "Property InvokeRequired is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.InvokeRequired"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotInvokeRequired(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.InvokeRequired,
						  "Property InvokeRequired is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.InvokeRequired"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreInvokeRequiredEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreInvokeRequiredEqual(expected.InvokeRequired,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.InvokeRequired"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreInvokeRequiredEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.InvokeRequired,
						"Property InvokeRequired not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.IsAccessible"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void IsAccessible(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.IsAccessible,
						  "Property IsAccessible is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.IsAccessible"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void IsNotccessible(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.IsAccessible,
						  "Property IsAccessible is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.IsAccessible"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreIsAccessibleEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreIsAccessibleEqual(expected.IsAccessible,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.IsAccessible"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreIsAccessibleEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.IsAccessible,
						"Property IsAccessible not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Left"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreLeftEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreLeftEqual(expected.Left,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Left"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreLeftEqual(
			System.Int32 expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Left,
						"Property Left not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Location"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreLocationEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreLocationEqual(expected.Location,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Location"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreLocationEqual(
			System.Drawing.Point expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Location,
						"Property Location not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Name"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreNameEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreNameEqual(expected.Name,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Name"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreNameEqual(
			System.String expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Name);
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Parent"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreParentEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected.Parent,actual);
		}
			
		/// <summary>
		/// Verifies that the property value <see cref="Control.ProductName"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreProductNameEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreProductNameEqual(expected.ProductName,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.ProductName"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreProductNameEqual(
			System.String expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.ProductName);
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.ProductVersion"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreProductVersionEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreProductVersionEqual(expected.ProductVersion,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.ProductVersion"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreProductVersionEqual(
			System.String expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.ProductVersion);
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.RecreatingHandle"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void RecreatingHandle(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.RecreatingHandle,
						  "Property RecreatingHandle is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.RecreatingHandle"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotRecreatingHandle(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.RecreatingHandle,
						  "Property RecreatingHandle is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.RecreatingHandle"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreRecreatingHandleEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreRecreatingHandleEqual(expected.RecreatingHandle,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.RecreatingHandle"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreRecreatingHandleEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.RecreatingHandle,
						"Property RecreatingHandle not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Region"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreRegionEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreRegionEqual(expected.Region,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Region"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreRegionEqual(
			System.Drawing.Region expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Region,
						"Property Region not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Right"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreRightEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreRightEqual(expected.Right,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Right"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreRightEqual(
			System.Int32 expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Right,
						"Property Right not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.RightToLeft"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreRightToLeftEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreRightToLeftEqual(expected.RightToLeft,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.RightToLeft"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreRightToLeftEqual(
			System.Windows.Forms.RightToLeft expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.RightToLeft,
						"Property RightToLeft not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Site"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreSiteEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreSiteEqual(expected.Site,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Site"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreSiteEqual(
			System.ComponentModel.ISite expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Site,
						"Property Site not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Size"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreSizeEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreSizeEqual(expected.Size,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Size"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreSizeEqual(
			System.Drawing.Size expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Size,
						"Property Size not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.TabIndex"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreTabIndexEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreTabIndexEqual(expected.TabIndex,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.TabIndex"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreTabIndexEqual(
			System.Int32 expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.TabIndex,
						"Property TabIndex not equal");
			
		}		
		/// <summary>
		/// Verifies that the property value <see cref="Control.TabStop"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void TabStop(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.TabStop,
						  "Property TabStop is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.TabStop"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotTabStop(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.TabStop,
						  "Property TabStop is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.TabStop"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreTabStopEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreTabStopEqual(expected.TabStop,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.TabStop"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreTabStopEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.TabStop,
						"Property TabStop not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Tag"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreTagEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreTagEqual(expected.Tag,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Tag"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreTagEqual(
			System.Object expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Tag,
						"Property Tag not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Text"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreTextEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreTextEqual(expected.Text,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Text"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreTextEqual(
			System.String expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Text);
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Top"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreTopEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreTopEqual(expected.Top,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Top"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreTopEqual(
			System.Int32 expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Top,
						"Property Top not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.TopLevelControl"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreTopLevelControlEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected.TopLevelControl,actual);
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.Visible"/>
		/// is true.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void Visible(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsTrue(actual.Visible,
						  "Property Visible is false");
		}

		/// <summary>
		/// Verifies that the property value <see cref="Control.Visible"/>
		/// is false.
		/// </summary>
		/// <param name="actual">
		/// Instance containing the expected value.
		/// </param>
		public static void NotVisible(
			Control actual
			)
		{		
			Assert.IsNotNull(actual);
			Assert.IsFalse(actual.Visible,
						  "Property Visible is true");			
		}

	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Visible"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreVisibleEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreVisibleEqual(expected.Visible,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Visible"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreVisibleEqual(
			System.Boolean expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Visible,
						"Property Visible not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.Width"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreWidthEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreWidthEqual(expected.Width,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.Width"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreWidthEqual(
			System.Int32 expected,
			Control actual
			)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Width,
						"Property Width not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="Control.WindowTarget"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreWindowTargetEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreWindowTargetEqual(expected.WindowTarget,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="Control.WindowTarget"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreWindowTargetEqual(
			System.Windows.Forms.IWindowTarget expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.WindowTarget,
						"Property WindowTarget not equal");
			
		}		
	
		/// <summary>
		/// Verifies that the property value <see cref="System.ComponentModel.Component.Container"/>
		/// of <paramref name="expected"/> and <paramref name="actual"/> are equal.
		/// </summary>
		/// <param name="expected">
		/// Instance containing the expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreContainerEqual(
			Control expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreContainerEqual(expected.Container,actual);
		}
		
		/// <summary>
		/// Verifies that the property value <see cref="System.ComponentModel.Component.Container"/>
		/// of <paramref name="actual"/> is equal to <paramref name="expected"/>.
		/// </summary>
		/// <param name="expected">
		/// Expected value.
		/// </param>
		/// <param name="actual">
		/// Instance containing the tested value.
		/// </param>
		public static void AreContainerEqual(
			System.ComponentModel.IContainer expected,
			Control actual
			)
		{
			if (expected==null && actual==null)
                return;
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.Container,
						"Property Container not equal");
			
		}		
	}
}
