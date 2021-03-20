using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public GameObject movePlate;
    public int xPos;
    public int yPos;
    public string player;

    Board board;

    void Start()
    {
        board = FindObjectOfType<Board>();
        xPos = (int)transform.position.x;
        yPos = (int)transform.position.y;

        switch (name)
        {
            case "white": player = "White";
                break;
            case "black": player = "Black";
                break;
        }
    }

    public void SetCoords()
    {
        transform.position = new Vector3(xPos, yPos, 0f);
    }

    private void OnMouseUp()
    {
        if (!board.IsGameOver() && board.GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            InitiateMovePlates();
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

    private void InitiateMovePlates()
    {
        // сделать шаг в любом направлении
        if (FindObjectOfType<GameController>().moveOneSquare)
        {
            ActivateMoveOneSquare();
        }

        // перепрыгнуть по диагонали
        if (FindObjectOfType<GameController>().jumpDiagonal)
        {
            ActivateJumpOverDiag();
        }

        // перепрыгнуть по вертикали по горизонтали
        if (FindObjectOfType<GameController>().jumpLine)
        {
            ActivateJumpOver();
        }
    }

    private void ActivateJumpOver()
    {
        if (board.GetPosition(xPos + 1, yPos))
        {
            JumpOver(xPos + 2, yPos);
        }
        if (board.GetPosition(xPos, yPos + 1))
        {
            JumpOver(xPos, yPos + 2);
        }
        if (board.GetPosition(xPos - 1, yPos))
        {
            JumpOver(xPos - 2, yPos);
        }
        if (board.GetPosition(xPos, yPos - 1))
        {
            JumpOver(xPos, yPos - 2);
        }
    }

    private void ActivateJumpOverDiag()
    {
        if (board.GetPosition(xPos + 1, yPos + 1))
        {
            JumpOver(xPos + 2, yPos + 2);
        }
        if (board.GetPosition(xPos - 1, yPos + 1))
        {
            JumpOver(xPos - 2, yPos + 2);
        }
        if (board.GetPosition(xPos + 1, yPos - 1))
        {
            JumpOver(xPos + 2, yPos - 2);
        }
        if (board.GetPosition(xPos - 1, yPos - 1))
        {
            JumpOver(xPos - 2, yPos - 2);
        }
    }

    private void ActivateMoveOneSquare()
    {
        MoveOneSquare(xPos, yPos + 1);
        MoveOneSquare(xPos, yPos - 1);
        MoveOneSquare(xPos + 1, yPos);
        MoveOneSquare(xPos - 1, yPos);
        MoveOneSquare(xPos + 1, yPos + 1);
        MoveOneSquare(xPos - 1, yPos - 1);
        MoveOneSquare(xPos + 1, yPos - 1);
        MoveOneSquare(xPos - 1, yPos + 1);
    }

    private void JumpOver(int x, int y)
    {
        if (board.PositionOnBoardExists(x, y))
        {
            if (board.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }
        }
    }

    private void MoveOneSquare(int x, int y)
    {
        if (board.PositionOnBoardExists(x, y))
        {
            if (board.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }
        }
    }

    private void MovePlateSpawn(int x, int y)
    {
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, 0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(x, y);
    }    
}
