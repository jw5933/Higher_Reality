using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    private GameObject nodeOrPlayer = null;
    [SerializeField] private GameObject myRune3d;
    [SerializeField] private GameObject myRune2d;
    [SerializeField] private GameObject myRuneObj;
    public List<Node> interactableGroup = new List<Node>();

    public GameObject currObj{get{return nodeOrPlayer;} set{nodeOrPlayer = value;}}

    public void moveTo(Vector3 dest, bool moveToPlayer, bool on2d){
        this.transform.position = dest;
        this.transform.parent = nodeOrPlayer.transform;
        changeRune(moveToPlayer, on2d);
    }

    public void changeRune(bool onPlayer, bool on2d){
        if (on2d) setRune(myRune2d);
        else if (onPlayer) setRune(myRune3d);
        else setRune(myRuneObj);
        
    }
    
    private void setRune(GameObject rune){
        myRune3d.SetActive(false);
        myRune2d.SetActive(false);
        myRuneObj.SetActive(false);
        rune.SetActive(true);
    }

    private void Start(){
        //snap the rune to its location
        Graph g = FindObjectOfType<Graph>();
        nodeOrPlayer = g.findClosestNodeAt(transform.position).gameObject;
        moveTo(nodeOrPlayer.transform.position, false, false);
        ((Node)nodeOrPlayer.GetComponent(typeof(Node))).rune = this;
    }
}
