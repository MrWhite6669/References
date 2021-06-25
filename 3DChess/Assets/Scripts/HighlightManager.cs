using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TCPWhite;

public class HighlightManager : MonoBehaviour
{

    List<Highlighter> highlighters = new List<Highlighter>();
    public List<Highlighter> toHighlight = new List<Highlighter>();
    public LayerMask layerMask;

    private Dictionary<int, Highlighter[]> sets = new Dictionary<int, Highlighter[]>();

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++) //Gets the highlighters 
        {
            highlighters.Add(transform.GetChild(i).GetComponent<Highlighter>());
        }
        SetMasks();
        LoadSets();
        ChessManager.instance.onFigureMoved += CheckSurroundings;
        Manager.instance.onMatchStarted += CheckSurroundings;
    }

    void LoadSets()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            AddToSet(transform.GetChild(i).GetComponent<Highlighter>());
        }
    }

    void AddToSet(Highlighter sender) //Add highlighter to one of the sets, if there isnt any it creates one
    {
        if (!sets.ContainsKey(sender.set)) sets.Add(sender.set, new Highlighter[8]);
        sets[sender.set][sender.index] = sender;
    }

    void SetMasks() //Set the masks to all Highlighters
    {
        foreach (Highlighter h in highlighters) h.SetMask(layerMask);
    }

    public void Highlight() // Highlight the nodes that you can move on
    {
        foreach (Highlighter h in toHighlight)
        {
            if (ChessManager.instance.isYourTurn)
            {
                h.Highlight();
                if (!h.moveOnly && h.OccupiedByEnemy)
                {
                    h.HighlightRed();
                   
                }

            }
        }
    }

    public void CheckSurroundings() //Check for surrounding nodes that you can move on
    {
        toHighlight.Clear();

        foreach (int i in sets.Keys)
        {
            ChessManager.instance.DebugPrint(string.Format("----------------------------------------\n [Highlighters]Checking set {0} on {1} \n----------------------------------------", i, transform.parent.gameObject.name));
            if(CheckPhase.instance.initiated)CheckPhase.instance.CheckSet(sets[i],this);
            else CheckSet(sets[i]);
        }
    }

    public void CheckSet(Highlighter[] set) //Check selected set
    {
        bool ended = false;
        Figure figure = GetComponentInParent<Figure>();

        foreach (Highlighter h in set)
        {
            if (figure != null && h != null)
            {
                h.CheckUnder();
                if (h.OccupiedByEnemy)
                {
                    ChessManager.instance.DebugPrint(string.Format("[Highlighters]Highlighter {0} in set {1} is occupied by enemy.", h.index,h.set));
                    ended = true;

                    if (h.HasKing && figure.isYours)
                    {
                        ChessManager.instance.DebugPrint(string.Format("[Highlighters]This highlither has a king: s{0}:{1}i {2} ", h.set, h.index, h.transform.parent.parent.name));
                        foreach (Highlighter x in sets[h.set])
                        {
                            if (x != null && x.nodeToHighlight != null)
                            {
                                Message msg2 = new Message(ID.ThreatenNode);
                                msg2.data.Add("node", ChessManager.instance.TranslateNodeToCordinates(x.nodeToHighlight));
                                ServerListener.instance.SendMessageToServer(msg2);
                            }
                        }
                        Message msg = new Message(ID.Check);
                        msg.data.Add("attacker", figure.gameObject.name);
                        ServerListener.instance.SendMessageToServer(msg);
                        
                    }

                    if (!h.moveOnly) toHighlight.Add(h);
                    return;

                }
                else if (h.IsOccupied)
                {
                    ChessManager.instance.DebugPrint(string.Format("[Highlighters]Highlighter {0} in set {1} is occupied.", h.index,h.set));
                    ended = true;
                    return;
                }
                else if (ChessManager.instance.check)
                {
                    if (!ended)
                    {
                        if (figure.IsKing && h.IsThreatend)
                        {
                            print("This node is threatend!");
                            ended = true;
                        }
                        if (!figure.IsKing && !h.IsThreatend)
                        {   
                            ended = true;
                        }
                    }

                }


                if (figure.timesMoved >= h.maxMoves)
                {
                    ChessManager.instance.DebugPrint(string.Format("[Highlighters]Highlighter {0} in set {1} moved its max moves.({2}/{3})",h.index,h.set,figure.timesMoved, h.maxMoves));
                    return;
                }
                if (!ended && !h.attackOnly)
                {
                    ChessManager.instance.DebugPrint(string.Format("[Highlighters]Highlighter {0} on set {1} is free to move.", h.index , h.set));
                    toHighlight.Add(h);
                }

            }
        }
    }

}
