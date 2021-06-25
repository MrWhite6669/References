using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour {

    [Header("DEBUG")]
    public bool occupiedByEnemy;

    [Header("Data")]
    public int index; // Position in the set
    public int set; // Index of the set

    [Header("Settings")]
    public Vector3 Size; //Size of the Gizmos
    public bool attackOnly;
    public bool moveOnly;
    public int maxMoves = 1000;
    private LayerMask mask;
    public bool Empty { get; set; }

    public Node nodeToHighlight;
    
    public void CheckUnder() //Check for things under the highlighter
    {
        RaycastHit info;
        bool hit = Physics.Raycast(transform.position, new Vector3(0,-1,0),out info,20f, mask);
        if (hit)
        {
            nodeToHighlight = info.collider.GetComponent<Node>();
            Empty = false;
        }
        else Empty = true;

        //DebugShowData();
    }

    public void DebugShowData() // Show data of the highlighter [DEBUG ONLY]
    {
            Debug.Log(string.Format("Node S:{0} I:{1} , Occupied = {2} , Enemy = {3}", set, index, IsOccupied, OccupiedByEnemy));
    }

    public void SetMask(LayerMask mask)
    {
        this.mask = mask;
    }

    public void Highlight() //Highlith the node under
    {
        if (nodeToHighlight != null) nodeToHighlight.Highlight();
    }

    public void HighlightRed() //Highlith the node under to red
    {
        if (nodeToHighlight != null) nodeToHighlight.HighlightRed();
    }

    public void Unlight()
    {
        if (nodeToHighlight != null) nodeToHighlight.UnLight();
    }

    public bool IsOccupied
    {
        get
        {
            if (Empty || nodeToHighlight.Empty) return false;
            return true;
        }
    }

    public bool OccupiedByEnemy
    {
        get
        {
            if (!Empty && IsOccupied && !nodeToHighlight.figure.isYours) { occupiedByEnemy = true;return true; }
            return false;
        }
    }

    public bool IsThreatend
    {
        get
        {
            if (nodeToHighlight != null && nodeToHighlight.threatend) return true;
            return false;
        }
    }

    public bool HasKing
    {
        get
        {
            if (nodeToHighlight != null && nodeToHighlight.figure.IsKing) return true;
            return false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (attackOnly) Gizmos.color = Color.yellow;
        if (moveOnly) Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Size);
    }



}
