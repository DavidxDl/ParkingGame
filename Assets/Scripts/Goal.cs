using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private Transform goalVisual;
    

   


    public void SetGreenColor(){
        goalVisual.GetComponent<Renderer>().material.color = Color.green;
    }
    public void SetDefaultColor(){
        goalVisual.GetComponent<Renderer>().material.color = Color.yellow;
    }
}
