using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Part of MiRIAD, an authoring tool for digital twins
///
///This script goes on the 'create' button of menus.
///???-->followspotfour@gmail.com
///</summary>
public class CreateButton : MonoBehaviour
{
    public GameObject createThis;
    public Transform parentObject;

    public void Create(){
      GameObject newThing = Instantiate(createThis);
      newThing.transform.SetParent(parentObject, true);
      newThing.transform.position = parentObject.position;
      newThing.transform.rotation = parentObject.rotation;
    }
}
