using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyFrameSlot : MonoBehaviour
{
    public KeyFrame myKeyFrame;
    public GameObject Key;

    public void SetMeSelected()
    {
        TimeLine.Instance.Pause();
        TimeLine.Instance.SelectedFrameSlot = this;
        if (myKeyFrame != null)
            ViewKeyFrame();
    }

    public void AddKeyFrame(KeyFrame keyframe)
    {
        myKeyFrame = keyframe;
        Key.SetActive(true);
        myKeyFrame.KeyFrameNumber = Convert.ToInt32(GetComponent<Text>().text);
    }

    public void RemoveKeyFrame()
    {
        myKeyFrame = null;
        Key.SetActive(false);
    }

    public void ViewKeyFrame()
    {
        EditorController.Instance.characterValues.PlayKeyFrame(myKeyFrame , true , 0);
    }
}
