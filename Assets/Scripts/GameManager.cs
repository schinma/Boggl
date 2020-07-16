﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public enum Langage { FR, ENG, NONE };

    [Header("References")]
    public Grid gridManager;
    public Timer timerManager;
    public AudioManager audioManager;

    public ScoreRoundDisplay display;
    public Button shuffleButton;
    public WordListDisplay playersDisplay;

    [Header("UI Elements")]
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject endRoundPanel;
    [SerializeField] private GameObject connectionPanel;

    [Header("Game settings")] 
    public int numberOfRounds = 3;
    public int roundTime = 60;
    public Langage langage = Langage.FR;

    public int score = 0;

    private bool roundStarted = false;
    private int currentRoundNumber = 0;

    private void Start()
    {
        display.UpdateGameScoreText(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (roundStarted && timerManager.time <= 0)
        {
            FinishRound();
        }
    }

    public void GoToLobbyScreen()
    {
        lobbyPanel.SetActive(true);
        connectionPanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    public void GoToConnectScreen()
    {
        connectionPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    public void StartGame()
    {

        lobbyPanel.SetActive(false);
        gamePanel.SetActive(true);
        endPanel.SetActive(false);
        endRoundPanel.SetActive(false);

        timerManager.timerLength = roundTime;
        gridManager.LoadDictionnary(langage);

        score = 0;
        currentRoundNumber = 0;
        display.UpdateGameScoreText(score);
        display.UpdateRoundText(currentRoundNumber, numberOfRounds);
        shuffleButton.interactable = true;
        gridManager.ClearWordList();
    }

    public void StartRound()
    {
        gridManager.ClearWordList();
        gridManager.ShakeLetters();
        timerManager.RestartTimer();
        roundStarted = true;
        currentRoundNumber++;
        display.UpdateRoundText(currentRoundNumber, numberOfRounds);
        shuffleButton.interactable = false;
        gridManager.UnblockGrid();
    }

    public void FinishRound()
    {
        roundStarted = false;
        int roundScore = CalculateRoundScore(gridManager.GetWordFound());
        score += roundScore;
        display.UpdateGameScoreText(score);
        gridManager.BlockGrid();
        gridManager.ClearWarning();

        audioManager.PlayEndRound();
        StartCoroutine(EndRoundCoroutine(roundScore));
    }

    IEnumerator EndRoundCoroutine(int roundScore)
    {
        endRoundPanel.SetActive(true);
        display.UpdateRoundScore(roundScore);
        yield return new WaitForSeconds(3);

        shuffleButton.interactable = true;
        endRoundPanel.SetActive(false);
        
        if (currentRoundNumber == numberOfRounds)
        {
            audioManager.PlayEndGame();
            StartCoroutine(EndGameCoroutine());
        }
    }

    IEnumerator EndGameCoroutine()
    {
        endPanel.SetActive(true);
        shuffleButton.interactable = false;
        yield return new WaitForSeconds(5);

        endPanel.SetActive(false);
        gamePanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public int CalculateRoundScore(List<string> wordList)
    {
        int result = 0;
        foreach(string word in wordList)
        {
            switch (word.Length)
            {
                case 3:
                    result += 1;
                    break;
                case 4:
                    result += 1;
                    break;
                case 5:
                    result += 2;
                    break;
                case 6:
                    result += 3;
                    break;
                case 7:
                    result += 5;
                    break;
                default:
                    result += 11;
                    break;
            }
        }
        return result;
    }

    public void SetRoundsNumber(int number)
    {
        numberOfRounds = number + 3;
    }

    public void SetRoundTime(int time)
    {
        roundTime = time * 30 + 60;
    }

    public void SetLangage(int lan)
    {
        switch(lan)
        {
            case 0:
                langage = Langage.FR;
                break;
            case 1:
                langage = Langage.ENG;
                break;
        }
    }
}