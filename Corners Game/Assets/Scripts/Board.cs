﻿using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject squareBlackPrefab;
    [SerializeField] GameObject squareWhitePrefab;
    [SerializeField] GameObject pawnBlackPrefab;
    [SerializeField] GameObject pawnWhitePrefab;
    [SerializeField] Transform tilesContainer;
    [SerializeField] Transform pawnsContainer;

    GameObject[,] squares;
    GameController gameController;

    int width = 8;
    int height = 8;


    void Start()
    {
        squares = new GameObject[width, height];
        gameController = FindObjectOfType<GameController>();
    }

    public void StartGame()
    {
        CreateBoard();
        PlacePawns();
    }

    private void CreateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 posBlack = new Vector2(x, y);
                GameObject squareBlack = Instantiate(squareBlackPrefab, posBlack, Quaternion.identity) as GameObject;
                squareBlack.transform.parent = tilesContainer;
                squareBlack.name = "( " + x + ", " + y + " )";
                y++;
                Vector2 posWhite = new Vector2(x, y);
                GameObject squareWhite = Instantiate(squareWhitePrefab, posWhite, Quaternion.identity) as GameObject;
                squareWhite.transform.parent = tilesContainer;
                squareWhite.name = "( " + x + ", " + y + " )";
            }
            x++;
            for (int y = 0; y < height; y++)
            {
                Vector2 posBlack = new Vector2(x, y);
                GameObject squareWhite = Instantiate(squareWhitePrefab, posBlack, Quaternion.identity) as GameObject;
                squareWhite.transform.parent = tilesContainer;
                squareWhite.name = "( " + x + ", " + y + " )";
                y++;
                Vector2 posWhite = new Vector2(x, y);
                GameObject squareBlack = Instantiate(squareBlackPrefab, posWhite, Quaternion.identity) as GameObject;
                squareBlack.transform.parent = tilesContainer;
                squareBlack.name = "( " + x + ", " + y + " )";
            }
        }
    }
    private void PlacePawns()
    {
        // пешки первого игока
        for (int x = 5; x <= 7; x++)
        {
            for (int y = 0; y <= 2; y++)
            {
                Vector2 pawnPos = new Vector2(x, y);
                GameObject pawn = Instantiate(pawnWhitePrefab, pawnPos, Quaternion.identity);
                pawn.transform.parent = pawnsContainer;
                pawn.name = "white";
                squares[x, y] = pawn;
            }
        }

        // пешки второго игрока
        for (int x = 0; x <= 2; x++)
        {
            for (int y = 5; y <= 7; y++)
            {
                Vector2 pawnPos = new Vector2(x, y);
                GameObject pawn = Instantiate(pawnBlackPrefab, pawnPos, Quaternion.identity);
                pawn.transform.parent = pawnsContainer;
                pawn.name = "black";
                squares[x, y] = pawn;
            }
        }
    }

    public void SetPositionEmpty(int x, int y)  // отметить, что квадрат свободен
    {
        squares[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return squares[x, y];
    }
       
    public bool PositionOnBoardExists(int x, int y) 
    {
        if (x < 0 || y < 0 || x >= squares.GetLength(0) || y >= squares.GetLength(1))
        {
            return false;
        }
        else { return true; }
    }

    public void SetPosition(GameObject obj) // отметить, что квадрат занят определенной пешкой
    {
        Pawn pawn = obj.GetComponent<Pawn>();
        squares[pawn.GetPositionX(), pawn.GetPositionY()] = obj;
    }

    public void CheckWinner()               // проверить все ли пешки стоят на поле противника
    {
        if (GetPosition(0, 5) && GetPosition(0, 5).name == "white" &&
            GetPosition(0, 6) && GetPosition(0, 6).name == "white" &&
            GetPosition(0, 7) && GetPosition(0, 7).name == "white" &&
            GetPosition(1, 5) && GetPosition(1, 5).name == "white" &&
            GetPosition(1, 6) && GetPosition(1, 6).name == "white" &&
            GetPosition(1, 7) && GetPosition(1, 7).name == "white" &&
            GetPosition(2, 5) && GetPosition(2, 5).name == "white" &&
            GetPosition(2, 6) && GetPosition(2, 6).name == "white" &&
            GetPosition(2, 7) && GetPosition(2, 7).name == "white")
        {
            gameController.Winner(gameController.GetPlayerOneName());
        }
        if (GetPosition(5, 0) && GetPosition(5, 0).name == "black" &&
            GetPosition(5, 1) && GetPosition(5, 1).name == "black" &&
            GetPosition(5, 2) && GetPosition(5, 2).name == "black" &&
            GetPosition(6, 0) && GetPosition(6, 0).name == "black" &&
            GetPosition(6, 1) && GetPosition(6, 1).name == "black" &&
            GetPosition(6, 2) && GetPosition(6, 2).name == "black" &&
            GetPosition(7, 0) && GetPosition(7, 0).name == "black" &&
            GetPosition(7, 1) && GetPosition(7, 1).name == "black" &&
            GetPosition(7, 2) && GetPosition(7, 2).name == "black")
        {
            gameController.Winner(gameController.GetPlayerTwoName());
        }
    }
}
