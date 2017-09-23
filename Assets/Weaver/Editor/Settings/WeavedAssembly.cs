﻿using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Weaver
{
    /// <summary>
    /// Keeps track of the assembly path and if the
    /// weaving is enabled or not.
    /// </summary>
    [Serializable]
    public class WeavedAssembly
    {
        public delegate void WeavedAssemblyDelegate(WeavedAssembly weavedAssembly);

        [SerializeField]
        private string m_RelativePath;
        [SerializeField]
        private bool m_Enabled;
        [SerializeField]
        private int m_LastWriteTime;

        private bool m_IsValid;

        /// <summary>
        /// Returns back true if the assembly is
        /// valid and false if it's not. 
        /// </summary>
        public bool isValid
        {
            get { return m_IsValid; }
        }

        /// <summary>
        /// Returns back the file path to this assembly
        /// </summary>
        public string relativePath
        {
            get { return m_RelativePath; }
            set { m_RelativePath = value; }
        }

        /// <summary>
        /// Returns true if this assembly should be modified
        /// by Weaver or not. 
        /// </summary>
        public bool enabled
        {
            get { return m_Enabled; }
            set { m_Enabled = value; }
        }

        /// <summary>
        /// Returns back the system path to this
        /// assembly. 
        /// </summary>
        public string GetSystemPath()
        {
            // Get our path
            string path = Application.dataPath;
            // Get the length
            int pathLength = path.Length;
            // Split it
            path = path.Substring(0, pathLength - /* Assets */ 6);
            // Add our relative path
            path = Path.Combine(path, relativePath);
            // Return the result
            return path;
        }

        /// <summary>
        /// Sees if this file has been modified since the last time we checked.
        /// </summary>
        /// <returns></returns>
        public bool HasChanges()
        {
            if (File.Exists(relativePath))
            {
                m_IsValid = true;
                int writeTime = File.GetLastWriteTime(relativePath).Second;
                if (m_LastWriteTime != writeTime)
                {
                    m_LastWriteTime = writeTime;
                    return true; 
                }
            }
            else
            {
                m_IsValid = false;
            }
            return false;
        }
    }
}
