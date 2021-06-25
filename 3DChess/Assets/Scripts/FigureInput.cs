using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureInput : MonoBehaviour {
    

    void OnMouseEnter()
    {
        transform.GetComponentInParent<Figure>().MouseEnter();
    }

    void OnMouseExit()
    {
        transform.GetComponentInParent<Figure>().MouseExit();
    }

    void OnMouseDown()
    {
        transform.GetComponentInParent<Figure>().MouseClick();
    }

}
