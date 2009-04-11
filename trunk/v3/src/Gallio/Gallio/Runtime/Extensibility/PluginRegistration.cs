﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gallio.Collections;
using Gallio.Reflection;

namespace Gallio.Runtime.Extensibility
{
    /// <summary>
    /// Provides information used to register a plugin.
    /// </summary>
    public class PluginRegistration
    {
        private string pluginId;
        private TypeName pluginTypeName;
        private DirectoryInfo baseDirectory;
        private PropertySet pluginProperties;
        private PropertySet traitsProperties;
        private IHandlerFactory pluginHandlerFactory;

        /// <summary>
        /// Creates a plugin registration.
        /// </summary>
        /// <param name="pluginId">The plugin id</param>
        /// <param name="pluginTypeName">The plugin type name</param>
        /// <param name="baseDirectory">The plugin base directory</param>
        /// <exception cref="ArgumentNullException">Thrown if <aramref name="pluginId"/>,
        /// <paramref name="pluginTypeName"/> or <paramref name="baseDirectory"/> is null</exception>
        public PluginRegistration(string pluginId, TypeName pluginTypeName, DirectoryInfo baseDirectory)
        {
            PluginId = pluginId;
            PluginTypeName = pluginTypeName;
            BaseDirectory = baseDirectory;
        }

        /// <summary>
        /// Gets or sets the plugin id.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
        public string PluginId
        {
            get { return pluginId; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                pluginId = value;
            }
        }

        /// <summary>
        /// Gets or sets the plugin type name.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
        public TypeName PluginTypeName
        {
            get { return pluginTypeName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                pluginTypeName = value;
            }
        }

        /// <summary>
        /// Gets or sets the base directory.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
        public DirectoryInfo BaseDirectory
        {
            get { return baseDirectory; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                baseDirectory = value;
            }
        }

        /// <summary>
        /// Gets or sets the plugin properties.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
        public PropertySet PluginProperties
        {
            get
            {
                if (pluginProperties == null)
                    pluginProperties = new PropertySet();
                return pluginProperties;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                pluginProperties = value;
            }
        }

        /// <summary>
        /// Gets or sets the traits properties.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
        public PropertySet TraitsProperties
        {
            get
            {
                if (traitsProperties == null)
                    traitsProperties = new PropertySet();
                return traitsProperties;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                traitsProperties = value;
            }
        }

        /// <summary>
        /// Gets or sets the plugin handler factory.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
        public IHandlerFactory PluginHandlerFactory
        {
            get
            {
                if (pluginHandlerFactory == null)
                    pluginHandlerFactory = new SingletonHandlerFactory();
                return pluginHandlerFactory;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                pluginHandlerFactory = value;
            }
        }
    }
}
