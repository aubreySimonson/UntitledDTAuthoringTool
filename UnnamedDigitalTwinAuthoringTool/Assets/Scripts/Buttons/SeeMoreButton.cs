using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

///<summary>
///Part of MiRIAD, an authoring tool for digital twins
///
///This script goes on the 'see more' button of menus.
///???-->followspotfour@gmail.com
///</summary>
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
