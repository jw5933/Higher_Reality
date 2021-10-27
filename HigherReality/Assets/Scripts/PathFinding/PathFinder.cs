using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private int maxCount = 100;
    int count = 0;

    Graph graph; //to get all the nodes
    [SerializeField]private Node sNode; //the node the player is on; start node
    [SerializeField]private Node eNode; //the place the player wants to go; end node

    [SerializeField]private Queue<Node> q = new Queue<Node>();
    [SerializeField]private Queue<Node> explored = new Queue<Node>();
    private Stack<Node> pathStack  = new Stack<Node>();
    [SerializeField]private List<Node> path = new List<Node>();

    public bool foundEnd;

    public Node startNode{set {sNode = value;}}
    public Node endNode{get{return eNode;} set {eNode = value;}}

    public List<Node> findPath(Node currNode, Node destNode){
        Debug.Log("Curr node: " + currNode.name + " End node: " + destNode.name);
        sNode = currNode;
        eNode = destNode;
        Debug.Log("Curr node: " + sNode.name + " End node: " + eNode.name);
        resetNodes();
        pathBFS();

        return path;
    }

    private void pathBFS(){ //iterate through all possible paths, stopping when we've reached the goal.
        sNode.colour = 1;
        sNode.predNode = null;

        q.Enqueue(sNode);
        explored.Enqueue(sNode);
        while(!foundEnd && q.Count != 0 && count < maxCount){
            count++;
            Node n = q.Dequeue();
            foreach (Node v in n.neighbours){
                if (v.colour == 0){
                    // Debug.Log(n.name + "'s neighbour " + v.name + " has been reached");
                    v.colour = 1;
                    v.predNode = n;
                    if(!explored.Contains(v)) explored.Enqueue(v);

                    if(!foundEnd && v == eNode){ //stop when we've found the destination
                        foundEnd = true;
                        Debug.Log("Found path now adding path");
                        addPath();
                        return;
                    }
                    q.Enqueue(v);
                }
            }
            n.colour = 2;
            if (count >= maxCount){
                if(!foundEnd){
                    path.Clear();
                    return;
                }
                else {
                    if (pathStack.Count == 0) addPath();
                    return;
                }
            }
            if(!foundEnd){
                path.Clear();
                return;
            }
        }
    }

    private void addPath(){
        path.Clear();
        Node n = eNode;
        while(n != null){
            pathStack.Push(n);
            n = n.predNode;
        }
        pathStack.Pop(); //don't need to pathStack to current node
        while(pathStack.Count!=0){
            path.Add(pathStack.Pop());
        }
        resetNodes();
    }

    private void resetNodes(){
        q.Clear();
        while (explored.Count != 0){
            Node n = explored.Dequeue();
            Debug.Log("reseting the values of node " + n.name);
            n.predNode = null;
            n.colour = 0;
        }
        foundEnd = false;
    }

    //drawing the pathStack
    // private void OnDrawGizmos(){
    //     if (foundEnd){
    //         foreach (Node n in pathStack){
    //             Gizmos.color = Color.green;
    //             Gizmos.DrawCube(n.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
    //             Gizmos.color = Color.yellow;
    //                 if (n.predNode != null)
    //                     Gizmos.DrawLine(n.transform.position, n.predNode.transform.position);
    //         }
    //     }
    // }
}
