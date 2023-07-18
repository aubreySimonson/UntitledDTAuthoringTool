using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepresentationMenuOption : MonoBehaviour
{
    public GameObject repPrefab;
    public SampleType associatedNode;
    public void InstantiateRep(){
        GameObject newRepresentation = Instantiate(repPrefab);
        newRepresentation.GetComponent<AbstractRepresentation>().Initialize(associatedNode);
    }
}
