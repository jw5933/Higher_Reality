using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Graph : MonoBehaviour
{
    private List<Node> allNodes = new List<Node>();
    Node[] arr;

    //let level designer set the distance check for neighbouring nodes
    private static Vector3[] neighbourDirection; //{east, west, north, south}
    public Vector3[] neighbourDir{get{return neighbourDirection;}}

    [Range(0.01f,1f)]
    [SerializeField] private float maxMagnitude = 0.1f;

    [Range(0f,5f)]
    [SerializeField] private float maxNodeDistance = 1f;
    public float nodeDistance{get{return maxNodeDistance;}}
    
    void Awake(){
        allNodes = FindObjectsOfType<Node>().ToList();
        initNodes();
        arr = allNodes.ToArray();

        neighbourDirection = new Vector3[]//{east, west, north, south}
        {new Vector3(maxNodeDistance, 0f, 0f), 
        new Vector3(-maxNodeDistance, 0f, 0f), 
        new Vector3(0f, 0f, maxNodeDistance), 
        new Vector3(0f, 0f, -maxNodeDistance)};
    }
    // Start is called before the first frame update
    void Start()
    {
        arr = allNodes.ToArray();
        initNeighbours();
    }

    void initNodes(){
        foreach(Node n in allNodes){
            n.graph = this;
        }
    }

    // set neighbors for each Node; must run AFTER all Nodes are initialized
    private void initNeighbours()
    {
        foreach (Node n in allNodes){
            if (n != null){
                n.findNeighbours();
            }
        }
    }
    // locate the specific Node at target position within rounding error
    public Node findNodeAt(Vector3 pos)
    {
        foreach (Node n in allNodes)
        {
            Vector3 diff = n.transform.position - pos;

            if (diff.sqrMagnitude < 0.01f)
            {
                return n;
            }
        }
        return null;
    }
    
    public Node findClosestNodeAt(Vector3 pos){
        // Debug.Log(pos.x + " " + pos.y + " " + pos.z);
        Node closestNode = allNodes[0];
        float closestDistanceSqr = Mathf.Infinity;

        foreach(Node n in arr){
            Vector3 diff = n.transform.position - pos;

            Vector3 nodeScreenPos = Camera.main.WorldToScreenPoint(n.transform.position);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
            diff = nodeScreenPos - screenPos;

            if (diff.sqrMagnitude < closestDistanceSqr)
            {
                closestNode = n;
                closestDistanceSqr = diff.sqrMagnitude;
            }
        }
        return closestNode;
    }

    public Node findObjectNodeAt(Vector3 pos)
    {
        foreach (Node n in allNodes){
            Vector3 diff = n.transform.parent.transform.position - pos;
            if (diff.sqrMagnitude < maxMagnitude){
                return n;
            }
        }
        return null;
    }
}


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