using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwap : MonoBehaviour
{
    GameObject camSaved;
    [SerializeField] List<GameObject> allCams;
    [SerializeField] GameManager gameManager;

    int currCam = 0;
    bool onCamSaved = false;
    private Rune[] runes;
    public GameObject currCamera{get{return allCams[currCam];}}

    void Awake(){
        runes = FindObjectsOfType<Rune>();
        camSaved = allCams[0];
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow)){
            onCamSaved = true;

            foreach(GameObject cam in allCams){
                cam.SetActive(false);
            }

            camSaved.SetActive(true);
            
        }
        else if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow)){
            if (onCamSaved){
                foreach(GameObject cam in allCams){
                    cam.SetActive(false);
                }
                allCams[currCam].SetActive(true);
            }
            else{
                saveCam();
            }
            onCamSaved = false;
        }
        else if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow)){
            onCamSaved = false;
            if (currCam == 0) currCam = allCams.Count-1;
            else currCam--;
            swapCam();
        }
        else if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow)){
            onCamSaved = false;
            if (currCam == allCams.Count-1) currCam = 0;
            else currCam++;
            swapCam();
        }
    }


    void swapCam(){
        // camSaved.SetActive(false);
        foreach(GameObject cam in allCams){
            cam.SetActive(false);
        }

        allCams[currCam].SetActive(true);
    }

    void saveCam(){
        camSaved = allCams[currCam];
    }
}
