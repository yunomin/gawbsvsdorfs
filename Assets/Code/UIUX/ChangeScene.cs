using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameObject ge;
    public GameObject ExitMenuPanel;
    public Button quitEscMenu;

    void Start()
    {

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

    public void LoadGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void EndGame(int Winner)
    {
        SceneManager.LoadScene("EndScene");
    }
}
