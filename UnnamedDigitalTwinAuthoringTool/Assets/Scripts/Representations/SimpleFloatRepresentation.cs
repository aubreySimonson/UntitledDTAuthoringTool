using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SimpleFloatRepresentation : AbstractRepresentation
{

    public TextMeshPro display;

    //Spaghetti, but might be useful when all of the parts do actually have to talk to eachother
    public SampleTypeFloat underlyingNode;

    //menus should call this after instantiating the relevant prefab. 
    //this is absolutely feral data architecture and should be refactored later

    public void Initialize(SampleTypeFloat associatedNode){
        display.text = "initialize was called";
        SetUnderlyingNode(associatedNode);
        display.text = "underlying node set";
        SetDisplayValue(associatedNode.lastSampleValue);
        gameObject.transform.localScale = new Vector3(0.022f, 0.022f, 0.022f);
    }
    public void SetDisplayValue(string newValue){
        display.text = newValue;
    }

    public void SetDisplayValue(float newValue){
        display.text = newValue.ToString();
    }

    public void SetUnderlyingNode(SampleTypeFloat node){
        underlyingNode = node;
    }
}
