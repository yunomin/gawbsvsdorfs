using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject engine;
    public Text t11;
    public GameObject tutorialPanel;

    private int index; // stage of tutorial
    private Queue<string> sentences;
    
    public void StartTutorial(Dialogue dialogue, int i)
    {
        index = i;
        sentences = new Queue<string>();
        if(index == 3 || index == 5)
        {
            return;
        }
        tutorialPanel.SetActive(true);
        engine.GetComponent<GameEngine>().isTurn = false;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        ShowNextSentence();
    }
    public void ShowNext()
    {
        
    }
    public void ShowNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndTutorial();
            return;
        }
        string sentence = sentences.Dequeue();
        t11.text = sentence;
    }
    public void EndTutorial()
    {
        if (index == 0)
        {
            engine.GetComponent<GameEngine>().startGame();
        }
        else if(index == 1)
        {
        }
        else if (index == 2)
        {
        }
        else if (index == 4)
        {
        }
        else if (index == 6)
        {
        }
        else if (index == 7)
        {
            engine.GetComponent<GameEngine>().isTutorial = false;
        }
        engine.GetComponent<GameEngine>().isTurn = true;
        tutorialPanel.SetActive(false);
    }
}
