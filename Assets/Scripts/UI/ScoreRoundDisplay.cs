using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreRoundDisplay : MonoBehaviour
{
    [Header("References")]
    public Text roundNumberText;
    public Text scoreText;
    public Text endGameScoreText;
    public Text endRoundScoreText;


    public void UpdateGameScoreText(int score)
    {
        scoreText.text = score.ToString();
        endGameScoreText.text = score.ToString();
    }

    public void UpdateRoundScore(int score)
    {
        endRoundScoreText.text = score.ToString();
    }

    public void UpdateRoundText(int roundNumber, int totalRounds)
    {
        roundNumberText.text = roundNumber.ToString() + " sur " + totalRounds.ToString();
    }
}
