using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    public int stage = 0;

    public List<List<Health>> enemies;

    public Action<int> OnStageComplete;
    public Action OnLevelComplete;

    private void Awake()
    {
        enemies = new List<List<Health>>();

        List<Transform> stages = new List<Transform>();

        int stageIndex = 0;

        foreach (Transform stage in transform)
        {
            Debug.Log(stage.name);
            stages.Add(stage);

            List<Health> enemiesInStage = stage.GetComponentsInChildren<Health>().ToList();

            enemies[stageIndex].AddRange(enemiesInStage);
        }
    }

    private void OnEnable()
    {
        OnStageComplete += MoveToNextStage;
        OnLevelComplete += LevelComplete;

        for (int i = 0; i < enemies.Count; i++)
        {
            for (int j = 0; j < enemies[i].Count; j++)
            {
                enemies[i][j].onDeath += RemoveFromStage;
            }
        }
    }

    private void OnDisable()
    {
        OnStageComplete -= MoveToNextStage;
        OnLevelComplete -= LevelComplete;

        for (int i = 0; i < enemies.Count; i++)
        {
            for (int j = 0; j < enemies[i].Count; j++)
            {
                enemies[i][j].onDeath -= RemoveFromStage;
            }
        }
    }

    void RemoveFromStage(Health enemyToRemove)
    {
        if (stage == enemies.Count())
        {
            if (enemies[stage].Count > 0)
            {
                enemies[stage].Remove(enemyToRemove);
            }
            else
            {
                stage++;
                OnStageComplete.Invoke(stage);
            }
        }
        else
        {
            OnLevelComplete.Invoke();
        }
    }

    void MoveToNextStage(int nextStage)
    {

    }

    void LevelComplete()
    {

    }
}

[System.Serializable]
public class Stage {

}