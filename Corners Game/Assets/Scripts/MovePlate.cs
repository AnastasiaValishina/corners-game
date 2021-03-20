using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    Board board;
    GameObject reference = null;

    int matrixX;
    int matrixY;

    public void OnMouseUp()
    {
        board = FindObjectOfType<Board>();
        board.SetPositionEmpty(reference.GetComponent<Pawn>().xPos, reference.GetComponent<Pawn>().yPos);

        reference.GetComponent<Pawn>().xPos = matrixX;
        reference.GetComponent<Pawn>().yPos = matrixY;
        reference.GetComponent<Pawn>().SetCoords();

        board.SetPosition(reference);
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
