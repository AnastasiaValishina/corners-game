using UnityEngine;
using UnityEngine.EventSystems;

public class MovePlate : MonoBehaviour, IPointerClickHandler
{
    GameObject reference = null;

    int plateX;
    int plateY;

	public void OnPointerClick(PointerEventData eventData)
	{
        Pawn pawnScript = reference.GetComponent<Pawn>();

		// отметить, что квадрат, на котором стояла пешка, свободен
		Board.Instance.SetPositionEmpty(pawnScript.GetPositionX(), pawnScript.GetPositionY());

        // переместить пешку
        pawnScript.SetPositionX(plateX);
        pawnScript.SetPositionY(plateY);
        pawnScript.SetCoords();

		// отметить, что квадрат занят пешкой
		Board.Instance.SetPosition(reference);

		// проверить есть ли победитель
		Board.Instance.CheckWinner();

        // передать ход след игроку
        GameController.Instance.NextTurn();

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
