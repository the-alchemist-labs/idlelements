using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;


public class DataService
{
    // move to safe place
    private const string KEY = "FAaeKAN0R9eiXi7B2H9mLwUz4qUhOB6x7D2XktjH90U=";
    private const string IV = "G4aMWxXw19wyUVUR9KEpKg==";

    public bool SaveData<T>(string fileName, T Data, bool shouldEncrypt)
    {
        string path = $"{Application.persistentDataPath}/{fileName}.json";

        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using FileStream stream = File.Create(path);
            if (shouldEncrypt)
            {
                WriteEncryptedData(Data, stream);
            }
            else
            {
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(Data));
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    private void WriteEncryptedData<T>(T Data, FileStream Stream)
    {
        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);
        using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
        using CryptoStream cryptoStream = new CryptoStream(
            Stream,
            cryptoTransform,
            CryptoStreamMode.Write
        );

        // You can uncomment the below to see a generated value for the IV & key.
        //Debug.Log($"Initialization Vector: {Convert.ToBase64String(aesProvider.IV)}");
        //Debug.Log($"Key: {Convert.ToBase64String(aesProvider.Key)}");
        cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Data)));
    }


    public T LoadData<T>(string fileName, bool isEncrypted)
    {
        string path = $"{Application.persistentDataPath}/{fileName}.json";

        if (!File.Exists(path))
        {
            Debug.LogError($"Cannot load file at {path}. File does not exist!");
            return default;
        }

        try
        {
            return isEncrypted ? ReadEncryptedData<T>(path) :JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            return default;
        }
    }

    private T ReadEncryptedData<T>(string Path)
    {
        byte[] fileBytes = File.ReadAllBytes(Path);
        using Aes aesProvider = Aes.Create();

        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);

        using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
            aesProvider.Key,
            aesProvider.IV
        );
        using MemoryStream decryptionStream = new MemoryStream(fileBytes);
        using CryptoStream cryptoStream = new CryptoStream(
            decryptionStream,
            cryptoTransform,
            CryptoStreamMode.Read
        );
        using StreamReader reader = new StreamReader(cryptoStream);

        string result = reader.ReadToEnd();

        Debug.Log($"Decrypted result (if the following is not legible, probably wrong key or iv): {result}");
        return JsonConvert.DeserializeObject<T>(result);
    }
}