using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour
{
    [SerializeField] private GameObject nodeOrPlayer;

    public GameObject currObj{get{return nodeOrPlayer;} set{nodeOrPlayer = value;}}

    public void moveTo(Vector3 dest){
        this.transform.position = dest;
        this.transform.parent = nodeOrPlayer.transform;
    }

    private void Awake(){
        //snap the rune to its location
        moveTo(nodeOrPlayer.transform.position);
        ((Node)nodeOrPlayer.GetComponent(typeof(Node))).rune = this;
    }
}
