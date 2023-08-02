using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScalingFloat : FloatRepresentation
{
    public TextMeshPro display;
    public Vector3 originalLocalScale;
    public float maxScale;//how much should we scale by if the thing is at its maximum value
    private Vector3 maxScaleVector;//this is just to make the lerp more convenient later
    public float minScale;//how much should we scale the thing by if the thing is at its min value?
    private Vector3 minScaleVector;//same for this

    //Spaghetti, but might be useful when all of the parts do actually have to talk to eachother
    public SampleTypeFloat underlyingNode;

    void Start(){
        originalLocalScale = gameObject.transform.localScale;
    }

    //menus should call this after instantiating the relevant prefab. 
    //this is absolutely feral data architecture and should be refactored later
    public override void Initialize(SampleTypeFloat associatedNode){
        display.text = "initialize was called";
        SetUnderlyingNode(associatedNode);
        display.text = "underlying node set";
        SetDisplayValue(associatedNode.lastSampleValue);
        PrecompileVectors();//we shouldn't have to do this again when things update-- just the once
        SetScale(associatedNode.lastSampleValue);
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

    public void SetScale(float newValue){
        //50/50 shot max and min are backwards
        display.transform.localScale = Vector3.Lerp(maxScaleVector, minScaleVector, GetNormalizedValue(newValue));
    }

    //this whole function exists because doing this in the code block where you're actually doing the lerping is super annoying
    private void PrecompileVectors(){
        minScaleVector = new Vector3(minScale, minScale, minScale);
        maxScaleVector = new Vector3(maxScale, maxScale, maxScale);
    }

    //takes the actual data value and returns a number between 0 and 1
    private float GetNormalizedValue(float value){
        float range = -1.0f;
        float normalizedValue = 0.1f;//if out value is exactly the mean, return this. Technically that means it shrinks with slight increases in distance from the mean, but frankly I don't really care.
        if(value > underlyingNode.meanVal){
            range = underlyingNode.maxVal - underlyingNode.meanVal;
            normalizedValue = (value-underlyingNode.meanVal)/range;
        }
        if(value<underlyingNode.meanVal){
            range = underlyingNode.meanVal - underlyingNode.minVal;
            normalizedValue = (underlyingNode.meanVal-value)/range;
        }
        Debug.Log("pretty sure the normalized value is " + normalizedValue.ToString());
        return normalizedValue;
    }
}
