using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermometerRepresentation : FloatRepresentation
{
    //Spaghetti, but might be useful when all of the parts do actually have to talk to eachother
    public SampleTypeFloat underlyingNode;
    private float originalYOffset;//we aren't actually using this right now
    private Renderer rend;
    public GameObject meanLine;
    public Transform bottom, top;//defines the range of where the mean line can go

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
        SetMeanLine();
    }

    public void SetUnderlyingNode(SampleTypeFloat node){
        underlyingNode = node;
    }

    public void Recalculate(){
        MoveMercury(underlyingNode.lastSampleValue);
        SetMeanLine();

    }

    public void SetMeanLine(){
        meanLine.transform.position = Vector3.Lerp(bottom.position, top.position, GetNormalizedValue(underlyingNode.meanVal));
    }

    //this shifts the texture offset to move how much is purple
    private void MoveMercury(float newValue){
        //normalize the value
        float normalizedValue = GetNormalizedValue(newValue);//eventually make this not fake data
        Vector2 newOffset = new Vector2(0.0f, normalizedValue/2.0f);//this is a little hard-coded to our specific texture
        rend.material.mainTextureOffset = newOffset;
    }

    //takes the actual data value and returns a number between 0 and 1
    private float GetNormalizedValue(float value){
        float range = underlyingNode.maxVal-underlyingNode.minVal;
        //holy fuck the thing you need is linear interpolation-- nope. You need to /get/ t.
        float normalizedValue = (value-underlyingNode.minVal)/range;
        Debug.Log("pretty sure the normalized value is " + normalizedValue.ToString());
        return normalizedValue;

    }
}
