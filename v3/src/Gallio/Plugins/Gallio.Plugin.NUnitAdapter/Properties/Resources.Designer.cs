﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1378
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gallio.Plugin.NUnitAdapter.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gallio.Plugin.NUnitAdapter.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NUnit v{0}.
        /// </summary>
        internal static string NUnitFrameworkTemplate_FrameworkTemplateName {
            get {
                return ResourceManager.GetString("NUnitFrameworkTemplate_FrameworkTemplateName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot load one or more NUnit test assemblies..
        /// </summary>
        internal static string NUnitFrameworkTemplateBinding_CannotLoadNUnitTestAssemblies {
            get {
                return ResourceManager.GetString("NUnitFrameworkTemplateBinding_CannotLoadNUnitTestAssemblies", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The test controller has been disposed..
        /// </summary>
        internal static string NUnitTestController_ControllerWasDisposedException {
            get {
                return ResourceManager.GetString("NUnitTestController_ControllerWasDisposedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failure Message.
        /// </summary>
        internal static string NUnitTestController_FailureMessageSectionName {
            get {
                return ResourceManager.GetString("NUnitTestController_FailureMessageSectionName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failure Stack Trace.
        /// </summary>
        internal static string NUnitTestController_FailureStackTraceSectionName {
            get {
                return ResourceManager.GetString("NUnitTestController_FailureStackTraceSectionName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Running NUnit tests..
        /// </summary>
        internal static string NUnitTestController_RunningNUnitTests {
            get {
                return ResourceManager.GetString("NUnitTestController_RunningNUnitTests", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Run test: {0}..
        /// </summary>
        internal static string NUnitTestController_StatusMessages_RunningTest {
            get {
                return ResourceManager.GetString("NUnitTestController_StatusMessages_RunningTest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unhandled Exception.
        /// </summary>
        internal static string NUnitTestController_UnhandledExceptionSectionName {
            get {
                return ResourceManager.GetString("NUnitTestController_UnhandledExceptionSectionName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NUnit.
        /// </summary>
        internal static string NUnitTestFramework_NUnitFrameworkName {
            get {
                return ResourceManager.GetString("NUnitTestFramework_NUnitFrameworkName", resourceCulture);
            }
        }
    }
}
