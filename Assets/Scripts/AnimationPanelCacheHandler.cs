using AnotherFileBrowser.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AnimationPanelCacheHandler : MonoBehaviour
{
    public CharacterCharacteristic characterCharacteristic = new CharacterCharacteristic();
    public Text[] paths;

    public void AssignAnimationFile(int num)
    {
        string json = "";

        var bp = new BrowserProperties();
        bp.filter = "text files (*.txt)|*.txt";
        bp.filterIndex = 0;

        var path_ = "";
        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            json = File.ReadAllText(path);
            path_ = path;
        });

        if (json == string.Empty)
            return;

        CachedJsonAnimation cachedJsonAnimation = JsonUtility.FromJson<CachedJsonAnimation>(json);

        characterCharacteristic.animations[num] = new Animation() { animationFrames = Animation.GetKeyFramesFromJson(cachedJsonAnimation.Keyframes) };

        paths[num].text = path_;
    }

    public void PassCharacterCharacteristic()
    {
        foreach (var item in characterCharacteristic.animations)
        {
            if (item == null)
                return;
        }

        MyPlayerInitializer.Instance.myCharacterCharacteristic = characterCharacteristic;
        MyPlayerInitializer.Instance.CacheCharacterCharacteristic(characterCharacteristic);
    }

}
