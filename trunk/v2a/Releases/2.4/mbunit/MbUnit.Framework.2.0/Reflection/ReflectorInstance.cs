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
        /// Use this constructor if you plan to test default constructor of a non-public class.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        public Reflector(string assemblyName, string className)
            : this(assemblyName, className, null)
        {
        }

        /// <summary>
        /// Use this constructor if you plan to test constructor with arguments of a non-public class.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        public Reflector(string assemblyName, string className, params object[] args)
        {
            Assembly a = Assembly.Load(assemblyName);
            Type type = a.GetType(className);
            if (args != null)
            {
                Type[] argTypes = new Type[args.Length];
                for (int ndx = 0; ndx < args.Length; ndx++)
                    argTypes[ndx] = (args[ndx] == null) ? typeof(object) : args[ndx].GetType();

                ConstructorInfo ci = type.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                    , null, argTypes, null);
                _obj = ci.Invoke(args);
            }
            else
                _obj = Activator.CreateInstance(type);
        }

        /// <summary>
        /// Execute a NonPublic method without arguments on a object
        /// </summary>
        /// <param name="obj">Object to call method on</param>
        /// <param name="methodName">Method to call</param>
        /// <returns>The object the method should return.</returns>
        public object RunPrivateMethod(string methodName)
        {
            return RunPrivateMethod(methodName, null);
        }

        /// <summary>
        /// Execute a NonPublic method with arguments on a object
        /// </summary>
        /// <param name="obj">Object to call method on</param>
        /// <param name="methodName">Method to call</param>
        /// <returns>The object the method should return.</returns>
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

        /// <summary>
        /// Gets value of NonPublic field.
        /// </summary>
        /// <param name="fieldName">NonPublic field name</param>
        /// <returns>Field value</returns>
        public object GetNonPublicField(string fieldName)
        {
            FieldInfo fi = _obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            IsMember(fi, fieldName, MemberType.Field);
            return fi.GetValue(_obj);
        }

        /// <summary>
        /// Gets value of NonPublic property
        /// </summary>
        /// <param name="propName">Property name</param>
        /// <returns>Property value</returns>
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
