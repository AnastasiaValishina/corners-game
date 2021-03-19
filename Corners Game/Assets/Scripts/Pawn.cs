using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public GameObject selectedBack;

    bool isSelected = false;
    static Pawn previousSelected = null;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (isSelected)
        {
            Deselect();
        }
        else
        {
            if (previousSelected == null)
            {
                Select();
            }
            else
            {
                previousSelected.Deselect();
                Select();
            }
        }
    }

    private void Select()
    {
        isSelected = true;
        selectedBack.SetActive(true);
        previousSelected = gameObject.GetComponent<Pawn>();
    }
    
    private void Deselect()
    {
        isSelected = false;
        selectedBack.SetActive(false);
        previousSelected = null;
    }
}
