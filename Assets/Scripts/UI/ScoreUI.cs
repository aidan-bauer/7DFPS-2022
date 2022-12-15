using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{

    [SerializeField] GameObject levelEndScreen;
    [SerializeField] TextMeshProUGUI currentScore;
    [SerializeField] TextMeshProUGUI endOfLevelScore;

    ScoreManager scoreManager;

    private void OnEnable()
    {
        ScoreManager.ScoreChange += UpdateScoreUI;
        StageManager.OnLevelComplete += UpdateLevelEnd;
    }

    private void OnDisable()
    {
        ScoreManager.ScoreChange -= UpdateScoreUI;
        StageManager.OnLevelComplete -= UpdateLevelEnd;
    }

    private void Awake()
    {
        scoreManager = GetComponentInParent<ScoreManager>();

        UpdateScoreUI(0);
    }

    public void UpdateScoreUI(int i)
    {
        currentScore.text = scoreManager.score.ToString();
    }

    public void UpdateLevelEnd()
    {
        endOfLevelScore.text = scoreManager.score.ToString();
        levelEndScreen.SetActive(true);
    }
}
