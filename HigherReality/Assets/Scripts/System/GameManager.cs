using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerMovement player;
    [SerializeField] Node currNode;
    [SerializeField] Rune currRune;
    [SerializeField] Node interactable;


    // Start is called before the first frame update
    void Start()
    {
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
    }

    void checkRuneExists(){
        if (currNode == null) return;
        if (currRune == null){
            if (currNode.rune != null){
                player.rune = currNode.rune;
                currRune = player.rune;
                currNode.rune = null; //remove the rune from the node
                currRune.debugObj = player.gameObject;
            }
            
        }
        else{
            if(currNode.tag == currRune.getObject.tag){
                if (currNode.getInteractable != null){
                    interactable = currNode.getInteractable;
                    interactable.moveToNext();
                }
                else currNode.moveToNext();
            }
        }
    }
}
