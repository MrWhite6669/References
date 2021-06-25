using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TCPWhite;
using System.IO;

public class ChessManager : MonoBehaviour
{

    #region Singleton
    public static ChessManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Figures")]
    GameObject figures; //Parent of the figures
    public Vector3 moveOffset;
    public bool debug;
    string debugLog;

    public delegate void OnMatchEnded();
    public delegate void OnFigureMoved();
    public OnMatchEnded onMatchEnded;
    public OnFigureMoved onFigureMoved;


    public Figure selectedFigure;
    public Figure king;
    private string alfabet = " abcdefghi";

    [Header("Nodes")]
    public List<Node> nodes = new List<Node>();

    [Header("MatchInfo")]
    public int turnIndex = 1;
    public bool isYourTurn = false;
    public bool check = false;

    void Start()
    {
        LoadNodes();
        LoadFigures();
        //DebugCheckNodes();
    }

    public void ThreatenNode(Node node)
    {
        CheckPhase.instance.nodeSet.Add(node);
    }

    public void LoadFigures() // Get figures parent
    {
        figures = transform.Find("Figures").gameObject;
    }

    public void Check()
    {
        check = true;
        king.currentNode.HighlightRed();
    }

    public void Unthreaten()
    {
        foreach (Node n in nodes) n.UnThreaten();
    }
    
    public void Checkmate()
    {
        ServerListener.instance.SendMessageToServer(new Message(ID.Mate));
        Manager.instance.ShowMessagePanel("Checkmate!", Color.cyan, 3);
        Manager.instance.ShowLoadingPanel();
        check = false;
    }

    public void LoadNodes() // Load nodes and give them indexed position
    {
        Transform nodesObject = transform.Find("Nodes");
        int x = 9;
        int y = 1;
        for (int i = 0; i < nodesObject.childCount; i++)
        {
            x--;
            if (x < 1) // When node in line y gets index 1 , next node will be in next line y
            {
                x = 8;
                y++;
            }
            nodesObject.GetChild(i).GetComponent<Node>().posX = x;
            nodesObject.GetChild(i).GetComponent<Node>().posY = y;
            nodes.Add(nodesObject.GetChild(i).GetComponent<Node>());
        }
    }

    public void DebugCheckNodes() // Check nodes positions [DEBUG ONLY]
    {
        foreach (Node n in nodes)
        {
            Debug.Log(TranslateNodeToCordinates(n));
        }
    }

    public void EndMatch() // End the match
    {
        Manager.instance.BackToMenu();
        Manager.instance.HideTurnPanel();
        UnlightNodes();
        onMatchEnded.Invoke();
    }

    public Node FindNodeByCordinates(string s) // Find node by typed cordinates f.e.: A5 = 15 and D8 = 48
    {
        int y = int.Parse(s[1].ToString());
        int x = 0;
        int index = 0;
        foreach (char c in alfabet)
        {
            if (c == s[0]) x = index;
            index++;
        }
        return FindNode(x, y);
    }

    public string TranslateNodeToCordinates(Node node) // Gets cordinates by selected node f.e.: 15 = A5 and 48 = D8
    {
        string x = alfabet[node.posX].ToString();
        string y = node.posY.ToString();

        return x + y;
    }

    public Node FindNode(int x, int y) // Find node by indexed position x y
    {
        foreach (Node n in nodes)
        {
            if (n.posX == x && n.posY == y) return n;
        }
        return null;
    }

    public void SelectFigure(Figure figure)
    {
        UnlightNodes();
        selectedFigure = figure;
        DebugPrint(selectedFigure.ToString());
        HighlightNodes();
    }

    public void Move(Node from, Node to) // Move figure from one node to another
    {
        Figure temp = from.figure;
        if (to.figure != null)
        {
            to.figure.Die();
            to.figure = temp;
        }
        temp.transform.position = to.transform.position + moveOffset;
        temp.timesMoved++;
        onFigureMoved.Invoke();
        from.figure = null;
        from.Empty = true;
        to.Empty = false;
        UnlightNodes();
    }

    public void RequestMove(Node nodeToMoveOn) // Send request to server if you can move
    {
        string move = string.Format("{0}:{1}", TranslateNodeToCordinates(selectedFigure.GetComponent<Figure>().currentNode), TranslateNodeToCordinates(nodeToMoveOn));
        Message msg = new Message(ID.Move);
        msg.data.Add("position", move);
        ServerListener.instance.SendMessageToServer(msg);
    }

    public Figure GetSelected()
    {
        return selectedFigure;
    }

    private void HighlightNodes()
    {
        selectedFigure.highlighterManager.Highlight();
    }

    private void UnlightNodes()
    {
        foreach (Node n in nodes) if (n != null) n.UnLight();
    }

    public void DebugPrint(string text)
    {
        if (debug) print(text);
        debugLog += "\n" + text;
    }

    public void SaveLog()
    {
        using (StreamWriter sw = new StreamWriter("DebugLog.txt"))
        {
            sw.WriteLine(debugLog);
        }
    }


}
