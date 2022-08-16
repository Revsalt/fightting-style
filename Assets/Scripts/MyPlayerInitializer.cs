using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerInitializer : MonoBehaviour
{
    public static MyPlayerInitializer Instance;

    public string characterCharacteristicCachePath = "";
    public CharacterCharacteristic myCharacterCharacteristic;

    private void Start()
    {
        characterCharacteristicCachePath = Application.dataPath;

        Instance = this;

        LoadCharacterCharacteristic();
    }

    public void LoadCharacterCharacteristic()
    {
        if (!File.Exists(characterCharacteristicCachePath + "/" + "MyPlayerCharacteristics.txt"))
            return;

        string json = File.ReadAllText(characterCharacteristicCachePath + "/" + "MyPlayerCharacteristics.txt");

        if (json == string.Empty)
            return;

        myCharacterCharacteristic = CharacterCharacteristic.GetCharacterCharacteristicFromJson(json);
    }

    public void CacheCharacterCharacteristic(CharacterCharacteristic characterCharacteristic)
    {
        string json = CharacterCharacteristic.GetCharacterCharacteristicToJson(characterCharacteristic);

        File.WriteAllText(characterCharacteristicCachePath + "/" + "MyPlayerCharacteristics" + ".txt", json);
    }
}

[System.Serializable]
public class CharacterCharacteristic
{
    public Animation[] animations = new Animation[3];

    public static CharacterCharacteristic GetCharacterCharacteristicFromJson(string json)
    {
        string[] animations = json.Split('^');
        List<Animation> Animations = new List<Animation>();

        foreach (var item in animations)
        {
            string json_ = item.Substring(1, item.Length - 2);

            Debug.Log(json_);

            CachedJsonAnimation cachedJsonAnimation = JsonUtility.FromJson<CachedJsonAnimation>(json_);

            Animations.Add(new Animation() { animationFrames = Animation.GetKeyFramesFromJson(cachedJsonAnimation.Keyframes)});

        }

        return new CharacterCharacteristic() { animations = Animations.ToArray() };
    }

    public static string GetCharacterCharacteristicToJson(CharacterCharacteristic characterCharacteristic)
    {
        string json = "";

        for (int i = 0; i <= characterCharacteristic.animations.Length - 1; i++)
        {
            CachedJsonAnimation cachedJsonAnimation = new CachedJsonAnimation();
            cachedJsonAnimation.Keyframes = Animation.GetKeyFramesInJson(characterCharacteristic.animations[i].animationFrames);

            string animation = JsonUtility.ToJson(cachedJsonAnimation);
            animation = "[" + animation + "]";

            if (i != characterCharacteristic.animations.Length - 1)
                animation += "^";

            json += animation;
        }

        return json;
    }
}

