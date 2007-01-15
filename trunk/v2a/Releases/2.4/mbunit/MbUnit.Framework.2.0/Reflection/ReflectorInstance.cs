using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MbUnit.Framework.Reflection
{
    public partial class Reflector
    {
        object obj;

        /// <summary>
        /// Constructor for object
        /// </summary>
        /// <param name="obj">Object to be referred to in methods</param>
        public Reflector(object obj)
        {
            this.obj = obj;
        }

        /// <summary>
        /// Execute a NonPublic method on a object
        /// </summary>
        /// <param name="obj">Object to call method on</param>
        /// <param name="methodName">Method to call</param>
        /// <returns></returns>
        public object RunPrivateMethod(string methodName)
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
        public object GetPrivateVariable(string variableName)
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
