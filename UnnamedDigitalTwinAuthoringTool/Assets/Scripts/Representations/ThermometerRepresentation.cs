using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermometerRepresentation : FloatRepresentation
{
    //Spaghetti, but might be useful when all of the parts do actually have to talk to eachother
    public SampleTypeFloat underlyingNode;

    //menus should call this after instantiating the relevant prefab. 
    //this is absolutely feral data architecture and should be refactored later
    public override void Initialize(SampleTypeFloat associatedNode){
        SetUnderlyingNode(associatedNode);
        SetDisplayValue(associatedNode.lastSampleValue);
        gameObject.transform.localScale = new Vector3(0.015f, 0.2f, 0.05f);
    }
    public void SetDisplayValue(string newValue){

    }

    public void SetDisplayValue(float newValue){

    }

    public void SetUnderlyingNode(SampleTypeFloat node){
        underlyingNode = node;
    }
}
