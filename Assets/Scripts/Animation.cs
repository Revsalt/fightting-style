using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation
{
    public KeyFrame[] animationFrames;
    public int[] limbDamagePrioritaizing;

    public static string GetKeyFramesInJson(KeyFrame[] _animationFrames)
    {
        string json = "";

        for (int i = 0; i <= _animationFrames.Length - 1; i++)
        {
            string key = JsonUtility.ToJson(_animationFrames[i]);
            key = "[" + key + "]";

            if (i != _animationFrames.Length - 1)
                key += "|";

            json += key;
        }

        return json;
    }

    public static KeyFrame[] GetKeyFramesFromJson(string json)
    {
        string[] keys = json.Split('|');
        List<KeyFrame> Keys = new List<KeyFrame>();

        foreach (var item in keys)
        {
            Keys.Add(JsonUtility.FromJson<KeyFrame>(item.Substring(1, item.Length - 2)));
        }

        return Keys.ToArray();
    }
}
