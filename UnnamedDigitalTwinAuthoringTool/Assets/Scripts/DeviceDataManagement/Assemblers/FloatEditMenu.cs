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
    public SampleTypeFloat associatedNode;

    //each of the prefabs should have an abstract representation option
    private List<AbstractRepresentation> representationOptions;
    public List<GameObject> representationPrefabs;
    public GameObject menuOptionPrefab;
    public GameObject repMenu;
    public GameObject representationCollector;
    public float currentY = 0.5f;//where we put the last menu option
    public float yInterval;//amount to move every menu option down by
    // Start is called before the first frame update
    void Start()
    {        
        StartCoroutine(WaitForNodeInfo());

        representationOptions = new List<AbstractRepresentation>();
        //check that all representation prefabs have an abstract representation-- otherwise throw them away
        foreach(GameObject repPrefab in representationPrefabs){
            if(repPrefab.GetComponent<AbstractRepresentation>() == null){
                representationPrefabs.Remove(repPrefab);
            }
            else{
                representationOptions.Add(repPrefab.GetComponent<AbstractRepresentation>());
            }
        }
    }

    IEnumerator WaitForNodeInfo()
    {
        yield return new WaitUntil(() => associatedNode != null);
        UpdateMenu();
        CreateRepresentationsMenu();
    }

    public void CreateRepresentationsMenu(){
        foreach(GameObject rep in representationPrefabs){
            //create the menu option
            GameObject menuOption = Instantiate(menuOptionPrefab);
            menuOptionPrefab.GetComponent<RepresentationMenuOptionFloat>().representationCollector = representationCollector;
            menuOptionPrefab.GetComponent<RepresentationMenuOptionFloat>().associatedNode = associatedNode;
            //put it where it goes
            menuOption.transform.parent = repMenu.transform;
            menuOption.transform.localPosition = new Vector3(0.03f, currentY, 0.0f);
            menuOption.transform.localScale = new Vector3(3.0f, 2.5f, 0.1f);
            menuOption.transform.rotation = repMenu.transform.rotation;
            currentY-=yInterval;

            //change the label to the name
            AbstractRepresentation representation = rep.GetComponent<AbstractRepresentation>();
            menuOption.transform.GetComponent<VarFinder>().label.SetText(representation.name);
            //make the button on the menu option be to instantiate a copy of this thing
            menuOptionPrefab.GetComponent<RepresentationMenuOptionFloat>().representationPrefab = rep;
        }

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
        if(IsDefaultText(totalLabel.text) || float.Parse(totalLabel.text)!=associatedNode.numberOfSamples){
            //update label
            totalLabel.SetText(associatedNode.numberOfSamples.ToString());
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
