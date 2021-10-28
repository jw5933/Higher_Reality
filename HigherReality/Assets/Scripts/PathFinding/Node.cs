using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{
    //rune architecture
    private Rune myRune;
    public Rune rune{get{return myRune;} set{myRune = value;}}

    //interaction
    private bool isMoving;
    //the speed of this block's movement
    [SerializeField] private float platformSpeed;
    private Node interactableObj;
    //team needs to be able to add an interactable object's connecting blocks and locations
    [SerializeField] private List<Vector3> positions = new List<Vector3>();
    [SerializeField] private List<Node> connections = new List<Node>();
    private int currConnection = 0;

    public GameObject getObject{get{return this.gameObject;}}
    public Node interactable{get{return interactableObj;} set {interactableObj = value;}}



    //pathfinder
    private List<Edge> edges = new List<Edge>(); //only for gizmos
    // private List<Node> neighbours = new List<Node>();
    public List<Node> neighbours = new List<Node>();
    public List<Node> getNeighbours {get{return neighbours;}} //performance is a bit low..?
    [SerializeField] private List<Node> excludedNodes = new List<Node>();

    private Graph myGraph;
    public Graph graph{set{myGraph = value;}}

    private int c; //used for bfs: 0 - white; 1 - grey; 2 - black
    private Node pi; //used to find the path -> predecessor node

    // invoked when Player enters this node
    // public UnityEvent gameEvent;

    //let other classes get set the pNode
    public Node predNode { get { return pi; } set { pi = value; } }
    public int colour {get{return c;} set{c = value;}}

    public static Vector3[] neighbourDir = //{east, west, north, south}
    {new Vector3(1f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(0f, 0f, 1f), new Vector3(0f, 0f, -1f)};


    //automatically connect interactables
    private void Awake(){
        foreach (Node n in connections){
            if (n!= null) n.interactableObj = this;
        }
    }

    public void moveToNext(){
        if(!isMoving){
            Vector3 a = this.transform.parent.transform.position;
            currConnection = currConnection++ >= positions.Count-1 ? 0 : currConnection++;
            // Debug.Log(currConnection);
            StartCoroutine(moveFromTo(a, positions[currConnection], platformSpeed));
        }
    }

    IEnumerator moveFromTo(Vector3 a, Vector3 b, float speed){
        removeNeighbours();
        isMoving = true;
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f){
            t += step; // Goes from 0 to 1, incrementing by step each time
            this.transform.parent.transform.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        this.transform.parent.transform.position = b;
        updateNeighbour(connections[currConnection]);
        isMoving = false;
    }

    private void removeNeighbours(){
        foreach (Node n in connections){
            if (n != null) n.neighbours.Remove(this);
        }
        neighbours.Clear();
    }

    private void updateNeighbour(Node n){
        if (n == null) return;
        n.neighbours.Add(this);
        neighbours.Add(n);
    }



    //==========================================            free use code           ==========================================
    /*
    * Copyright (c) 2020 Razeware LLC
    * 
    * Permission is hereby granted, free of charge, to any person obtaining a copy
    * of this software and associated documentation files (the "Software"), to deal
    * in the Software without restriction, including without limitation the rights
    * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    * copies of the Software, and to permit persons to whom the Software is
    * furnished to do so, subject to the following conditions:
    * 
    * The above copyright notice and this permission notice shall be included in
    * all copies or substantial portions of the Software.
    *
    * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
    * distribute, sublicense, create a derivative work, and/or sell copies of the 
    * Software in any work that is designed, intended, or marketed for pedagogical or 
    * instructional purposes related to programming, coding, application development, 
    * or information technology.  Permission for such use, copying, modification,
    * merger, publication, distribution, sublicensing, creation of derivative works, 
    * or sale is expressly withheld.
    *    
    * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    * THE SOFTWARE.
    */

    public void findNeighbours(){
        foreach(Vector3 v in neighbourDir){
            Node n = myGraph.findNodeAt(transform.position + v);
            // Debug.Log(n.name);
            // if (!neighbours.Contains(n)) Debug.Log("does not contain this node");
            if (n != null && !neighbours.Contains(n) && !excludedNodes.Contains(n)){
                Edge newEdge = new Edge(n, true);
                edges.Add(newEdge);
                neighbours.Add(n);
            }
        }
    }

    // ========================== gizmos stuff ========================
        // gizmo colors
    [SerializeField] private float gizmoRadius = 0.1f;
    [SerializeField] private Color defaultGizmoColor = Color.black;
    [SerializeField] private Color selectedGizmoColor = Color.blue;
    [SerializeField] private Color inactiveGizmoColor = Color.gray;
    //drawing for visuals
    private void OnDrawGizmos(){
        Gizmos.color = defaultGizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }

    // draws a sphere gizmo in a different color when selected
    private void OnDrawGizmosSelected(){
        Gizmos.color = selectedGizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);

        // draws a line to each neighbor
        foreach (Edge e in edges)
        {
            if (e.neighbour != null)
            {
                Gizmos.color = (e.isEnabled) ? selectedGizmoColor : inactiveGizmoColor;
                Gizmos.DrawLine(transform.position, e.neighbour.transform.position);
            }
        }
    }
}