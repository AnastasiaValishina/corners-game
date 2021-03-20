using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject squareBlackPrefab;
    public GameObject squareWhitePrefab;
    public GameObject pawnBlackPrefab;
    public GameObject pawnWhitePrefab;
    public Text winnerText;
    public Text restartText;
    public Text turnText;

    GameObject[,] squares; // positions
    GameObject[] playerBlack = new GameObject[9];
    GameObject[] playerWhite = new GameObject[9];

    string currentPlayer = "White";
    bool gameOver = false;

    int width = 8;
    int height = 8;


    void Start()
    {
        squares = new GameObject[width, height];
        CreateBoard();
        PlacePawns();
        UpdatePlayerText();
    }

    private void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            SceneManager.LoadScene(0);
        }
    }
    private void CreateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 posBlack = new Vector2(x, y);
                GameObject squareBlack = Instantiate(squareBlackPrefab, posBlack, Quaternion.identity) as GameObject;
                squareBlack.transform.parent = transform;
                squareBlack.name = "( " + x + ", " + y + " )";
                y++;
                Vector2 posWhite = new Vector2(x, y);
                GameObject squareWhite = Instantiate(squareWhitePrefab, posWhite, Quaternion.identity) as GameObject;
                squareWhite.transform.parent = transform;
                squareWhite.name = "( " + x + ", " + y + " )";
            }
            x++;
            for (int y = 0; y < height; y++)
            {
                Vector2 posBlack = new Vector2(x, y);
                GameObject squareWhite = Instantiate(squareWhitePrefab, posBlack, Quaternion.identity) as GameObject;
                squareWhite.transform.parent = transform;
                squareWhite.name = "( " + x + ", " + y + " )";
                y++;
                Vector2 posWhite = new Vector2(x, y);
                GameObject squareBlack = Instantiate(squareBlackPrefab, posWhite, Quaternion.identity) as GameObject;
                squareBlack.transform.parent = transform;
                squareBlack.name = "( " + x + ", " + y + " )";
            }
        }
    }
    private void PlacePawns()
    {
        // place player 1 pawns
        for (int x = 5; x <= 7; x++)
        {
            for (int y = 0; y <= 2; y++)
            {
                Vector2 pawnPos = new Vector2(x, y);
                GameObject pawn = Instantiate(pawnWhitePrefab, pawnPos, Quaternion.identity);
                pawn.transform.parent = transform;
                pawn.name = "white";
                squares[x, y] = pawn;
            }
        }

        // place player 2 pawns
        for (int x = 0; x <= 2; x++)
        {
            for (int y = 5; y <= 7; y++)
            {
                Vector2 pawnPos = new Vector2(x, y);
                GameObject pawn = Instantiate(pawnBlackPrefab, pawnPos, Quaternion.identity);
                pawn.transform.parent = transform;
                pawn.name = "black";
                squares[x, y] = pawn;
            }
        }
    }

    public void SetPositionEmpty(int x, int y)
    {
        squares[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return squares[x, y];
    }

    public bool PositionOnBoardExists(int x, int y)
    {
        if (x <= 0 || y <= 0 || x >= squares.GetLength(0) || y >= squares.GetLength(1))
        {
            return false;
        }
        else { return true; }
    }
    public void SetPosition(GameObject obj)
    {
        Pawn pawn = obj.GetComponent<Pawn>();
        squares[pawn.xPos, pawn.yPos] = obj;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "White")
        {
            currentPlayer = "Black";
            UpdatePlayerText();
        }
        else
        {
            currentPlayer = "White"; 
            UpdatePlayerText();
        }
    }

    private void UpdatePlayerText()
    {
        turnText.GetComponent<Text>().text = currentPlayer + "'s turn";
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        winnerText.GetComponent<Text>().enabled = true;
        winnerText.GetComponent<Text>().text = playerWinner + " is the winner";
        restartText.GetComponent<Text>().enabled = true;
    }
}
