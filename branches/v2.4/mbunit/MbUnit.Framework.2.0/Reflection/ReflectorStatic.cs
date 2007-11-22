using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace MbUnit.Framework.Reflection
{
    public partial class Reflector
    {
        /// <summary>
        /// Create Instance
        /// </summary>
        /// <param name="assemblyName">Full assembly path.</param>
        /// <param name="className">Type Name such as (System.String)</param>
        /// <returns>Newly created object.</returns>
        public static object CreateInstance(string assemblyName, string typeName)
        {
            return CreateInstance(assemblyName, typeName, null);
        }

        /// <summary>
        /// Create Instance
        /// </summary>
        /// <param name="assemblyName">Full assembly path.</param>
        /// <param name="className">Type Name such as (System.String)</param>
        /// <param name="args">Constructor parameters.</param>
        /// <returns>Newly created object.</returns>
        public static object CreateInstance(string assemblyName, string typeName, params object[] args)
        {
            object obj = null;
            Type[] argTypes = Type.EmptyTypes;
            Assembly a = Assembly.Load(assemblyName);
            Type type = a.GetType(typeName);
            if (args != null)
            {
                argTypes = new Type[args.Length];
                for (int ndx = 0; ndx < args.Length; ndx++)
                    argTypes[ndx] = (args[ndx] == null) ? typeof(object) : args[ndx].GetType();
            }
            ConstructorInfo ci = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                , null, argTypes, null);
            obj = ci.Invoke(args);
            return obj;
        }

        /// <summary>
        /// Get public, non-public, or static field value.
        /// </summary>
        /// <param name="obj">Object where field is defined.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Field value</returns>
        public static object GetField(object obj, string fieldName)
        {
            return GetField(AccessModifier.Default, obj, fieldName);
        }

        /// <summary>
        /// Get field value.
        /// </summary>
        /// <param name="access">Specify field access modifier.</param>
        /// <param name="obj">Object where field is defined.</param>
        /// <param name="fieldName">Field name.</param>
        /// <returns>Field value</returns>
        public static object GetField(AccessModifier access, object obj, string fieldName)
        {
            CheckObject(obj);
            FieldInfo fi = obj.GetType().GetField(fieldName, BindingFlags.Instance | (BindingFlags)access);
            IsMember(obj, fi, fieldName, MemberType.Field);
            return fi.GetValue(obj);
        }

        /// <summary>
        /// Set field value.
        /// </summary>
        /// <param name="obj">Object where field is defined.</param>
        /// <param name="fieldName">Field Name.</param>
        /// <param name="fieldValue">Field Value.</param>
        public static void SetField(object obj, string fieldName, object fieldValue)
        {
            SetField(AccessModifier.Default, obj, fieldName, fieldValue);
        }

        /// <summary>
        /// Set field value.
        /// </summary>
        /// <param name="access">Specify field access modifier.</param>
        /// <param name="obj">Object where field is defined.</param>
        /// <param name="fieldName">Field Name.</param>
        /// <param name="fieldValue">Field Value.</param>
        public static void SetField(AccessModifier access, object obj, string fieldName, object fieldValue)
        {
            CheckObject(obj);
            FieldInfo fi = obj.GetType().GetField(fieldName, BindingFlags.Instance | (BindingFlags)access);
            IsMember(obj, fi, fieldName, MemberType.Field);
            fi.SetValue(obj, fieldValue);
        }

        /// <summary>
        /// Get Property Value
        /// </summary>
        /// <param name="obj">Object where property is defined.</param>
        /// <param name="propertyName">Property Name.</param>
        /// <returns>Property Value.</returns>
        public static object GetProperty(object obj, string propertyName)
        {
            return GetProperty(AccessModifier.Default, obj, propertyName);
        }

        /// <summary>
        /// Get Property Value
        /// </summary>
        /// <param name="access">Specify property access modifier.</param>
        /// <param name="propertyName">Property Name.</param>
        /// <returns>Property Value.</returns>
        public static object GetProperty(AccessModifier access, object obj, string propertyName)
        {
            CheckObject(obj);
            PropertyInfo pi = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | (BindingFlags)access);
            IsMember(obj, pi, propertyName, MemberType.Property);
            return pi.GetValue(obj, null);
        }

        /// <summary>
        /// Set Property value.
        /// </summary>
        /// <param name="obj">Object where property is defined.</param>
        /// <param name="fieldName">Property Name.</param>
        /// <param name="fieldValue">Property Value.</param>
        public static void SetProperty(object obj, string propertyName, object propertyValue)
        {
            SetProperty(AccessModifier.Default, obj, propertyName, propertyValue);
        }

        /// <summary>
        /// Set Property value.
        /// </summary>
        /// <param name="access">Specify property access modifier.</param>
        /// <param name="obj">Object where property is defined.</param>
        /// <param name="fieldName">Property Name.</param>
        /// <param name="fieldValue">Property Value.</param>
        public static void SetProperty(AccessModifier access, object obj, string propertyName, object propertyValue)
        {
            CheckObject(obj);
            PropertyInfo pi = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | (BindingFlags)access);
            IsMember(obj, pi, propertyName, MemberType.Property);
            pi.SetValue(obj, propertyValue, null);
        }

        /// <summary>
        /// Execute a NonPublic method with arguments on a object
        /// </summary>
        /// <param name="access">Specify method access modifier.</param>
        /// <param name="obj">Object where method is defined.</param>
        /// <param name="methodName">Method to call</param>
        /// <param name="methodParams">Method's parameters.</param>
        /// <returns>The object the method should return.</returns>
        public static object InvokeMethod(AccessModifier access, object obj, string methodName, params object[] methodParams)
        {
            CheckObject(obj);
            Type[] paramTypes = null;

            if (methodParams != null)
            {
                paramTypes = new Type[methodParams.Length];

                for (int ndx = 0; ndx < methodParams.Length; ndx++)
                    paramTypes[ndx] = methodParams[ndx].GetType();
            }
            else
                paramTypes = new Type[0];

            MethodInfo mi = obj.GetType().GetMethod(methodName
                    , BindingFlags.Instance | (BindingFlags)access
                    , null, paramTypes, null);

            IsMember(obj, mi, methodName, MemberType.Method);
            return mi.Invoke(obj, methodParams);
        }

        private static void IsMember(object obj, object member, string memberName, MemberType type)
        {
            if (member == null)
                throw new ReflectionException(memberName, type, obj);
        }

        private static void CheckObject(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj cannot be null.");
        }

        #region Obsolete Members
        /// <summary>
        /// Execute a NonPublic method on a object
        /// </summary>
        /// <param name="obj">Object to call method on</param>
        /// <param name="methodName">Method to call</param>
        /// <returns></returns>
        [Obsolete("Use InvokeMethod instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static object RunNonPublicMethod(object obj, string methodName)
        {
            Type objType = obj.GetType();

            MethodInfo methodRequired = objType.GetMethod(methodName, BindingFlags.NonPublic |
                                                                        BindingFlags.Instance |
                                                                        BindingFlags.Public |
                                                                        BindingFlags.Static);

            return methodRequired.Invoke(obj, null);
        }

        /// <summary>
        /// Get the value from a NonPublic variable or field.
        /// </summary>
        /// <param name="obj">Object which contains field</param>
        /// <param name="variableName">Field Name</param>
        /// <returns></returns>
        [Obsolete("Use GetField instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static object GetNonPublicVariable(object obj, string variableName)
        {
            Type objType = obj.GetType();

            FieldInfo variableInfo = objType.GetField(variableName, BindingFlags.NonPublic |
                                                                        BindingFlags.Instance |
                                                                        BindingFlags.Public |
                                                                        BindingFlags.Static);

            return variableInfo.GetValue(obj);
        }
        #endregion
    }
}
