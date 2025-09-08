using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class FileHandler
{
    public static void SaveToJSON<T>(List<T> toSave, string fn)
    {
        Debug.Log(GetPath(fn));
        string content = JsonHelper.ToJson<T>(toSave.ToArray());

        WriteFile(GetPath(fn), content);
    }

    public static void SaveToJSON<T>(T toSave, string fn)
    {
        string content = JsonUtility.ToJson (toSave);

        WriteFile(GetPath(fn), content);
    }

    public static List<T> ReadListFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPath(filename));

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<T>();
        }

        List<T> result = JsonHelper.FromJson<T>(content).ToList();
        return result;
    }

    public static T ReadFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPath(filename));

        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return default(T);
        }

        T result = JsonUtility.FromJson<T>(content);
        return result;
    }

    private static string GetPath(string fn)
    {
        return Application.persistentDataPath + "/" + fn;
    }

    private static void WriteFile(string path, string content)
    {
        FileStream fs = new FileStream(path, FileMode.Create);

        using (StreamWriter w = new StreamWriter(fs))
        {
            w.Write(content);
        }
    }

    private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string content = sr.ReadToEnd();
                return content;
            }
        }

        return "";
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T> (string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool pretty)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, pretty);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
