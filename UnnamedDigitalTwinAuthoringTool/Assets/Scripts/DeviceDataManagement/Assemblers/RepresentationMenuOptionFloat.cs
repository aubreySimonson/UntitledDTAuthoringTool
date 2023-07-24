using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;//remove when you're done debugging

public class RepresentationMenuOptionFloat : MonoBehaviour
{
    //clearly expose poke button
    public GameObject representationCollector;
    public GameObject representationPrefab;
    public SampleTypeFloat associatedNode;
    public bool testInstantiateRep = false;

    private void Update() {
        if(testInstantiateRep){
            InstantiateRepresentation();
            testInstantiateRep=false;
        }    
    }

    public void InstantiateRepresentation(){
        GameObject newRepresentation = Instantiate(representationPrefab);
        newRepresentation.transform.parent = gameObject.transform;
        newRepresentation.transform.localPosition = new Vector3(0.25f, -0.04f, 0.0f);
        newRepresentation.transform.rotation = gameObject.transform.parent.rotation;
        if(representationCollector!=null){
            newRepresentation.transform.parent = representationCollector.transform;
        }

        //this whole project is really a first attempt at using polymorphism in a meaningful way,
        //and it is at best going medium-well
        newRepresentation.GetComponent<FloatRepresentation>().Initialize(associatedNode);
    }

}