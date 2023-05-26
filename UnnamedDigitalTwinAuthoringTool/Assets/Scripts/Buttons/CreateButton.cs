using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Part of Untitled Digital Twin Authoring Tool
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
