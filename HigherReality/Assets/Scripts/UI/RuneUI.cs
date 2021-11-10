using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneUI : MonoBehaviour
{
    private Image myImage;
    private void Awake(){
        myImage = GetComponent<Image>();
    }
    public void setAlpha(float n){
        Color colour = myImage.color;
        colour.a = n;
        myImage.color = colour;
    }
}
