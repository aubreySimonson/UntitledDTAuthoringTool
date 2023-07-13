using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Goes on the edit menu of SampleType Menus for specifically floats.
/// This makes the labels say the right information
///
/// </summary>
public class FloatEditMenu : MonoBehaviour
{
    public Text maxLabel, minLabel, meanLabel, lastLabel, timeStampLabel, totalLabel;
    public SampleTypeFloat associatedNode;//figuring out how to give this the node it needs will be... rough
    // Start is called before the first frame update
    void Start()
    {
        UpdateMenu();
    }

    //this should get called any time the information might have changed. 
    //if would be pretty expensive to do this in an update function,
    //so try to be smart about it
    public void UpdateMenu(){
        if(float.Parse(maxLabel.text)!=associatedNode.maxVal){
            //update label
            maxLabel.text = associatedNode.maxVal.ToString();
        }
        if(float.Parse(minLabel.text)!=associatedNode.minVal){
            //update label
            minLabel.text = associatedNode.minVal.ToString();
        }
        if(float.Parse(meanLabel.text)!=associatedNode.meanVal){
            //update label
            meanLabel.text = associatedNode.meanVal.ToString();
        }
        if(float.Parse(lastLabel.text)!=associatedNode.lastSampleValue){
            //update label
            lastLabel.text = associatedNode.lastSampleValue.ToString();
        }
        if(timeStampLabel.text!=associatedNode.lastTimeStamp.ToString()){
            //update label
            timeStampLabel.text = associatedNode.lastSample.ToString();
        }
    }
}
