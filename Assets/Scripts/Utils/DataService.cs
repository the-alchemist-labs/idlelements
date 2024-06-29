using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public sealed class DataService
{
    private static DataService instance = null;
    private static readonly object padlock = new object();
    private static bool isEncrypted = Conts.IsEncrypted;

    // move to safe place
    private const string KEY = "FAaeKAN0R9eiXi7B2H9mLwUz4qUhOB6x7D2XktjH90U=";
    private const string IV = "G4aMWxXw19wyUVUR9KEpKg==";

    private DataService() { }

    public static DataService Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new DataService();
                }
                return instance;
            }
        }
    }

    public bool SaveData<T>(string fileName, bool isPersistent, T Data)
    {
        string path = GetPath(fileName, isPersistent);
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using FileStream stream = File.Create(path);
            if (isEncrypted)
            {
                WriteEncryptedData(Data, stream);
            }
            else
            {
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(Data, Formatting.Indented));
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

        cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Data)));
    }

    public T LoadData<T>(string fileName, bool isPersistent)
    {
        string path = GetPath(fileName, isPersistent);

        if (!File.Exists(path))
        {
            Debug.LogError($"Cannot load file at {path}. File does not exist!");
            return default;
        }

        try
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new MyEnumConverter() }
            };

            return isEncrypted ? ReadEncryptedData<T>(path) : JsonConvert.DeserializeObject<T>(File.ReadAllText(path), settings);
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

        Debug.LogWarning($"Decrypted result (if the following is not legible, probably wrong key or iv): {result}");
        return JsonConvert.DeserializeObject<T>(result);
    }

    private static string GetPath(string fileName, bool isPersistent)
    {
        return Path.Combine(isPersistent ? Application.persistentDataPath : Application.streamingAssetsPath, $"{fileName}.json");
    }
}

public class MyEnumConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ElementalId) || objectType == typeof(ElementalId?);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
        }
        else
        {
            writer.WriteValue((int)value);
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonToken.Integer)
        {
            return (ElementalId)Enum.ToObject(typeof(ElementalId), reader.Value);
        }

        throw new JsonSerializationException("Unexpected token type: " + reader.TokenType);
    }
}