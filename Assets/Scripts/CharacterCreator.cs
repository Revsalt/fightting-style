using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    public static CharacterCreator Instance;
    public GameObject currentPanel = null;

    private void Start()
    {
        Instance = this;
    }

    public void OpenPanel(GameObject panel)
    {
        if (panel == currentPanel)
            return;

        panel.SetActive(true);
        currentPanel.SetActive(false);

        currentPanel = panel;
    }
}
