using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Goes on the edit menu of SampleType Menus for specifically floats.
/// This makes the labels say the right information
///
/// </summary>
public class FloatEditMenu : MonoBehaviour
{
    public TextMeshPro maxLabel, minLabel, meanLabel, lastLabel, timeStampLabel, totalLabel;
    public SampleTypeFloat associatedNode;//figuring out how to give this the node it needs will be... rough
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForNodeInfo());
    }

    IEnumerator WaitForNodeInfo()
    {
        yield return new WaitUntil(() => associatedNode != null);
        UpdateMenu();
    }

    //this should get called any time the information might have changed. 
    //if would be pretty expensive to do this in an update function,
    //so try to be smart about it
    public void UpdateMenu(){
        if(IsDefaultText(maxLabel.text) || float.Parse(maxLabel.text)!=associatedNode.maxVal){
            //update label
            maxLabel.SetText(associatedNode.maxVal.ToString());
        }
        if(IsDefaultText(minLabel.text) || float.Parse(minLabel.text)!=associatedNode.minVal){
            //update label
            minLabel.SetText(associatedNode.minVal.ToString());
        }
        if(IsDefaultText(meanLabel.text) || float.Parse(meanLabel.text)!=associatedNode.meanVal){
            //update label
            meanLabel.SetText(associatedNode.meanVal.ToString());
        }
        if(IsDefaultText(lastLabel.text) || float.Parse(lastLabel.text)!=associatedNode.lastSampleValue){
            //update label
            lastLabel.SetText(associatedNode.lastSampleValue.ToString());
        }
        if(timeStampLabel.text!=associatedNode.lastTimeStamp.ToString()){
            //update label
            timeStampLabel.SetText(associatedNode.lastTimeStamp.ToString());
        }
        if(IsDefaultText(totalLabel.text) || float.Parse(totalLabel.text)!=associatedNode.total){
            //update label
            totalLabel.SetText(associatedNode.total.ToString());
        }
    }

    //for checking if the text is our default "there's no information here" string or a float
    private bool IsDefaultText(string text){
        try
        {
            float.Parse(text);
            return false;
        }
        catch
        {
            return true;
        }
    }
}
