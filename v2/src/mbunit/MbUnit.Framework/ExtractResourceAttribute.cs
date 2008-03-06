using System;
using System.Collections;
using System.IO;
using System.Reflection;

using MbUnit.Core.Exceptions;
using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework
{
	/// <summary>
	/// <p>Test methods annotated with this attribute will have the 
	/// specified embedded resource extracted.
	/// </p>
	/// </summary>
	/// <remarks>
	/// <p>For example:</p>
	/// <code>
	/// [Test]
	///	[ExtractResource("MyAssembly.Test.txt", "Test.txt")]
	/// public void SomeTest()
	/// {
	///   Assert.IsTrue(File.Exists("Test.txt"));
	/// }
	/// </code>
	/// <p>It's possible to extract the resource into a stream as well by not 
	/// specifying a destination file.</p>
	/// <code>
	/// [Test]
	///	[ExtractResource("MyAssembly.Test.txt")]
	/// public void SomeOtherTest()
	/// {
	///   Assert.IsNotNull(ExtractResourceAttribute.Stream);
	/// }
	/// </code>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ExtractResourceAttribute : DecoratorPatternAttribute
	{
        private string resourceName;
        private string destination;
        private ResourceCleanup resourceCleanup;
        [ThreadStatic]
        private static Stream stream;
        private Type type;

        /// <summary>
        /// The full name of the resource. Use Reflector to find this out 
        /// if you don't know.
        /// </summary>
        public string ResourceName
        {
            get { return this.resourceName; }
            set { this.resourceName = value; }
        }

        /// <summary>
        /// The destination file to write the resource to. 
        /// Should be a path.
        /// </summary>
        public string Destination
        {
            get { return this.destination; }
            set { this.destination = value; }
        }

        /// <summary>
        /// Whether or not to cleanup the resource.
        /// </summary>
        public ResourceCleanup ResourceCleanup
        {
            get { return this.resourceCleanup; }
            set { this.resourceCleanup = value; }
        }

        /// <summary>
        /// The current resource stream if using the attribute without specifying 
        /// a destination.
        /// </summary>
        public static Stream Stream
        {
            get { return stream; }
        }

        /// <summary>
        /// The type within the assembly that contains the embedded resource.
        /// </summary>
        public Type Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

		/// <summary>
		/// Extracts the resource to a stream. Access the stream like so: <see cref="ExtractResourceAttribute.Stream" />.
		/// </summary>
		/// <param name="resourceName"></param>
		public ExtractResourceAttribute(string resourceName)
			: this(resourceName, (Type)null)
		{
		}

		/// <summary>
		/// Extracts the resource to a stream.
		/// </summary>
		/// <param name="resourceName"></param>
		/// <param name="type">Any type in the assembly where the resource is embedded.</param>
		public ExtractResourceAttribute(string resourceName, Type type)
		{
			this.resourceName = resourceName;
			this.type = type;
		}

		/// <summary>
		/// Extracts the specified resource to the destination. 
		/// The destination should be a file name. Will attempt to cleanup resource 
		/// after the test is complete.
		/// </summary>
		/// <param name="resourceName">The full name of the embedded resource. Use reflector or ILDasm if you're unsure.</param>
		/// <param name="destination">The filename or file path where the embedded resource should be extracted to.</param>
		public ExtractResourceAttribute(string resourceName, string destination)
			: this(resourceName, destination, ResourceCleanup.DeleteAfterTest, null)
		{
		}

		/// <summary>
		/// Extracts the specified resource to the destination. 
		/// The destination should be a file name.
		/// </summary>
		/// <param name="resourceName">The full name of the embedded resource. Use reflector or ILDasm if you're unsure.</param>
		/// <param name="destination">The filename or file path where the embedded resource should be extracted to.</param>
		/// <param name="cleanupOptions">Whether or not to try and cleanup the resource at the end</param>
		public ExtractResourceAttribute(string resourceName, string destination, ResourceCleanup cleanupOptions)
			: this(resourceName, destination, cleanupOptions, null)
		{
		}

		/// <summary>
		/// Extracts the specified resource to the destination. 
		/// The destination should be a file name.
		/// </summary>
		/// <param name="resourceName">The full name of the embedded resource. Use reflector or ILDasm if you're unsure.</param>
		/// <param name="destination">The filename or file path where the embedded resource should be extracted to.</param>
		/// <param name="cleanupOptions">Whether or not to cleanup the extracted resource after the test.</param>
		/// <param name="type">Any type in the assembly where the resource is embedded.</param>
		public ExtractResourceAttribute(string resourceName, string destination, ResourceCleanup cleanupOptions, Type type)
		{
			this.type = type;
			this.resourceName = resourceName;
			this.destination = destination;
			this.resourceCleanup = cleanupOptions;
		}

		public override IRunInvoker GetInvoker(IRunInvoker invoker)
		{
			return new ExtractResourceRunInvoker(invoker, this);
		}

		private sealed class ExtractResourceRunInvoker : DecoratorRunInvoker
		{
			private ExtractResourceAttribute attribute;

			public ExtractResourceRunInvoker(IRunInvoker invoker, ExtractResourceAttribute attribute)
				: base(invoker)
			{
                if (attribute.Type == null)
                {
                    MethodRunInvoker methodInvoker = invoker as MethodRunInvoker;
                    if (methodInvoker != null)
                        attribute.Type = methodInvoker.Method.DeclaringType;
                }
				this.attribute = attribute;
			}

			public override object Execute(object o, IList args)
			{
				Assembly assembly = attribute.Type.Assembly;

				using (Stream stream = assembly.GetManifestResourceStream(attribute.ResourceName))
				{
                    if (stream == null)
                        throw new MissingResourceException(attribute.ResourceName, assembly.FullName);

					if (attribute.Destination == null || attribute.Destination == string.Empty)
					{
						ExtractResourceAttribute.stream = stream;
						return this.Invoker.Execute(o, args);
					}
					else
						WriteResourceToFile(stream);
				}

				try
				{
					return this.Invoker.Execute(o, args);
				}
				finally
				{
					if (attribute.ResourceCleanup == ResourceCleanup.DeleteAfterTest)
						File.Delete(attribute.Destination);
				}
			}

			private void WriteResourceToFile(Stream stream)
			{
				using (StreamWriter outfile = File.CreateText(attribute.Destination))
				{
					using (StreamReader infile = new StreamReader(stream))
					{
						outfile.Write(infile.ReadToEnd());
					}
				}
			}
		}
	}

	/// <summary>
	/// Used to specify whether or not the test should 
	/// delete the extracted resource when the test is complete.
	/// </summary>
	public enum ResourceCleanup
	{
		/// <summary>Do not delete the extracted resource</summary>
		NoCleanup = 0,
		/// <summary>Delete the extracted resource after the test.</summary>
		DeleteAfterTest = 1,
	}
}
