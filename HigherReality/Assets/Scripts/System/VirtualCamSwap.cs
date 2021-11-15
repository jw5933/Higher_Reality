using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCamSwap : MonoBehaviour
{
    Canvas runeCanvas;
    // int camSaved = 0;
    [SerializeField] List<GameObject> allCams;
    List <CinemachineVirtualCamera> virtualCams = new List<CinemachineVirtualCamera>();
    GameManager gameManager;

    int currCam = 0;
    int prevCam = 0;
    // bool onCamSaved = false;
    private Rune[] runes;
    public GameObject currCamera{get{return allCams[currCam];}}

    void Awake(){
        runeCanvas = FindObjectOfType<Canvas>();
        gameManager = FindObjectOfType<GameManager>();
        runes = FindObjectsOfType<Rune>();
        foreach(GameObject o in allCams){
            virtualCams.Add(o.GetComponent<CinemachineVirtualCamera>());
        }
    }
    void Update(){
        // if(Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow)){
        //     onCamSaved = true;
        //     swapCam(virtualCams[currCam], virtualCams[camSaved]);
        // }
        // else if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow)){
        //     if (onCamSaved){
        //         swapCam(virtualCams[camSaved], virtualCams[currCam]);
        //     }
        //     else{
        //         camSaved = currCam;
        //     }
        //     onCamSaved = false;
        // }
        if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow)){
            // onCamSaved = false;
            prevCam = currCam;
            if (currCam == 0) currCam = allCams.Count-1;
            else currCam--;
            swapCam(virtualCams[prevCam], virtualCams[currCam]);
        }
        else if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow)){
            // onCamSaved = false;
            prevCam = currCam;
            if (currCam == allCams.Count-1) currCam = 0;
            else currCam++;
            swapCam(virtualCams[prevCam], virtualCams[currCam]);
        }
    }


    void swapCam(CinemachineVirtualCamera p, CinemachineVirtualCamera n){
        // runeCanvas.worldCamera = allCams[currCam].GetComponent<Camera>();
        n.Priority = 11;
        p.Priority = 10;
    }
}
