using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool togglePlatformColours;
    [SerializeField] private Animator runeAnimator;
    AudioManager audioManager;
    PlayerMovement player;
    [SerializeField]private Material playerDefaultMat;
    [SerializeField]private Material platformDefaultMat;

    Node currNode;
    Rune currRune;
    CamSwap camSystem;
    [SerializeField] private Vector3 scaleDownBy = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 scaleChange {get{return scaleDownBy;}}
    public Rune rune{get{return currRune;}}

    // Start is called before the first frame update
    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<PlayerMovement>();
        camSystem = FindObjectOfType<CamSwap>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            currNode = player.node;
            currRune = player.rune;

            interactWithRune();
        }
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)){
            currNode = player.node;
            currRune = player.rune;
            checkRune();
        }
    }

    void checkRune(){
        if (currNode == null) return;
        if (currRune == null){
            pickUpRune();
        }
        else{
            handleDrop();
        }
    }

    void pickUpRune(){
        if (currNode == null) return;
        if (currNode.rune != null){
            player.rune = currNode.rune;
            currRune = player.rune;
            currNode.rune = null; //remove the rune from the node
            currRune.currObj = player.gameObject;
            
            //set the player's material to the rune's colour
            player.mat = currRune.playerMaterial;
            changeInteractableColour(currRune, currRune.playerMaterial);

            audioManager.playRuneSound(0);
            currRune.moveTo(player.runePos, true);
        }
    }

    void interactWithRune(){
        if (currNode == null || currRune == null) return;
        // if(currNode.tag == currRune.tag){
            foreach (Node n in currRune.interactableGroup){
                Debug.Log(n.name);
                n.moveToNext();
                n.playSound(audioManager);
            }
        // }
    }

    void handleDrop(){
        if (currNode == null || currRune == null) return;
        //place the rune on the current node
        if (currNode.rune !=null){
            swapDrop(); //cannot drop the rune if there is already one there
            return;
        }
        currNode.rune = currRune;
        player.rune = null;
        currRune.currObj = currNode.gameObject;
        //set the player's material to the default player material
        player.mat = playerDefaultMat;
        changeInteractableColour(currRune, platformDefaultMat);

        audioManager.playRuneSound(1);
        currRune.moveTo(currNode.transform.position, false);
    }

    void swapDrop(){
        Rune temp = player.rune;
        player.rune = currNode.rune;
        currRune = player.rune;
        currNode.rune = temp; //remove the rune from the node
        currRune.currObj = player.gameObject;
        currNode.rune.currObj = currNode.gameObject;
        //set the player's material to the rune's colour
        player.mat = currRune.playerMaterial;
        changeInteractableColour(currRune, currRune.playerMaterial);
        changeInteractableColour(currNode.rune, platformDefaultMat);

        audioManager.playRuneSound(0);
        currRune.moveTo(player.runePos, true);
        temp.moveTo(currNode.transform.position, false);
    }

    public void playRuneAnim(AnimatorController newController){
        // Debug.Log(newMotion);
        runeAnimator.runtimeAnimatorController = newController;
        runeAnimator.SetTrigger("runeUI");
    }

    void changeInteractableColour(Rune r, Material m){
        if (!togglePlatformColours) return;
        foreach(Node n in r.interactableGroup){
            n.mat = m;
        }
    }
}
