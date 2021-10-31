using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Audio
    [SerializeField] AudioManager audioManager;
    //variables
    PlayerMovement player;
    Node currNode;
    Rune currRune;
    Node interactable;
    [SerializeField] private CamSwap camSystem;
    public Rune rune{get{return currRune;}}

    // Start is called before the first frame update
    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (player == null) player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            currNode = player.node;
            currRune = player.rune;

            checkRuneExists();
        }
        if (Input.GetKeyDown(KeyCode.D)){
            Debug.Log("checking d");
            if (!player.checkIsMoving){
                Debug.Log("checked d and player is not moving");
                currNode = player.node;
                currRune = player.rune;
                checkDrop();
            }
        }
    }

    void checkRuneExists(){
        if (currNode == null) return;
        if (currRune == null){
            if (currNode.rune != null){
                player.rune = currNode.rune;
                currRune = player.rune;
                currNode.rune = null; //remove the rune from the node
                currRune.currObj = player.gameObject;

                currRune.moveTo(player.runePos, true, camSystem.isOn2d);
            }
            
        }
        else interactWithRune();
    }

    void interactWithRune(){
        if(currNode.tag == currRune.tag){
            if (currNode.interactable != null){
                interactable = currNode.interactable;
                interactable.moveToNext();
                interactable.playSound(audioManager);
            }
            else {
                currNode.moveToNext();
                currNode.playSound(audioManager);
            }
        }
    }

    void checkDrop(){
        if (currNode == null || currRune == null) return;
        //place the rune on the current node
        currNode.rune = currRune;
        player.rune = null;
        currRune.currObj = currNode.gameObject;
        currRune.moveTo(currNode.transform.position, false, camSystem.isOn2d);
    }
}
