using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Node : MonoBehaviour
{
    //rune architecture
    private Rune myRune;

    //interaction
    private bool isMoving;
    //the speed of this block's movement
    [Range(0.1f, 10f)]
    [SerializeField] private float platformSpeed = 1;
    //team needs to be able to add an interactable object's end locations
    [SerializeField] private List<Vector3> positions = new List<Vector3>();
    private List<Node> connections = new List<Node>();
    private int currPos = 0;

    //pathfinder
    private List<Edge> edges = new List<Edge>(); //only for gizmos
    [SerializeField] private List<Node> excludedNodes = new List<Node>();
    //public so that pathfinder can easily access
    public List<Node> neighbours = new List<Node>();

    private Graph myGraph;

    private int c; //used for bfs: 0 - white; 1 - grey; 2 - black
    private Node pi; //used to find the path -> predecessor node

    //audio
    [SerializeField] private int blockLength; //0- short; 1 - medium; 2 - long

    //getters + setters
    public Rune rune{get{return myRune;} set{myRune = value;}}
    public GameObject getObject{get{return this.gameObject;}}
    public Graph graph{set{myGraph = value;}}
        //pathfinder
    public Node predNode { get { return pi; } set { pi = value; } }
    public int colour {get{return c;} set{c = value;}}


    // ========================== METHODS =================================
    //automatically connect interactables
    private void Awake(){
        //ensure there are no null nodes in node arrays
        if (neighbours.Count != 0) neighbours.RemoveAll(node => node == null);
    }

    private void Start(){
        if (positions.Count != 0){
            findConnections();
            foreach (Node n in connections){
                if (n!= null) {
                    n.tag = this.tag;
                }
            }
        }
    }

    private void findConnections(){ //adjusted from free code below
        foreach (Vector3 vec in positions){
            foreach(Vector3 v in myGraph.neighbourDir){
                Debug.Log("checking pos");
                Node n = myGraph.findObjectNodeAt(vec + v);
                if (n != null && !connections.Contains(n) && !excludedNodes.Contains(n))
                    connections.Add(n);
            }
        }
    }

    public void moveToNext(){ //only for up+down, side<->side blocks
        if(!isMoving){
            Vector3 a = this.transform.parent.transform.position;
            currPos = currPos++ >= positions.Count-1 ? 0 : currPos++;
            // Debug.Log(currPos);
            StartCoroutine(moveFromTo(a, positions[currPos], platformSpeed));
        }
    }

    IEnumerator moveFromTo(Vector3 a, Vector3 b, float speed){
        removeAdjNeighbours();
        isMoving = true;
        float step = (speed / (a - b).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t < 1.0f){
            t += step; // Goes from 0 to 1, incrementing by step each time
            this.transform.parent.transform.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        this.transform.parent.transform.position = b;
        findAdjNeighbours();
        isMoving = false;
    }

    private void removeAdjNeighbours(){
        foreach (Node n in connections){
            if (n != null) n.neighbours.Remove(this);
        }
        neighbours.Clear();
    }

    private void findAdjNeighbours(){ //adjusted from free code below
        foreach(Vector3 v in myGraph.neighbourDir){
            Node n = myGraph.findObjectNodeAt(transform.parent.transform.position + v);
            if (n != null && !neighbours.Contains(n) && !excludedNodes.Contains(n)){
                neighbours.Add(n);
                n.neighbours.Add(this);
            }
        }
    }

    public void playSound(AudioManager am){
        if(blockLength < 3 && blockLength>=0)
            am.playBlockSound(blockLength);
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
        foreach(Vector3 v in myGraph.neighbourDir){
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
     private float gizmoRadius = 0.1f;
     private Color defaultGizmoColor = Color.black;
     private Color selectedGizmoColor = Color.blue;
     private Color inactiveGizmoColor = Color.gray;
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