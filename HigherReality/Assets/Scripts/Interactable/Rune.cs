using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    private GameObject nodeOrPlayer = null;
    [SerializeField] private GameObject myRuneHover;
    [SerializeField] private GameObject myRuneObj;
    public Material playerMaterial;
    public List<Node> interactableGroup = new List<Node>();

    public GameObject currObj{get{return nodeOrPlayer;} set{nodeOrPlayer = value;}}

    private void Start(){
        //snap the rune to its location
        Graph g = FindObjectOfType<Graph>();
        nodeOrPlayer = g.findClosestNodeAt(transform.position).gameObject;
        moveTo(nodeOrPlayer.transform.position, false);
        ((Node)nodeOrPlayer.GetComponent(typeof(Node))).rune = this;
    }

    public void moveTo(Vector3 dest, bool moveToPlayer){
        this.transform.position = dest;
        this.transform.parent = nodeOrPlayer.transform;
        changeRune(moveToPlayer);
    }

    public void changeRune(bool onPlayer){
        if (onPlayer) setRune(myRuneHover);
        else setRune(myRuneObj);
        
    }
    
    private void setRune(GameObject rune){
        myRuneHover.SetActive(false);
        myRuneObj.SetActive(false);
        rune.SetActive(true);
    }
}
