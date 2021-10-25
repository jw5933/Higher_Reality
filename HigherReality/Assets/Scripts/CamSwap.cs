using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwap : MonoBehaviour
{
    [SerializeField] GameObject cam3d;
    [SerializeField] GameObject cam1;
    [SerializeField] GameObject cam2;
    [SerializeField] GameObject cam3;
    [SerializeField] GameObject cam4;
    [SerializeField] GameObject cam5;
    int currCam = 1;
    bool onCams2d = false;

    void Update()
    {
        if(onCams2d && Input.GetKeyDown(KeyCode.UpArrow)){
            onCams2d = false;
            cam3d.SetActive(true);
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
        }
        else if (!onCams2d && Input.GetKeyDown(KeyCode.DownArrow)){
            cam3d.SetActive(false);
            switch(currCam){
                case 1:
                cam1.SetActive(true);
                break;
                case 2:
                cam2.SetActive(true);
                break;
                case 3:
                cam3.SetActive(true);
                break;
                case 4:
                cam4.SetActive(true);
                break;
                case 5:
                cam5.SetActive(true);
                break;
            }
            onCams2d = true;
        }
        else if (onCams2d && Input.GetKeyDown(KeyCode.LeftArrow)){
            if (currCam == 1) currCam = 5;
            else currCam--;
            swapCam();
        }
        else if (onCams2d && Input.GetKeyDown(KeyCode.RightArrow)){
            if (currCam == 5) currCam = 1;
            else currCam++;
            swapCam();
        }
    }


    void swapCam(){
        cam3d.SetActive(false);
        cam1.SetActive(false);
        cam2.SetActive(false);
        cam3.SetActive(false);
        cam4.SetActive(false);
        cam5.SetActive(false);
        
        switch(currCam){
                case 1:
                    cam1.SetActive(true);
                break;
                case 2:
                    cam2.SetActive(true);
                break;
                case 3:
                    cam3.SetActive(true);
                break;
                case 4:
                    cam4.SetActive(true);
                break;
                case 5: 
                    cam5.SetActive(true);
                break;
            }
    }
}
