using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///Part of Untitled Digital Twin Authoring Tool
///This script goes on menus were grabbing an object and removing it from the menu makes it a persistent thing.
///</summary>

public class GeneratorMenu : MonoBehaviour
{
    public GameObject collectorObject;//stuff taken from the menu become children of this.
    public Collider menuCollider;
    public List<GameObject> menuItems;

    // Start is called before the first frame update
    void Start()
    {
      if(menuCollider==null){
        menuCollider = gameObject.GetComponent<Collider>();
      }
    }

    void OnTriggerExit(Collider other){
      if(menuItems.Contains(other.gameObject)){
        other.gameObject.transform.parent = collectorObject.transform;
      }
    }
}
