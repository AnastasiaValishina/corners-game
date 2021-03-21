using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    Board board;
    GameObject reference = null;

    int plateX;
    int plateY;

    public void OnMouseDown()
    {
        board = FindObjectOfType<Board>();
        board.SetPositionEmpty(reference.GetComponent<Pawn>().xPos, reference.GetComponent<Pawn>().yPos);

        reference.GetComponent<Pawn>().xPos = plateX;
        reference.GetComponent<Pawn>().yPos = plateY;
        reference.GetComponent<Pawn>().SetCoords();

        board.SetPosition(reference);

        FindObjectOfType<GameController>().NextTurn();

        reference.GetComponent<Pawn>().DestroyMovePlates();

        board.CheckWinner();
    }

    public void SetCoords(int x, int y)
    {
        plateX = x;
        plateY = y;
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
