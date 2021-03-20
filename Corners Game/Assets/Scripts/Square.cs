using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    static Square targetSquare = null;
    Board board;

    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        targetSquare = gameObject.GetComponent<Square>();
        Debug.Log("name " + name);
    }

 /*   public Vector2 GetTargetPosition()
    {
        if (targetSquare)
        {
            Vector2 targetPosition = new Vector2(transform.position.x, transform.position.y);
            return targetPosition;
        }
    }*/

    public bool isClicked()
    {
        if (targetSquare)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
