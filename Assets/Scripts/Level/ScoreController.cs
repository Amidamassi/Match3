using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text targetScoreText;
    [SerializeField] Text targetTimeTurnsText;
    [SerializeField] LevelController levelController;
    [SerializeField] Image winMenu;
    [SerializeField] Image loseMenu;

    private int score = 0;
    private float timer = 0;
    private int turns = 0;
    private bool pause = false;

    public void Initialize(bool scoreLevel)
    {
        score = 0;
        if (scoreLevel)
        {
            scoreText.text = score.ToString();
            turns = levelController.level.targetTurns;
            targetScoreText.text = "Goal " + levelController.level.targetScore;
            targetTimeTurnsText.text = "Turns" + turns;
        }
        else
        {
            scoreText.text = score.ToString();
            timer = levelController.level.targetTime;
            StartCoroutine(Timer());
            targetScoreText.text = "Goal " + levelController.level.targetScore;
            targetTimeTurnsText.text = "Time " + timer;
        }
    }

    public void Pause()
    {
        pause = !pause;
    }

    public void nextTurn()
    {
        turns--;
        targetTimeTurnsText.text = "Turns" + turns;
        if (turns == 0)
        {
            loseMenu.gameObject.SetActive(true);
        }
    }

    public void ChangeScore(int value)
    {
        score += value;
        if (score > levelController.level.targetScore)
        {
            winMenu.gameObject.SetActive(true);
            StopTimer();
        }
        scoreText.text = score.ToString();
    }

    public void StopTimer()
    {
        StopCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!pause)
            {
                targetTimeTurnsText.text = "Time: " + timer;
                timer -= 0.1f;
                if (timer < 0)
                {
                    loseMenu.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
