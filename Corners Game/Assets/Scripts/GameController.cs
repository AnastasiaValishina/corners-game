using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject toggleJumpDiagonal;
    public GameObject toggleJumpLine;
    public GameObject toggleMoveOneSquare;
    public GameObject popupMenu;

    public Text winnerText;
    public Text restartText;
    public Text turnText;

    public string playerOneName;
    public string playerTwoName;
    public GameObject inputFieldPlayerOne;
    public GameObject inputFieldPlayerTwo;

    public bool jumpDiagonal = false;
    public bool jumpLine = false;
    public bool moveOneSquare = true;

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
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            SceneManager.LoadScene(0);
        }
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
        playerOneName = inputFieldPlayerOne.GetComponent<Text>().text;
        playerTwoName = inputFieldPlayerTwo.GetComponent<Text>().text;
        currentPlayer = playerOneName;
        UpdatePlayerText();

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
        restartText.GetComponent<Text>().enabled = true;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }
}
