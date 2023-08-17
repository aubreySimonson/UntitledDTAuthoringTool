using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCollider : MonoBehaviour
{
    public bool isTouched;
    void OnTriggerEnter(Collider other){
      if(other.gameObject.tag == "hand"){
        isTouched = true;
      }
    }

    void OnTriggerExit(Collider other){
      if(other.gameObject.tag == "hand"){
        isTouched = false;
      }
    }
}
