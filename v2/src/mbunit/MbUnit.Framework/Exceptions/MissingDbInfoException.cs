
using System;
using System.Runtime.Serialization;
using System.IO;

namespace MbUnit.Framework.Exceptions
{
    /// <summary>
	/// Thrown if MbUnit cannot find <see cref="DbRestoreInfoAttribute"/> when required to restore 
	/// a database from its backup.
    /// </summary>
    [Serializable]
    public class MissingDbInfoException : Exception
    {
        private Type type;
        public MissingDbInfoException(Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// Creates an exception with a type
        /// and an inner exception.
        /// </summary>
        /// <param name="type">Error type</param>
        /// <param name="ex">Inner exception</param>
        public MissingDbInfoException(Type type, Exception ex)
			:base("",ex)
        {
            this.type = type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctx"></param>
        protected MissingDbInfoException(SerializationInfo info, StreamingContext ctx)
			:base(info,ctx)
        {}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Message
        {
            get
			{
                if (type!=null)
    				return String.Format("Type {0} missing DbInfo",type.GetType().FullName);
                else
                    return "Missing DbInfo";
			}
		}
        }
    }
