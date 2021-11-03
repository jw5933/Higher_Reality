using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwap : MonoBehaviour
{
    [SerializeField] GameObject cam3d;
    [SerializeField] List<GameObject> cams2d;
    [SerializeField] GameManager gameManager;

    int currCam = 0;
    bool onCams2d = false;
    public bool isOn2d{get{return onCams2d;}}

    [SerializeField] private List<Rune> runes;

    void Update()
    {
        if(onCams2d && Input.GetKeyDown(KeyCode.W)){
            onCams2d = false;

            //change how runes look for visibility
            foreach(Rune r in runes){
                r.changeRune(false, onCams2d);
            }
            gameManager.rune?.changeRune(true, onCams2d);
            
            cam3d.SetActive(true);
            foreach(GameObject cam in cams2d){
                cam.SetActive(false);
            }
            
        }
        else if (!onCams2d && Input.GetKeyDown(KeyCode.S)){
            onCams2d = true;

            //change how runes look for visibility
            foreach(Rune r in runes){
                r.changeRune(false, onCams2d);
            }

            cam3d.SetActive(false);
            cams2d[currCam].SetActive(true);
        }
        else if (onCams2d && Input.GetKeyDown(KeyCode.A)){
            if (currCam == 0) currCam = cams2d.Count-1;
            else currCam--;
            swapCam();
        }
        else if (onCams2d && Input.GetKeyDown(KeyCode.D)){
            if (currCam == cams2d.Count-1) currCam = 0;
            else currCam++;
            swapCam();
        }
    }


    void swapCam(){
        cam3d.SetActive(false);
        foreach(GameObject cam in cams2d){
            cam.SetActive(false);
        }

        cams2d[currCam].SetActive(true);
    }
}
