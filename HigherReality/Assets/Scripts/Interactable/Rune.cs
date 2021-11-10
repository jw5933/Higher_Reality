using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    private GameObject nodeOrPlayer = null;
    // [SerializeField] private GameObject myRuneObj;
    public Material playerMaterial;
    public List<Node> interactableGroup = new List<Node>();

    public GameObject currObj{get{return nodeOrPlayer;} set{nodeOrPlayer = value;}}
    private GameManager gm;
    private Vector3 myScale;
    
    private void Awake(){
        myScale = this.transform.localScale;
    }
    private void Start(){
        //snap the rune to its location
        Graph g = FindObjectOfType<Graph>();
        nodeOrPlayer = g.findClosestNodeAt(transform.position).gameObject;
        moveTo(nodeOrPlayer.transform.position, false);
        ((Node)nodeOrPlayer.GetComponent(typeof(Node))).rune = this;
        gm = FindObjectOfType<GameManager>();
    }

    public void moveTo(Vector3 dest, bool moveToPlayer){
        changeRune(moveToPlayer);
        this.transform.position = dest;
        this.transform.parent = nodeOrPlayer.transform;
    }

    public void changeRune(bool onPlayer){
        if (onPlayer) this.transform.localScale -= gm.scaleChange;
        else this.transform.localScale = myScale;
        
    }
}
