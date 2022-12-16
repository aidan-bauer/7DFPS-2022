using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    public int stage = 0;
    public AnimationClip[] levelTransitions;
    public List<List<Health>> enemies;

    public static Action<int> OnStageComplete;
    public static Action OnLevelComplete;

    Animation levelTransit;

    private void Awake()
    {
        levelTransit = GameObject.FindGameObjectWithTag("Player").GetComponent<Animation>();
        enemies = new List<List<Health>>();

        int stageIndex = 0;

        foreach (Transform stage in transform)
        {
            enemies.Add(new List<Health>());

            List<Health> enemiesInStage = stage.GetComponentsInChildren<Health>().ToList();

            enemies[stageIndex].AddRange(enemiesInStage);
            stageIndex++;

            foreach (Health enemy in enemiesInStage)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        ReactivateEnemies();
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
        if (stage < enemies.Count())
        {
            if (enemies[stage].Count > 0)
            {
                GameObject toDelete = enemies[stage][enemies[stage].IndexOf(enemyToRemove)].gameObject;
                enemies[stage].Remove(enemyToRemove);
                Destroy(toDelete);

                //if all enemies are eliminated, move to the next stage
                if (enemies[stage].Count == 0)
                {
                    stage++;
                    OnStageComplete.Invoke(stage);
                }
            }
        }
    }

    void MoveToNextStage(int nextStage)
    {
        if (stage == enemies.Count())
        {
            OnLevelComplete.Invoke();
        } else
        {
            levelTransit.AddClip(levelTransitions[stage - 1], levelTransitions[stage - 1].name);
            levelTransit.clip = levelTransitions[stage - 1];
            levelTransit.Play();
            StartCoroutine(TransitionEnd(ReactivateEnemies));
        }
    }

    void LevelComplete()
    {
        Debug.Log("FINISH!");
    }

    void ReactivateEnemies()
    {
        foreach (Health enemy in enemies[stage])
        {
            enemy.GetComponent<EnemyShooting>().Activate();
        }
    }

    IEnumerator TransitionEnd(Action callback)
    {
        bool transitionOver = false;

        while (!transitionOver)
        {
            if (!levelTransit.isPlaying)
            {
                callback.Invoke();
                transitionOver = true;
                //yield return null;
                yield break;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}

[System.Serializable]
public class Stage {

}