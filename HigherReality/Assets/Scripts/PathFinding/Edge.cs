using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Node neighbour;
    public bool isEnabled;

    public Edge(Node n, bool b){
        this.neighbour = n;
        this.isEnabled = b;
    }
}
