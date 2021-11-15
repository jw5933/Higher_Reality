using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIAlpha : MonoBehaviour
{
    private Image myImage;
    private PlayerMovement player;
    [SerializeField]private Animator sceneAnimator;
    private void Awake(){

        myImage = GetComponent<Image>();
        player = FindObjectOfType<PlayerMovement>();
    }
    public void setAlpha(float n){
        Color colour = myImage.color;
        colour.a = n;
        myImage.color = colour;
    }

    public void endStartAnimation(){
        player.setcontrolEnabled = true;
        if (sceneAnimator != null) sceneAnimator.enabled = false;
    }

    public void stopSceneAnim(){
        sceneAnimator.speed = 0;
    }
}
