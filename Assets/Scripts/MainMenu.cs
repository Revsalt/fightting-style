using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    public Button returnToMainMenuButton;
    public GameObject allPanels;
    public GameObject currentPanel = null;
    public GameObject pauseMenu = null;
    public Text status;
    [Header("Loading Vars")]
    public Slider loadProgressbar;
    public GameObject loadingPanel;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        allPanels.SetActive(!NetworkClient.isConnected);
        status.text = GetComponent<NetworkManagerHUD>().status;

        if (!NetworkServer.active)
            GetComponent<NetworkManagerHUD>().SetShowGUI(false);

        if (NetworkClient.isConnected && Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
            if (pauseMenu.activeInHierarchy)
                currentPanel = pauseMenu;
        }
    }

    public void BackToMainMenu()
    {
        if (NetworkServer.active || NetworkClient.active)
        {
            NetworkClient.Disconnect();
            NetworkServer.DisconnectAll();
        }

        StartCoroutine(LoadAysnc("MainMenu"));

        returnToMainMenuButton.gameObject.SetActive(false);
        allPanels.gameObject.SetActive(true);
    }

    public void OpenPanel(GameObject panel)
    {
        if (panel == currentPanel)
            return;

        panel.SetActive(true);
        currentPanel.SetActive(false);

        currentPanel = panel;
    }

    public void OpenScene(string SceneName)
    {
        if (SceneName != string.Empty)
            StartCoroutine(LoadAysnc(SceneName));

        returnToMainMenuButton.gameObject.SetActive(true);
        allPanels.gameObject.SetActive(false);
    }

    IEnumerator LoadAysnc (string SceneName)
    {
        loadingPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);
        while (!operation.isDone)
        {
            loadProgressbar.value = operation.progress;
            yield return null;
        }

        yield return new WaitForSeconds(1);
        loadProgressbar.value = 1;
        yield return new WaitForSeconds(.2f);

        loadingPanel.SetActive(false);
    }
}
