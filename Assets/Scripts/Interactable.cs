using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isSelected = false;

    public virtual void ToggleSelect()
    {
        isSelected = !isSelected;
    }
}
