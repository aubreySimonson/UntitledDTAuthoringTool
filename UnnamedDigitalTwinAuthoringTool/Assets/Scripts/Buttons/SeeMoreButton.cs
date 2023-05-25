using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SeeMoreButton : MonoBehaviour
{
  public GameObject thingToToggle;
  public TextMeshPro label;

  public void SeeMoreOrLess(){
    if(thingToToggle.activeSelf){//if more information is open, close it
      thingToToggle.SetActive(false);
      label.SetText("See More");
    }
    else{//otherwise, open it
      thingToToggle.SetActive(true);
      label.SetText("See Less");
    }
  }
}
