using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    Board board;
    public GameObject movePlate;

    public int xBoard;
    public int yBoard;

    public string player;

    void Start()
    {
        board = FindObjectOfType<Board>();
        xBoard = (int)transform.position.x;
        yBoard = (int)transform.position.y;

        switch (name)
        {
            case "white": player = "white";
                break;
            case "black": player = "black";
                break;
        }
    }

    public void SetCoords()
    {
        transform.position = new Vector3(xBoard, yBoard, 0f);
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
    //    switch (board)
        PawnMovePlate(xBoard, yBoard + 1);
        PawnMovePlate(xBoard, yBoard - 1);
        PawnMovePlate(xBoard + 1, yBoard);
        PawnMovePlate(xBoard - 1, yBoard);
        PawnMovePlate(xBoard + 1, yBoard + 1);
        PawnMovePlate(xBoard - 1, yBoard - 1);
        PawnMovePlate(xBoard + 1, yBoard - 1);
        PawnMovePlate(xBoard - 1, yBoard + 1);
    }

    private void PawnMovePlate(int x, int y)
    {
        if (board.PositionOnBoard(x, y))
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
