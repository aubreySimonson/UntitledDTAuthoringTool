using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleThis : MonoBehaviour
{
    public GameObject thingToToggle;

    public void Toggle(){
      thingToToggle.SetActive(!thingToToggle.activeSelf);
      
    }
}
