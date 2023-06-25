using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using AJ.Generic.Tools;
namespace AJ.Generic.Extension
{
    public static class DataExtension
    {
        /// <summary>
        /// 创建本地文件夹。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        public static void CreateDataDirectory(this object obj, string path)
        {
            var di = new DirectoryInfo(path);
            try
            {
                if (di.Exists)
                {
                    return;
                }
                di.Create();
            }
            catch (Exception e)
            {
                Debug.Log("The process failed: {0}" + e.ToString());
            }
            finally { }
        }
        /// <summary>
        /// 验证当前文件夹是否存在。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ExistsDataDirectory(this object obj, string path)
        {
            var di = new DirectoryInfo(path);
            return di.Exists;
        }
        /// <summary>
        /// 验证当前文件是否存在。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ExistsDataFile(this object obj, string path)
        {
            return File.Exists(path);
        }
        /// <summary>
        /// 获取文件目录。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string[] GetDataFile(this object obj, string path, string suffix)
        {
            string[] files = null;
            try
            {
                var directoryInfo = new DirectoryInfo(path);
                FileInfo[] infos = directoryInfo.GetFiles(suffix).OrderBy(p => p.CreationTime).ToArray();
                var fileList = new List<string>();
                foreach (var info in infos)
                {
                    fileList.Add(info.FullName);
                }
                fileList.Reverse();
                files = fileList.ToArray();
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Debug.Log(dirNotFound.Message);
            }
            return files;
        }
        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="file"></param>
        public static void DeleteDataFile(this object obj, string file)
        {
            if (obj.ExistsDataFile(file))
            {
                File.Delete(file);
            }
        }
        /// <summary>
        /// 删除文件下全部文件。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        /// <param name="suffix"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string[] DeleteDataFile(this object obj, string path, string suffix, int amount)
        {
            string[] files = null;
            try
            {
                files = obj.GetDataFile(path, suffix);

                if (files.Length > amount)
                {
                    File.Delete(files[files.Length - 1]);
                    files = obj.GetDataFile(path, suffix);
                }
            }
            catch (DirectoryNotFoundException dirNotFound)
            {
                Debug.Log(dirNotFound.Message);
            }
            return files;
        }
        /// <summary>
        /// 文件目录组合。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string PathCombine(this object obj, params string[] paths)
        {
            var file = Path.Combine(paths);
            return file;
        }
        /// <summary>
        /// 保存数据到本地。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Saved<T>(this object obj, T data, string path) where T : class
        {
            var file = path;
            using (FileStream dataStream = File.Create(file))
            {
                using (StreamWriter writer = new StreamWriter(dataStream))
                {
                    var jsonString = JsonUtility.ToJson(data);
                    writer.Write(jsonString);
                }
            }
        }
        /// <summary>
        /// 保存数据到本地(加密)。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Saved<T>(this object obj, T data, string path, string key, string iv) where T : class
        {
            var file = path;
            using (AesManaged myAes = new AesManaged())
            {
                var byteKey = Convert.FromBase64String(key);
                var byteIV = Convert.FromBase64String(iv);
                myAes.Key = byteKey;
                myAes.IV = byteIV;
                using (FileStream dataStream = File.Create(file))
                {
                    dataStream.Write(myAes.IV, 0, myAes.IV.Length);
                    ICryptoTransform encryptor = myAes.CreateEncryptor(myAes.Key, myAes.IV);
                    using (CryptoStream cryptoStream = new CryptoStream(dataStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cryptoStream))
                        {
                            var jsonString = JsonUtility.ToJson(data);
                            writer.Write(jsonString);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 读取本地数据。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T Loaded<T>(this object obj, string path) where T : class
        {
            var file = path;
            var data = default(T);
            if (File.Exists(file))
            {
                using (FileStream dataStream = File.Open(file, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        string json = reader.ReadToEnd();
                        data = JsonUtility.FromJson<T>(json);
                    }
                }
            }
            return data;
        }
        /// <summary>
        /// 读取本地数据(加密)。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T Loaded<T>(this object obj, string path, string key, string iv) where T : class
        {
            var file = path;
            var data = default(T);
            if (File.Exists(file))
            {
                using (AesManaged myAes = new AesManaged())
                {
                    var byteKey = Convert.FromBase64String(key);
                    var byteIV = Convert.FromBase64String(iv);
                    myAes.Key = byteKey;
                    myAes.IV = byteIV;
                    using (FileStream dataStream = File.Open(file, FileMode.Open))
                    {
                        dataStream.Write(myAes.IV, 0, myAes.IV.Length);
                        ICryptoTransform encryptor = myAes.CreateDecryptor(myAes.Key, myAes.IV);
                        using (CryptoStream cryptoStream = new CryptoStream(dataStream, encryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader reader = new StreamReader(cryptoStream))
                            {
                                string json = reader.ReadToEnd();
                                data = JsonUtility.FromJson<T>(json);
                            }
                        }
                    }
                }
            }
            return data;
        }
    }
}