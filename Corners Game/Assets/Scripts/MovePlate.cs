using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    Board board;
    GameObject reference = null;

    int matrixX;
    int matrixY;

    private void Start()
    {
        
    }

    public void OnMouseUp()
    {
        board = FindObjectOfType<Board>();
        board.GetComponent<Board>().SetPositionEmpty(reference.GetComponent<Pawn>().xBoard, reference.GetComponent<Pawn>().yBoard);

        reference.GetComponent<Pawn>().xBoard = matrixX;
        reference.GetComponent<Pawn>().yBoard = matrixY;
        reference.GetComponent<Pawn>().SetCoords();

        board.GetComponent<Board>().SetPosition(reference);
        board.NextTurn();

        reference.GetComponent<Pawn>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference (GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
