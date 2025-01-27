using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace CodeBase.Core.Services.SaveService
{
    public class SaveService : ISaveService
    {
        private static readonly string SavePath = Application.persistentDataPath + "/save.json";
        private static readonly string EncryptionKey = "aXzVm88s444FElFrwMXysDQGdnlRJzmQ";

        private readonly JsonSerializerSettings _settings;

        public SaveService()
        {
            _settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public async UniTask Save<T>(T data)
        {
            string json = JsonConvert.SerializeObject(data, _settings);
            string encryptedJson = Encrypt(json);
            await File.WriteAllTextAsync(SavePath, encryptedJson);
        }

        public async UniTask<T> Load<T>() where T : new()
        {
            if (!File.Exists(SavePath))
                return new T();

            string encryptedJson = await File.ReadAllTextAsync(SavePath);
            string json = Decrypt(encryptedJson);
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        private string Encrypt(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aes.IV = new byte[16];
            using ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }

        private string Decrypt(string encryptedText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
            aes.IV = new byte[16];

            using ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
