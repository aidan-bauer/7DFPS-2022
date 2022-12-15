using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public int score;
    //public static int score = 0;

    public static Action<int> ScoreChange;

    /*public static int Score
    {
        get
        {
            return score;
        }
    }*/

    private void OnEnable()
    {
        ScoreChange += AddScore;
    }

    private void OnDisable()
    {
        ScoreChange -= AddScore;
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }
}
