﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

/// <summary>
/// Link to Team Eider's website: https://eldertheduck.wordpress.com/
/// Link to Team Shelduck's website: https://shelduck.wordpress.com/
/// Link to Assessment 3 project version: https://eldertheduck.wordpress.com/assessment-3
/// </summary>

/// <summary>
/// Manages the high scores GUI.
/// </summary>
public class HighScoresGUI : MonoBehaviour {

    public Text scoreList;
    public Text nameList;

    public GameObject highScoresPanel;

    /// <summary>
    /// The number of scores to display.
    /// </summary>
    public int numberOfScores = 10;

    void OnEnable()
    {
        updateHighScores();
    }

    public void updateHighScores()
    {
        int i = 0;

        //Get the high scores in high score order.
        var scores = HighScoresBackend.instance.getHighScores().OrderBy(h => -h.score).ToList();


        scoreList.text = "";
        nameList.text = "";

        foreach (HighScoresBackend.HighScore score in scores)
        {
            i++;

            //Stop adding scores when we have added enough.
            if (i > numberOfScores)
            {
                break;
            }

            scoreList.text += score.score + "\n";
            nameList.text += score.name + "\n";
        }
    }

    public void closePanel()
    {
        highScoresPanel.SetActive(false);
    }

    public void openPanel()
    {
        highScoresPanel.SetActive(true);
        updateHighScores();
    }
}
