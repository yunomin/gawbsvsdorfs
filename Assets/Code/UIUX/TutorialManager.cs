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
        if(index == 0)
        {

            engine.GetComponent<GameEngine>().startGame();
        }
        tutorialPanel.SetActive(false);
    }
}
