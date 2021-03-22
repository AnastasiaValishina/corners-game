using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject popupMenu;
    public Text winnerText;
    public Text turnText;
    public GameObject restartButton;

    [Header("Difficulty Toggles")]
    public GameObject toggleJumpDiagonal;
    public GameObject toggleJumpLine;
    public GameObject toggleMoveOneSquare;
    public bool jumpDiagonal = false;
    public bool jumpLine = false;
    public bool moveOneSquare = true;

    [Header("Player names input")]
    public string playerOneName;
    public string playerTwoName;
    public GameObject inputFieldPlayerOne;
    public GameObject inputFieldPlayerTwo;

    string currentPlayer;
    bool gameOver = false;

    void Start()
    {
        toggleJumpDiagonal.GetComponent<Toggle>().isOn = jumpDiagonal;
        toggleJumpLine.GetComponent<Toggle>().isOn = jumpLine;
        toggleMoveOneSquare.GetComponent<Toggle>().isOn = moveOneSquare;
    }
    private void Update()
    {
        if (gameOver == true)
        {
            restartButton.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleJumpDiagonal(bool newValue)
    {
        jumpDiagonal = newValue;
    }
    
    public void ToggleJumpLine(bool newValue)
    {
        jumpLine = newValue;
    }

    public void ToggleMoveOneSquare(bool newValue)
    {
        moveOneSquare = newValue;
    }

    public void OnStartClick()
    {
        FindObjectOfType<Board>().StartGame();
        popupMenu.SetActive(false);
        SetPlayersNames();
        currentPlayer = playerOneName;
        UpdatePlayerText();
    }

    private void SetPlayersNames()
    {
        if (inputFieldPlayerOne.GetComponent<Text>().text == "")
        {
            playerOneName = "Игрок 1";
        }
        else
        {
            playerOneName = inputFieldPlayerOne.GetComponent<Text>().text;
        }

        if (inputFieldPlayerTwo.GetComponent<Text>().text == "")
        {
            playerTwoName = "Игрок 2";
        }
        else
        {
            playerTwoName = inputFieldPlayerTwo.GetComponent<Text>().text;
        }
    }

    private void UpdatePlayerText()
    {
        turnText.GetComponent<Text>().text = currentPlayer + " ходит...";
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void NextTurn()
    {
        if (currentPlayer == playerOneName)
        {
            currentPlayer = playerTwoName;
            UpdatePlayerText();
        }
        else
        {
            currentPlayer = playerOneName;
            UpdatePlayerText();
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        winnerText.GetComponent<Text>().enabled = true;
        winnerText.GetComponent<Text>().text = playerWinner + " победил!";
    }

    public bool IsGameOver()
    {
        return gameOver;
    }
}
