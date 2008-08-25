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
using System.ComponentModel;
using System.Reflection;
using MbUnit.Core;

namespace MbUnit.Framework {
    /// <summary>
    /// Tags fixture class, test method or method property with a 'postit' from an author
    /// regarding that class, method or property. 
    /// </summary>
    /// <remarks>This attribute currently does nothing and is replaced in MbUnit v3 by the
    /// [Description] and [Metadata] attributes as well as XML documents which can be used to embed 
    /// extra information</remarks>
    /// <example>
    /// <para>This example demonstrates postits attached to a test fixture and method. 
    /// The <see cref="PelikhanAttribute"/> class is derived from <see cref="AuthorAttribute"/>. </para>
    ///     [TestFixture]
    ///     [PostIt("This is a test fixture", typeof(PelikhanAttribute))]
    ///     public class PostIt {
    /// 
    ///         [Test]
    ///         [PostIt("This is a test method", typeof(PelikhanAttribute))]
    ///         public void IsTrue_True() {
    ///             Assert.IsTrue(true, "This test failed at {0}", DateTime.Now.ToShortDateString());
    ///         }
    ///     }
    /// </example>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class PostItAttribute : InformationAttribute {
        private string message;
        private AuthorAttribute author;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostItAttribute"/> class.
        /// </summary>
        /// <param name="message">The message to attach to the tagged object.</param>
        /// <param name="author">An <see cref="AuthorAttribute"/> class representing the author of the message</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> or <paramref name="author"/>
        /// is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="author"/> is not derived from <see cref="AuthorAttribute"/></exception>
        public PostItAttribute(string message, Type author) {
            if (message == null)
                throw new ArgumentNullException("message");
            if (author == null)
                throw new ArgumentNullException("author");
            if (!typeof(AuthorAttribute).IsAssignableFrom(author.GetType()))
                throw new ArgumentException("author must derive from AuthorAttribute");
            this.message = message;

            ConstructorInfo ci = TypeHelper.GetConstructor(author, Type.EmptyTypes);
            this.author = (AuthorAttribute)ci.Invoke(null);
        }

        /// <summary>
        /// Gets the message to be attached.
        /// </summary>
        /// <value>The message.</value>
        [Category("Data")]
        public String Message {
            get {
                return this.message;
            }
        }

        /// <summary>
        /// Gets the author of the message as an <see cref="AuthorAttribute"/>.
        /// </summary>
        /// <value>The author of the message.</value>
        [Category("Data")]
        public AuthorAttribute Author {
            get {
                return this.author;
            }
        }

        /// <summary>
        /// Gets the type of the class representing the author.
        /// </summary>
        /// <value>The class representing the author.</value>
        [Browsable(false)]
        public Type AuthorType {
            get {
                return this.author.GetType();
            }
        }

    }
}
