using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MbUnit.Framework.Reflection
{
    public enum MemberType { Method, Field, Property }

    public partial class Reflector
    {
        object _obj;

        /// <summary>
        /// Constructor for object
        /// </summary>
        /// <param name="obj">Object to be referred to in methods</param>
        public Reflector(object obj)
        {
            if (obj == null)
                throw new NullReferenceException("obj cannot be null.");
            _obj = obj;
        }

        /// <summary>
        /// Execute a NonPublic method on a object
        /// </summary>
        /// <param name="obj">Object to call method on</param>
        /// <param name="methodName">Method to call</param>
        /// <returns></returns>
        public object RunPrivateMethod(string methodName)
        {
            return RunPrivateMethod(methodName, null);
        }

        public object RunPrivateMethod(string methodName, params object[] methodParams)
        {
            Type[] paramTypes = null;

            if (methodParams != null)
            {
                paramTypes = new Type[methodParams.Length];

                for (int ndx = 0; ndx < methodParams.Length; ndx++)
                    paramTypes[ndx] = methodParams[ndx].GetType();
            }
            else
                paramTypes = new Type[0];

            MethodInfo mi = _obj.GetType().GetMethod(methodName
                    , BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance
                    , null, paramTypes, null);

            IsMember(mi, methodName, MemberType.Method);
            return mi.Invoke(_obj, methodParams);
        }

        public object GetNonPublicField(string fieldName)
        {
            FieldInfo fi = _obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            IsMember(fi, fieldName, MemberType.Field);
            return fi.GetValue(_obj);
        }

        public object GetNonPublicProperty(string propName)
        {
            PropertyInfo pi = _obj.GetType().GetProperty(propName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            IsMember(pi, propName, MemberType.Property);
            return pi.GetValue(_obj, null);
        }

        /// <summary>
        /// Get the value from a NonPublic variable or field.
        /// </summary>
        /// <param name="obj">Object which contains field</param>
        /// <param name="variableName">Field Name</param>
        /// <returns></returns>
        [Obsolete("Use GetNonPublicField instead")]
        public object GetPrivateVariable(string variableName)
        {
            Type objType = _obj.GetType();

            FieldInfo variableInfo = objType.GetField(variableName, BindingFlags.NonPublic |
                                                                        BindingFlags.Instance |
                                                                        BindingFlags.Public |
                                                                        BindingFlags.Static);

            return variableInfo.GetValue(_obj);
        }

        private void IsMember(object member, string memberName, MemberType type)
        {
            if (member == null)
                throw new ReflectionException(memberName, type, _obj);
        }
    }
}
