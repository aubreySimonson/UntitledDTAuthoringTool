using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SimpleFloatRepresentation : AbstractRepresentation
{

    public TextMeshPro label;

    //Spaghetti, but might be useful when all of the parts do actually have to talk to eachother
    public SampleTypeFloat underlyingNode;

    //menus should call this after instantiating the relevant prefab. 
    //this is absolutely feral data architecture and should be refactored later

    public void Initialize(SampleTypeFloat node){
        SetUnderlyingNode(node);
        SetLabelValue(node.lastSampleValue);
        //there may be a point in time where we want to do other things here?
    }
    public void SetLabelValue(string newValue){
        label.text = newValue;
    }

    public void SetLabelValue(float newValue){
        label.text = newValue.ToString();
    }

    public void SetUnderlyingNode(SampleTypeFloat node){
        underlyingNode = node;
    }
}
