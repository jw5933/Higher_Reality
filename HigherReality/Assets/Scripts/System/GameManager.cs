using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool togglePlatformColours;
    [SerializeField] private Animator runeAnimator;
    [SerializeField] private Animator sceneAnimator;
    [SerializeField] private AnimatorController endingAnimationController;
    
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
    private bool gameEnded;

    // Start is called before the first frame update
    void Awake(){
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<PlayerMovement>();
        camSystem = FindObjectOfType<CamSwap>();
    }

    // Update is called once per frame
    void Update()
    {
        currNode = player.node;
        if(gameEnded) return;
        if (Input.GetKeyDown(KeyCode.Space)){
            currRune = player.rune;

            interactWithRune();
        }
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)){
            currRune = player.rune;
            checkRune();
        }
    }

    private void LateUpdate(){
        if (!gameEnded && currNode?.tag == "Finish"){
            gameEnded = true;
            audioManager.playEndingSound();
            player.setcontrolEnabled = false;
            sceneAnimator.runtimeAnimatorController = endingAnimationController;
            sceneAnimator.enabled = true;
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
            changeInteractableColour(currRune, (currRune.platformMaterial != null? currRune.platformMaterial: currRune.playerMaterial));

            audioManager.playRuneSound(0);
            currRune.moveTo(player.runePos, true);
        }
    }

    void interactWithRune(){
        if (currNode == null || currRune == null) return;
            currRune.playSound(audioManager);
            foreach (Node n in currRune.interactableGroup){
                // Debug.Log(n.name);
                n.moveToNext();
            } 
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
        changeInteractableColour(currRune, (currRune.platformMaterial != null? currRune.platformMaterial: currRune.playerMaterial));
        changeInteractableColour(currNode.rune, platformDefaultMat);

        audioManager.playRuneSound(0);
        currRune.moveTo(player.runePos, true);
        temp.moveTo(currNode.transform.position, false);
    }

    public void playRuneAnim(AnimatorController newController){
        // Debug.Log(newMotion);
        if (runeAnimator == null) return;
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
