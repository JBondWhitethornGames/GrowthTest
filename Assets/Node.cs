using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector3 coordinates;
    public Node previousNode;
    public Node nextNode;

    public Node()
    {
        coordinates = new Vector3();
        previousNode = null;
        nextNode = null;
    }

    public Node(Vector3 newCoordinates, Node previousNode = null)
    {
        coordinates = newCoordinates;
        this.previousNode = previousNode;
        nextNode = null;
    }
}
