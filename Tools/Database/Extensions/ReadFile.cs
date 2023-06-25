using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace AJ.Generic.Tools
{
    public static class ReadFile
    {
        public static string[][] GetCSVData(string path)
        {
            TextAsset csv = Resources.Load<TextAsset>(path);

            string[] data = csv.text.Split(new char[] { '\n' });

            string[][] Array = new string[data.Length][];

            for (int i = 0; i < data.Length; i++)
            {
                Array[i] = data[i].Split(new char[] { ',' });
            }
            return Array;
        }
    }
}
