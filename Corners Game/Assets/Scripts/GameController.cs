using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject toggleJumpDiagonal;
    public GameObject toggleJumpLine;
    public GameObject toggleMoveOneSquare;

    public bool jumpDiagonal = false;
    public bool jumpLine = false;
    public bool moveOneSquare = true;

    void Start()
    {
        toggleJumpDiagonal.GetComponent<Toggle>().isOn = jumpDiagonal;
        toggleJumpLine.GetComponent<Toggle>().isOn = jumpLine;
        toggleMoveOneSquare.GetComponent<Toggle>().isOn = moveOneSquare;
    }   

    public void ToggleJumpDiagonal(bool newValue)
    {
        jumpDiagonal = newValue;
    }
    
    public void ToggleJumpLine(bool newValue)
    {
        jumpLine = newValue;
    }

    public void ToggleMoveOneSquare(bool newValue)
    {
        moveOneSquare = newValue;
    }
}
