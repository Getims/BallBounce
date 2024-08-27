using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Main.Scripts.Core.Utilities
{
    public static partial class Utils
    {
        public static string GetUniqueID() =>
            Guid.NewGuid().ToString().Remove(0, 20);

        public static string GetUniqueID(int length)
        {
            string randomString = Guid.NewGuid().ToString();
            if (length < randomString.Length)
                randomString = randomString.Substring(0, length);
            return randomString;
        }
    }
}