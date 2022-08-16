using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolInitializer : MonoBehaviour
{
    public Tool[] tools;

    private void Start()
    {
        UpdateTools();
    }

    public void Initialize(AnimatableObjects theAnimatableObject)
    {
        foreach (var item in tools)
        {
            item.myAnimatableObject = theAnimatableObject;
        }
    }

    public void UpdateTools()
    {
        foreach (var item in tools)
        {
            if (!EditorController.Instance.isPosition)
            {
                if (item.isPosX || item.isPosY)
                    item.transform.parent.gameObject.SetActive(false);
                if (item.isRot)
                    item.transform.gameObject.SetActive(true);
            }
            else
            {
                if (item.isRot)
                    item.transform.gameObject.SetActive(false);
                if (item.isPosX || item.isPosY)
                    item.transform.parent.gameObject.SetActive(true);
            }
        }
    }
}
