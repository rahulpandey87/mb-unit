using System;
using System.Collections;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=true)]
    public sealed class SeedAttribute : Attribute
    {}
}
