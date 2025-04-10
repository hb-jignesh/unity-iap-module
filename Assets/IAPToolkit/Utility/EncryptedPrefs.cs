using UnityEngine;
using System.Text;

namespace IAPToolkit
{
    public static class EncryptedPrefs
    {
        private static readonly string key = "super_secret_key_123!";

        public static void SetString(string keyName, string value)
        {
            PlayerPrefs.SetString(keyName, Encrypt(value));
        }

        public static string GetString(string keyName, string defaultValue = "")
        {
            var encrypted = PlayerPrefs.GetString(keyName, null);
            return string.IsNullOrEmpty(encrypted) ? defaultValue : Decrypt(encrypted);
        }

        public static void SetBool(string keyName, bool value)
        {
            SetString(keyName, value ? "1" : "0");
        }

        public static bool GetBool(string keyName)
        {
            return GetString(keyName) == "1";
        }

        public static bool HasKey(string keyName) => PlayerPrefs.HasKey(keyName);

        private static string Encrypt(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] ^= (byte)key[i % key.Length];
            return System.Convert.ToBase64String(bytes);
        }

        private static string Decrypt(string encryptedData)
        {
            byte[] bytes = System.Convert.FromBase64String(encryptedData);
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] ^= (byte)key[i % key.Length];
            return Encoding.UTF8.GetString(bytes);
        }
    }
}