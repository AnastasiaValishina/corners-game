using UnityEngine;
using UnityEngine.EventSystems;

public class MovePlate : MonoBehaviour, IPointerClickHandler
{
    GameObject reference = null;

    int plateX;
    int plateY;

	public void OnPointerClick(PointerEventData eventData)
	{
		AudioPlayer.Instance.PlaySlideSound();

		Pawn pawnScript = reference.GetComponent<Pawn>();

		// отметить, что квадрат, на котором стояла пешка, свободен
		Board.Instance.SetPositionEmpty(pawnScript.XPos, pawnScript.YPos);

		pawnScript.MoveTo(plateX, plateY);

		// отметить, что квадрат занят пешкой
		Board.Instance.SetPosition(reference);

		Board.Instance.CheckWinner();
        GameController.Instance.NextTurn();
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
