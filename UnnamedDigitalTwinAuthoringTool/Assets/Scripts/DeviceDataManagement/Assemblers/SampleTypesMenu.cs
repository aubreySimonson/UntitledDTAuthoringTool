using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assembles the menu of SampleTypes.
/// Should go on top-level Samples Menu gameobject,
/// which is a parent of the relevant canvas.
///
/// It, DevicesMenu, and ComponentsMenu should probably both inherit from
/// an abstract class called "Add Nodes Menu" or something,
/// but you're going to leave that problem until it's harder to fix.
///
/// </summary>

public class SampleTypesMenu : MonoBehaviour
{
    //we're making the assumption that all components are children of devices-- that might be wrong
    public AbstractNode parentNode;
    public GameObject parent;
    public List<AbstractNode> allSampleTypes;//only the ones which are children of parent component
    public GameObject sampleTypePrefab, floatPrefab;
    public GeneratorMenu generatorMenu;


    //positioning stuff
    public float currentY;//where we put the most recent menu option
    public float yInterval;//distance between menu options

    void Start()
    {
        StartCoroutine(WaitForParentInfo());
        if(generatorMenu == null){
            generatorMenu = gameObject.GetComponent<GeneratorMenu>();
        }
    }

    public void AssembleSamples(){
      FindSamples(parentNode);
      foreach(SampleType sampleType in allSampleTypes){
        GameObject newSampleType;
        if(sampleType is SampleTypeFloat){
          newSampleType = Instantiate(floatPrefab);
          newSampleType.GetComponent<FloatEditMenu>().associatedNode = (SampleTypeFloat)sampleType;//somewhat fragile
        }
        else{
          newSampleType = Instantiate(sampleTypePrefab);
        }
        generatorMenu.menuItems.Add(newSampleType);
        //...and then you need to do some magic to make them stack correctly, and get the name right...
        newSampleType.transform.parent = gameObject.transform;
        newSampleType.transform.localPosition = new Vector3(-0.25f, currentY, 0.0f);
        //newSampleType.transform.transform.LookAt(Vector3.zero);
        newSampleType.transform.rotation = newSampleType.transform.parent.rotation;
        currentY-=yInterval;
        //change the label to the name-- there must be better ways of doing this...
        newSampleType.transform.GetComponent<VarFinder>().label.SetText(sampleType.sampleTypeName);
      }
    }

    //how this works is going to be /very/ different from how it was for components...
    public void FindSamples(AbstractNode thisNode){
      //this assumes that no sample types will be childen of other sample types
      if(thisNode.gameObject.GetComponent<SampleType>() != null){
        allSampleTypes.Add(thisNode);
      }
      else{
        if(thisNode.childNodes[0]!=null){
          foreach(AbstractNode childNode in thisNode.childNodes){//not relying on the scene hierarchy
            FindSamples(childNode);
          }//end foreach
        }//end if
      }//end else
    }//end findsamples

    //we use a co-routine to avoid looking for all of the child components of the device
    //before being told what the parent device is.
    //this might be totally unnecessary to do.
    IEnumerator WaitForParentInfo()
    {
        yield return new WaitUntil(() => parentNode != null);
        AssembleSamples();
    }
}
