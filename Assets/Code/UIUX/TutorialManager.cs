using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject engine;
    private bool begineGame;
    //scene 1
    public Text t11;
    public GameObject tutorialPanel;


    private Queue<string> sentences;
    public void StartTutorial(Dialogue dialogue, bool isFirst)
    {
        begineGame = isFirst;
        sentences = new Queue<string>();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        ShowNext();
    }
    public void ShowNext()
    {
        if(sentences.Count == 0)
        {
            EndTutorial();
            return;
        }
        string sentence = sentences.Dequeue();
        t11.text = sentence;
    }
    public void EndTutorial()
    {
        if (begineGame)
        {
            tutorialPanel.SetActive(false);
            engine.GetComponent<GameEngine>().startGame();
        }
    }
}
