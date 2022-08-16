using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatableObjects : Interactable
{
    private ToolInitializer myTool;

    public override void ToggleSelect()
    {
        base.ToggleSelect();

        if (isSelected)
        {
            EditorController.ClearTools();
            myTool = Instantiate(EditorController.Instance.RotateTool, transform.position, Quaternion.identity).GetComponent<ToolInitializer>();
            myTool.Initialize(this);
        }
    }
}
