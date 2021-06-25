using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public int posX;
    public int posY;
    public Color highlightedColor;
    public Color hoverColor;
    public Color attackColor;
    public Color attackHover;
    public GameObject highlight;
    public bool highlighted = false;
    public bool threatend = false;

    public bool Empty { get; set; }
    
    public Figure figure;
    

    void Awake()
    {
        SetEmpty();
    }

    public void SetEmpty()
    {
        Empty = true;
    }

    void OnMouseEnter()
    {
        if (highlighted)
        {
            if(highlight.GetComponent<MeshRenderer>().material.color == attackColor)
            {
                highlight.GetComponent<MeshRenderer>().material.color = attackHover;
            }
            else highlight.GetComponent<MeshRenderer>().material.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        if (highlighted)
        {
            if (highlight.GetComponent<MeshRenderer>().material.color == attackHover)
            {
                highlight.GetComponent<MeshRenderer>().material.color = attackColor;
            }
            else highlight.GetComponent<MeshRenderer>().material.color = highlightedColor;
        }
    }

    void OnMouseDown()
    {
        if (highlighted)
        {
            if (figure != null && figure.IsKing && ChessManager.instance.check) return;
            ChessManager.instance.RequestMove(this);
            if(CheckPhase.instance.initiated)
                CheckPhase.instance.Finish();
            ChessManager.instance.check = false;
            ChessManager.instance.Unthreaten();
        }
    }

    public void Highlight()
    {
        highlight.SetActive(true);
        highlight.GetComponent<MeshRenderer>().material.color = highlightedColor;
        highlighted = true;
    }

    public void HighlightRed()
    {
        highlight.SetActive(true);
        highlight.GetComponent<MeshRenderer>().material.color = attackColor;
        highlighted = true;
    }

    public void UnLight()
    {
        highlight.SetActive(false);
        highlighted = false;
    }

    public void UnThreaten()
    {
        threatend = false;
    }


}
