using System;
using System.Reflection;
using System.Xml;

namespace MbUnit.Core.Invokers
{
	using MbUnit.Core;
	using MbUnit.Framework;
	using MbUnit.Core.Invokers;
	using MbUnit.Core.Runs;

	/// <summary>
	/// Summary description for ForEachTestRunInvoker.
	/// </summary>
	internal sealed class ForEachTestRunInvoker : MethodRunInvoker
	{
		private ForEachTestAttribute attribute;
		private XmlNode node;
        private object deserializedNode = null;

        public ForEachTestRunInvoker(
				IRun generator, 
				MethodInfo mi, 
				ForEachTestAttribute attribute,
				XmlNode node
			)
			:base(generator,mi)
		{
			if (node==null)
				throw new ArgumentNullException("node");
			if (attribute==null)
				throw new ArgumentNullException("attribute");
			this.node =node;
			this.attribute =attribute;
		}

		public XmlNode Node
		{
			get
			{
				return this.node;
			}
		}

        public object DeserializedNode
        {
            get
            {
                if (this.deserializedNode==null)
                {
                    if (this.Attribute.IsDeserialized)
                        this.deserializedNode = this.Attribute.Deserialize(this.node);
                    else
                        this.deserializedNode = this.node;
                }
                return this.deserializedNode;
            }
        }

        public ForEachTestAttribute Attribute
		{
			get
			{
				return this.attribute;
			}
		}

		public override object Execute(object fixture, System.Collections.IList args)
		{
            if (fixture == null)
                throw new ArgumentNullException("fixture");
            if (this.DeserializedNode == null)
                throw new ArgumentException("Error while deserializing node", "XmlNode");
            args.Add(this.DeserializedNode);
			return base.Execute(fixture,args);
		}

		public override string Name
		{
			get
			{
				return String.Format("ForEach({0},{1})",
					this.Method.Name,
					this.Node.Name);
			}
		}

	}
}
