using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Part of MiRIAD, an authoring tool for digital twins
///
///This script contains a simple helper function for the 'see more' button of the menu
///???-->followspotfour@gmail.com
///</summary>
public class ToggleThis : MonoBehaviour
{
    public GameObject thingToToggle;

    public void Toggle(){
      thingToToggle.SetActive(!thingToToggle.activeSelf);
      
    }
}
