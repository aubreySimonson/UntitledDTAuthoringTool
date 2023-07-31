using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;//remove when you're done debugging

public class RepresentationMenuOptionFloat : MonoBehaviour
{
    public GameObject representationCollector;
    public GameObject representationPrefab;
    public SampleTypeFloat associatedNode;
    public bool testInstantiateRep = false;

    private void Update() {
        if(testInstantiateRep){
            InstantiateRepresentation();
            testInstantiateRep=false;
        } 
           
        //there's an off by 1 error somewhere. 
        //the following fix it, though the source is... a mystery
        if(representationCollector != gameObject.transform.GetComponentInParent<FloatEditMenu>().representationCollector){
            Debug.Log("Representation collector has somehow become something not given to this menu option by the menu. Fixing now.", this);//this happens
            representationCollector = gameObject.transform.GetComponentInParent<FloatEditMenu>().representationCollector;
        }
        if(associatedNode != gameObject.transform.GetComponentInParent<FloatEditMenu>().associatedNode){
            Debug.Log("Associated node has somehow become something not given to this menu option by the menu. Fixing now.", this);//this happens
            associatedNode = gameObject.transform.GetComponentInParent<FloatEditMenu>().associatedNode;
        }
    }

    public void InstantiateRepresentation(){
        try{
            GameObject newRepresentation = Instantiate(representationPrefab);
            newRepresentation.transform.parent = gameObject.transform;
            newRepresentation.transform.localPosition = new Vector3(0.25f, -0.04f, 0.0f);
            newRepresentation.transform.rotation = gameObject.transform.parent.rotation;

            //this whole project is really a first attempt at using polymorphism in a meaningful way,
            //and it is at best going medium-well
            newRepresentation.GetComponent<FloatRepresentation>().Initialize(associatedNode);

            //we do this last, because somehow this tends to go wrong?
            if(representationCollector!=null){
                newRepresentation.transform.parent = representationCollector.transform;
            }
        }
        catch{
            //it would be cool to put a little alarm that we turn on when something like this errors out
        }
    }

}
