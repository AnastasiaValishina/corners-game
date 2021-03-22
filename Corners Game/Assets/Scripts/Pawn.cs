using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public GameObject movePlate;
    public int xPos;
    public int yPos;
    
    string player;
    Board board;
    GameController gameController;

    void Start()
    {
        board = FindObjectOfType<Board>();
        gameController = FindObjectOfType<GameController>();
        xPos = (int)transform.position.x;
        yPos = (int)transform.position.y;

        switch (name)
        {
            case "white": player = gameController.playerOneName;
                break;
            case "black": player = gameController.playerTwoName;
                break;
        }
    }

    public void SetCoords()
    {
        transform.position = new Vector3(xPos, yPos, 0f);
    }

    private void OnMouseDown()
    {
        if (!gameController.IsGameOver() && gameController.GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            InitiateMovePlates();
        }
    }

    private void InitiateMovePlates()
    {
        if (gameController.moveOneSquare)
        {
            ActivateMoveOneSquare();
        }

        if (gameController.jumpDiagonal)
        {
            ActivateJumpOverDiag();
        }

        if (gameController.jumpLine)
        {
            ActivateJumpOver();
        }
    }

    private void ActivateJumpOver()         // перепрыгнуть по вертикали по горизонтали
    {
        if (board.PositionOnBoardExists(xPos + 1, yPos) && board.GetPosition(xPos + 1, yPos))
        {
            SpawnMovePlate(xPos + 2, yPos);
        }
        if (board.PositionOnBoardExists(xPos, yPos + 1) && board.GetPosition(xPos, yPos + 1))
        {
            SpawnMovePlate(xPos, yPos + 2);
        }
        if (board.PositionOnBoardExists(xPos - 1, yPos) && board.GetPosition(xPos - 1, yPos))
        {
            SpawnMovePlate(xPos - 2, yPos);
        }
        if (board.PositionOnBoardExists(xPos, yPos - 1) && board.GetPosition(xPos, yPos - 1))
        {
            SpawnMovePlate(xPos, yPos - 2);
        }
    }

    private void ActivateJumpOverDiag()         // перепрыгнуть по диагонали
    {
        if (board.PositionOnBoardExists(xPos + 1, yPos + 1) && board.GetPosition(xPos + 1, yPos + 1))
        {
            SpawnMovePlate(xPos + 2, yPos + 2);
        }
        if (board.PositionOnBoardExists(xPos - 1, yPos + 1) && board.GetPosition(xPos - 1, yPos + 1))
        {
            SpawnMovePlate(xPos - 2, yPos + 2);
        }
        if (board.PositionOnBoardExists(xPos + 1, yPos - 1) && board.GetPosition(xPos + 1, yPos - 1))
        {
            SpawnMovePlate(xPos + 2, yPos - 2);
        }
        if (board.PositionOnBoardExists(xPos - 1, yPos - 1) && board.GetPosition(xPos - 1, yPos - 1))
        {
            SpawnMovePlate(xPos - 2, yPos - 2);
        }
    }

    private void ActivateMoveOneSquare()         // сделать шаг в любом направлении
    {
        SpawnMovePlate(xPos, yPos + 1);
        SpawnMovePlate(xPos, yPos - 1);
        SpawnMovePlate(xPos + 1, yPos);
        SpawnMovePlate(xPos - 1, yPos);
        SpawnMovePlate(xPos + 1, yPos + 1);
        SpawnMovePlate(xPos - 1, yPos - 1);
        SpawnMovePlate(xPos + 1, yPos - 1);
        SpawnMovePlate(xPos - 1, yPos + 1);
    }

    private void SpawnMovePlate(int x, int y)   // поместить на доске маркеры возможных ходов
    {
        if (board.PositionOnBoardExists(x, y))
        {
            if (board.GetPosition(x, y) == null)
            {
                GameObject mp = Instantiate(movePlate, new Vector3(x, y, 0f), Quaternion.identity);
                MovePlate mpScript = mp.GetComponent<MovePlate>();
                mpScript.SetReference(gameObject);
                mpScript.SetCoords(x, y);
            }
        }
    }
    public void DestroyMovePlates()
    {
        MovePlate[] movePlates = FindObjectsOfType<MovePlate>();
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i].gameObject);
        }
    }
}
