using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    GameObject reference = null;

    int plateX;
    int plateY;

    public void OnMouseDown()
    {
        Board board = FindObjectOfType<Board>();
        Pawn pawnScript = reference.GetComponent<Pawn>();

        // отметить, что квадрат, на котором стояла пешка, свободен
        board.SetPositionEmpty(pawnScript.xPos, pawnScript.yPos);

        // переместить пешку
        pawnScript.xPos = plateX;
        pawnScript.yPos = plateY;
        pawnScript.SetCoords();

        // отметить, что квадрат занят пешкой
        board.SetPosition(reference);

        // проверить есть ли победитель
        board.CheckWinner();

        // передать ход след игроку
        FindObjectOfType<GameController>().NextTurn();

        // удалить остальные маркеры возможных ходов 
        pawnScript.DestroyMovePlates();         
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
