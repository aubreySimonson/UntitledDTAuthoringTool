using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermometerRepresentation : FloatRepresentation
{
    //Spaghetti, but might be useful when all of the parts do actually have to talk to eachother
    public SampleTypeFloat underlyingNode;
    private float originalYOffset;
    private Renderer rend;

    //menus should call this after instantiating the relevant prefab. 
    //this is absolutely feral data architecture and should be refactored later
    public override void Initialize(SampleTypeFloat associatedNode){
        rend = gameObject.GetComponent<Renderer>();
        originalYOffset = rend.material.mainTextureOffset.y;//returns a Vector2, and we don't care about the x axis
        SetUnderlyingNode(associatedNode);
        SetDisplayValue(associatedNode.lastSampleValue);
        gameObject.transform.localScale = new Vector3(0.015f, 0.2f, 0.05f);
    }
    public void SetDisplayValue(string newValue){
        MoveMercury(float.Parse(newValue));
    }

    public void SetDisplayValue(float newValue){
        MoveMercury(newValue);
    }

    public void SetUnderlyingNode(SampleTypeFloat node){
        underlyingNode = node;
    }

    //this shifts the texture offset to move how much is purple
    private void MoveMercury(float newValue){
        //normalize the value
        float normalizedValue = 0.3f;//eventually make this not fake data
        Vector2 newOffset = new Vector2(0.0f, originalYOffset + normalizedValue);
        rend.material.mainTextureOffset = newOffset;

    }
}
