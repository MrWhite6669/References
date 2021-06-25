using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CheckPhase : MonoBehaviour
{
    #region Singleton
    public static CheckPhase instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject turnPanel;
    public Text turnText;
    public Color checkColor;
    public Color defaultColor;
    public Figure attacker;
    public List<Node> nodeSet = new List<Node>();

    public bool initiated = false;

    public void Initiate(string atck)
    {
        print(atck);
        attacker = GameObject.Find(atck).gameObject.GetComponent<Figure>();
        initiated = true;
        turnPanel.GetComponent<Image>().color = checkColor;
        turnText.text = "Check";
    }

    public void Finish()
    {
        attacker = null;
        initiated = false;
        turnPanel.GetComponent<Image>().color = defaultColor;
        turnText.text = "YOUR TURN";
    }

    public void CheckSet(Highlighter[] set, HighlightManager hm)
    {
        Figure figure = hm.GetComponentInParent<Figure>();
        if (figure == null) return;

        if (figure.isYours)
        {
            if (figure.IsKing)
            {
                foreach (Node attackerH in nodeSet)
                {
                    foreach (Highlighter figureH in set)
                    {
                        if (figureH == null) return;
                        figureH.CheckUnder();
                        if (!figureH.Empty && figureH.nodeToHighlight != attackerH && figureH.OccupiedByEnemy) hm.toHighlight.Add(figureH);
                    }
                }
            }
            else
            {
                foreach (Node attackerH in nodeSet)
                {
                    foreach (Highlighter figureH in set)
                    {
                        if (figureH == null) return;
                        figureH.CheckUnder();
                        if (!figureH.Empty)
                        {
                                if (attackerH == figureH.nodeToHighlight && figureH.maxMoves > figure.timesMoved && figureH.moveOnly)
                                {
                                    hm.toHighlight.Add(figureH);
                                }
                                else if (figureH.OccupiedByEnemy && figureH.nodeToHighlight.figure == attacker && !figureH.moveOnly) hm.toHighlight.Add(figureH);
                        }

                    }
                }
            }

            if (Manager.instance.NothingToHighlight) ChessManager.instance.Checkmate();

        }
    }

}
