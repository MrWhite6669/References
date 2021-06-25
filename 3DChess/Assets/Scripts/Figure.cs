using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour {

    public MeshRenderer graphics; // Graphics of the figure
    public Color selectedColor;
    public bool isYours = false;
    public bool isKing = false;
    public int timesMoved = 0; // How many times figure has moved

    public Node currentNode; // Current node under the figure
    public HighlightManager highlighterManager;
    public LayerMask mask;

    private Color defaultColor; //Default spawn color
    private Vector3 defaultPosition; //Default spawn position in the world
    private Vector3 rayOffset = new Vector3(0, 1, 0); //Offset of the raycast

    public enum Type
    {
        None,
        Black = 1,
        White = 2
    }

    public Type type;

    public bool IsKing
    {
        get
        {
            if (isKing) return true;
            return false;
        }
    }

    void Start()
    {
        LoadManager();
        LoadColor();
        UpdateCurrentNode();
        currentNode.Empty = false;
        defaultPosition = transform.position;
        ChessManager.instance.onMatchEnded += Respawn;
        ChessManager.instance.onFigureMoved += UpdateCurrentNode;
    }

    public void UpdateCurrentNode() // Update node under the figure
    {
        RaycastHit info;
        bool hit = Physics.Raycast(transform.position + rayOffset, Vector3.down, out info, 20f, mask);
        if (hit) //If something is under the node and its node , update the current node
        {
            currentNode = info.collider.GetComponent<Node>();
            currentNode.figure = this;
        }
    }

    void Respawn() // Respawn the figure
    {
        gameObject.SetActive(true);
        transform.position = defaultPosition;
        UpdateCurrentNode();
        currentNode.Empty = false;
        timesMoved = 0;
        isYours = false;
    }

    public void LoadColor() // Check the collor type and update figure graphics
    {
        switch (type)
        {
            case Type.Black:
                {
                    graphics.material.color = Color.black;
                }
                break;
            case Type.White:
                {
                    graphics.material.color = Color.white;
                }
                break;
            case Type.None:
                {
                    graphics.material.color = Color.blue;
                }
                break;
        }
        defaultColor = graphics.material.color;
    }

    public void LoadManager() // Load figures Highlight manager
    {
        highlighterManager = transform.Find("Higlighters").GetComponent<HighlightManager>();
    }

    public void MouseClick()
    {
        if (isYours)
        {
            ChessManager.instance.SelectFigure(this);
        }
        if (CheckPhase.instance.initiated) ChessManager.instance.king.currentNode.HighlightRed();
    }

    public void MouseEnter()
    {
        if(isYours)
        {
            graphics.material.color = selectedColor;
        }
    }

    public void Die()
    {
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }

     public void MouseExit()
    {
        if (isYours) graphics.material.color = defaultColor;
    }

    public override string ToString()
    {
        return string.Format("{0}|{1},{2}", gameObject.name, type.ToString(), ChessManager.instance.TranslateNodeToCordinates(currentNode));
    }

}
