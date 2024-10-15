using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class Data
{
    public string Name;
    public float Height;

    [JsonProperty] // 이러면 private 인자도 직렬화/역직렬화됨
    private string secret;

    public Data(string name, float height, string secret)
    {
        Name = name;
        Height = height;
        this.secret = secret;
    }

    public override string ToString()
    {
        return Name + " " + Height + " " + secret;
    }
}

public class JsonTest : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Data charles = new Data("철수", 200, "정상");

        string json1 = JsonConvert.SerializeObject(charles);
        Debug.Log(json1);

        Data aMan = JsonConvert.DeserializeObject<Data>(json1);
        Debug.Log(aMan);

        Save<Data>(aMan, "save.txt");

        Data secondMan = Load<Data>("save.txt");

        Debug.Log(secondMan);

    }

    // Generic타입, 어떤 타입이 들어와도 괜찮다.
    void Save<T>(T data, string filename)
    {
        // Application.persistentDataPath
        // 세이브 파일이 저장된 위치를 어플리케이션으로 가지고 온다
        string path = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log(path);

        try
        {
            string json = JsonConvert.SerializeObject(data);
            json = SimpleEncryptionUtility.Encrypt(json);
            File.WriteAllText(path, json);
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }

    }
    T Load<T>(string filename)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);

        try
        {
            if(File.Exists(path))
            {
                string json = File.ReadAllText(path);
                json = SimpleEncryptionUtility.Decrypt(json);
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                //파일이 없어요
                return default;
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
            return default;
        }
    }
}
