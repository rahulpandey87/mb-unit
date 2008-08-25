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

using System;
using System.Web;
using System.Web.UI;

namespace MbUnit.Framework
{
	/// <summary>
    /// Class containing generic assert methods for <see cref="Control">web controls</see> and the <see cref="Page"/> object
	/// </summary>
	public sealed class WebAssert
	{
		#region Private constructor
		private WebAssert(){}
		#endregion
		
		#region Control assertions
        /// <summary>
        /// Verifies that <paramref name="ctrl"/> has ViewState enabled.
        /// </summary>
        /// <param name="ctrl">The <see cref="Control"/> to test.</param>
		public static void IsEnableViewState(Control ctrl)
		{
			Assert.IsNotNull(ctrl);
			Assert.IsTrue(ctrl.EnableViewState,
			              "Control {0} has ViewState disabled",
			              ctrl.ID);
		}

        /// <summary>
        /// Verifies that <paramref name="ctrl"/> has <strong>not</strong> ViewState enabled.
        /// </summary>
        /// <param name="ctrl">The <see cref="Control"/> to test.</param>
		public static void IsNotEnableViewState(Control ctrl)
		{
			Assert.IsNotNull(ctrl);
			Assert.IsFalse(ctrl.EnableViewState,
			              "Control {0} has ViewState enabled",
			              ctrl.ID);
		}

        /// <summary>
        /// Verifies that <paramref name="ctrl"/> is visible.
        /// </summary>
        /// <param name="ctrl">The <see cref="Control"/> to test.</param>
		public static void IsVisible(Control ctrl)
		{
			Assert.IsNotNull(ctrl);
			Assert.IsTrue(ctrl.Visible,
			              "Control {0} is not visible",
			              ctrl.ID);
		}

        /// <summary>
        /// Verifies that <paramref name="ctrl"/> is not visible.
        /// </summary>
        /// <param name="ctrl">The <see cref="Control"/> to test.</param>
		public static void IsNotVisible(Control ctrl)
		{
			Assert.IsNotNull(ctrl);
			Assert.IsFalse(ctrl.Visible,
			              "Control {0} is visible",
			              ctrl.ID);
		}

        /// <summary>
        /// Verifies that <paramref name="ctrl"/> ID is equal to <paramref name="id"/>.
        /// </summary>
        /// <param name="ctrl">The <see cref="Control"/> to test.</param>
        /// <param name="id">The expected ID</param>
		public static void IsIDEqual(Control ctrl, string id)
		{
			Assert.IsNotNull(ctrl);
			Assert.AreEqual(ctrl.ID,id,
			                "Control ID {0} not equal to {1}",
			                ctrl.ID,id);
		}

        /// <summary>
        /// Verifies that <paramref name="ctrl"/> has child controls.
        /// </summary>
        /// <param name="ctrl">The <see cref="Control"/> to test.</param>
		public static void HasControls(Control ctrl)
		{
			Assert.IsNotNull(ctrl);
			Assert.IsTrue(ctrl.HasControls(),
			              "Control {0} has no controls",
			              ctrl.ID);
		}

        /// <summary>
        /// Verifies that <paramref name="ctrl"/> has no child controls.
        /// </summary>
        /// <param name="ctrl">The <see cref="Control"/> to test.</param>
		public static void HasNoControls(Control ctrl)
		{
			Assert.IsNotNull(ctrl);
			Assert.IsFalse(ctrl.HasControls(),
			              "Control {0} has child controls",
			              ctrl.ID);
		}

        /// <summary>
        /// Verifies that the <see cref="Control.TemplateSourceDirectory"/>
        /// property of <paramref name="expected"/> and <paramref name="actual"/>
        /// are equal.
        /// </summary>
        /// <param name="expected">The <see cref="Control"/> to test against.</param>
        /// <param name="actual">The <see cref="Control"/> to test.</param>
		public static void AreTemplateSourceDirectoryEqual(Control expected, Control actual)
		{
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);
			AreTemplateSourceDirectoryEqual(expected.TemplateSourceDirectory,actual);			                
		}

        /// <summary>
        /// Verifies that the <see cref="Control.TemplateSourceDirectory"/>
        /// property of <paramref name="actual"/> is equal to <paramref name="expected"/>
        /// are equal.
        /// </summary>
        /// <param name="expected">The expected source directory.</param>
        /// <param name="actual">The <see cref="Control"/> to test.</param>
		public static void AreTemplateSourceDirectoryEqual(string expected, Control actual)
		{
			Assert.IsNotNull(actual);
			Assert.AreEqual(expected,actual.TemplateSourceDirectory,
			                "TemplateSourceDirectory not equal");			                
		}

        /// <summary>
        /// Verifies that <paramref name="child"/> is a child control
        /// of <paramref name="parent"/>
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="child">The child.</param>
		public static void IsChild(Control parent, Control child)
		{
			Assert.IsNotNull(parent);
			Assert.IsNotNull(child);
			IsChild(parent,child.ID);
		}

        /// <summary>
        /// Verifies that <paramref name="childID"/> is the ID of a child control
        /// of <paramref name="parent"/>
        /// </summary>
        /// <param name="parent">The parent control.</param>
        /// <param name="childID">The ID of the child control.</param>
		public static void IsChild(Control parent, string childID)
		{
			Assert.IsNotNull(parent);
			Assert.IsTrue(parent.FindControl(childID)!=null,
			              "Could not find {0} in control {1}",
			              childID,parent.ID);
		}

        /// <summary>
        /// Verifies that <paramref name="child"/> is a not child control
        /// of <paramref name="parent"/>
        /// </summary>
        /// <param name="parent">The parent <see cref="Control"/></param>
        /// <param name="child">The control that should not be the child of <paramref name="parent"/>.</param>
		public static void IsNotChild(Control parent, Control child)
		{
			Assert.IsNotNull(parent);
			Assert.IsNotNull(child);
			IsNotChild(parent,child.ID);
		}

        /// <summary>
        /// Verifies that <paramref name="childID"/> is the not ID of a child control
        /// of <paramref name="parent"/>
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="childID">The ID of the control that should not be the child of <paramref name="parent"/>.</param>
		public static void IsNotChild(Control parent, string childID)
		{
			Assert.IsNotNull(parent);
			Assert.IsTrue(parent.FindControl(childID)==null,
			              "Could not find {0} in control {1}",
			              childID,parent.ID);
		}		
		#endregion
		
		#region Page Assertions

        /// <summary>
        /// Verifies that the <see cref="Page.ErrorPage"/> property of <paramref name="page"/>
        /// is equal to <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="page">The <see cref="Page"/> to test.</param>
		public static void AreErrorPageEqual(string expected, Page page)
		{
			Assert.IsNotNull(page);
			Assert.AreEqual(expected,page.ErrorPage,
			                "Error page not equal");
		}


        /// <summary>
        /// Verifies that the <see cref="Page.ClientTarget"/> property of <paramref name="page"/>
        /// is equal to <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="page">The <see cref="Page"/> to test.</param>
		public static void AreClientTargetEqual(string expected, Page page)
		{
			Assert.IsNotNull(page);
			Assert.AreEqual(expected,page.ClientTarget,
			                "Error page not equal");
		}

        /// <summary>
        /// Verifies that the <see cref="Page.IsPostBack"/> property of <paramref name="page"/>
        /// is true.
        /// </summary>
        /// <param name="page">The <see cref="Page"/> to test.</param>
		public static void IsPostBack(Page page)
		{
			Assert.IsNotNull(page);
			Assert.IsTrue(page.IsPostBack,
			                "IsPostBack is false");
		}

        /// <summary>
        /// Verifies that the <see cref="Page.IsPostBack"/> property of <paramref name="page"/>
        /// is false.
        /// </summary>
        /// <param name="page">The <see cref="Page"/> to test.</param>
		public static void IsNotPostBack(Page page)
		{
			Assert.IsNotNull(page);
			Assert.IsFalse(page.IsPostBack,
			                "IsPostBack is true");
		}

        /// <summary>
        /// Verifies that the <see cref="Page.IsValid"/> property of <paramref name="page"/>
        /// is true.
        /// </summary>
        /// <param name="page">The <see cref="Page"/> to test.</param>
		public static void IsValid(Page page)
		{
			Assert.IsNotNull(page);
			Assert.IsTrue(page.IsValid,
			                "IsValid is false");
		}

        /// <summary>
        /// Verifies that the <see cref="Page.IsValid"/> property of <paramref name="page"/>
        /// is false.
        /// </summary>
        /// <param name="page">The <see cref="Page"/> to test.</param>
		public static void IsNotValid(Page page)
		{
			Assert.IsNotNull(page);
			Assert.IsFalse(page.IsValid,
			                "IsValid is true");
		}

        /// <summary>
        /// Verifies that the <see cref="Page.SmartNavigation"/> property of <paramref name="page"/>
        /// is true.
        /// </summary>
        /// <param name="page">The <see cref="Page"/> to test.</param>
		public static void IsSmartNavigation(Page page)
		{
			Assert.IsNotNull(page);
			Assert.IsTrue(page.SmartNavigation,
			                "SmartNavigation is false");
		}

        /// <summary>
        /// Verifies that the <see cref="Page.SmartNavigation"/> property of <paramref name="page"/>
        /// is false.
        /// </summary>
        /// <param name="page">The <see cref="Page"/> to test.</param>
		public static void IsNotSmartNavigation(Page page)
		{
			Assert.IsNotNull(page);
			Assert.IsFalse(page.SmartNavigation,
			                "SmartNavigation is true");
		}
		
		#endregion
	}
}
