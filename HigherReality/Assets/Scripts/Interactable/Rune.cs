using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor.Animations;

public class Rune : MonoBehaviour
{
    //audio
    [Range(0,2)]
    [SerializeField] private int blockAudioLength; //0- short; 1 - medium; 2 - long
    [SerializeField]private RuntimeAnimatorController myUIController;
    // public Motion motion{get{return myMotion;}}
    private GameObject nodeOrPlayer = null;
    // [SerializeField] private GameObject myRuneObj;
    public Material playerMaterial;
    public Material platformMaterial;
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
        if (onPlayer){
            gm.playRuneAnim(myUIController);
            this.transform.localScale -= gm.scaleChange;
        }
        else this.transform.localScale = myScale;
        
    }
    public void playSound(AudioManager am){
        if(blockAudioLength < 3 && blockAudioLength>=0)
            am.playBlockSound(blockAudioLength);
    }
}
