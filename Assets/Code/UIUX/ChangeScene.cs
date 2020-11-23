using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Security.Cryptography.X509Certificates;

public class ChangeScene : MonoBehaviour
{
    public GameObject ge;
    public GameObject ExitMenuPanel;
    public Button quitEscMenu;
    public Font fontb;
    private Text[] GetText;

    void Start()
    {
        GetText = Text.FindObjectsOfType<Text>();

        foreach (Text go in GetText)
            go.font = fontb;

        Resume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ge.GetComponent<GameEngine>().GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        quitEscMenu.onClick.AddListener(Resume);
    }

    void Pause()
    {
        ExitMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        ge.GetComponent<GameEngine>().GameIsPause = true;
    }

    void Resume()
    {
        ExitMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        ge.GetComponent<GameEngine>().GameIsPause = false;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
 
}
