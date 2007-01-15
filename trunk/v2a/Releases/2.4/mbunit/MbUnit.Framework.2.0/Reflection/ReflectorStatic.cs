using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MbUnit.Framework.Reflection
{
    public partial class Reflector
    {
        /// <summary>
        /// Execute a NonPublic method on a object
        /// </summary>
        /// <param name="obj">Object to call method on</param>
        /// <param name="methodName">Method to call</param>
        /// <returns></returns>
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
        public static object GetNonPublicVariable(object obj, string variableName)
        {
            Type objType = obj.GetType();

            FieldInfo variableInfo = objType.GetField(variableName, BindingFlags.NonPublic |
                                                                        BindingFlags.Instance |
                                                                        BindingFlags.Public |
                                                                        BindingFlags.Static);

            return variableInfo.GetValue(obj);
        }
    }
}
