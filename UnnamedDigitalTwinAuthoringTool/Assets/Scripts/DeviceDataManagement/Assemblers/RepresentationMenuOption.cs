using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepresentationMenuOption : MonoBehaviour
{
    public GameObject repPrefab;
    public SampleType associatedNode;
    public GameObject representationCollector;
    public void InstantiateRep(){
        GameObject newRepresentation = Instantiate(repPrefab);
        newRepresentation.transform.parent = gameObject.transform;
        newRepresentation.transform.localPosition = new Vector3(0.25f, -0.04f, 0.0f);
        newRepresentation.transform.rotation = gameObject.transform.parent.rotation;
        if(representationCollector!=null){
            newRepresentation.transform.parent = representationCollector.transform;
        }
        newRepresentation.transform.localScale = new Vector3(0.022f, 0.022f, 0.022f);
        newRepresentation.GetComponent<AbstractRepresentation>().Initialize(associatedNode);
    }
}
