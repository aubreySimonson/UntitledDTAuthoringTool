using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;//for file reading
using UnityEngine.UI;

///<summary>
///Part of MiRIAD, an authoring tool for digital twins
/// This reads data, and assembles it into a graph of Nodes (which really need a better name...)
/// It should be possible to replace this with other parsers from other sources,
/// and have the rest of the project not particularly care
///???-->followspotfour@gmail.com
///</summary>

public class MTConnectParser : MonoBehaviour
{
  public bool useStaticSampleData;//if true, load from a file. Otherwise, look at the url.
  public string fileName;//this should be just the name of the file, with no type extension. Put the file in the Resources folder.

  public GameObject nodePrefab;

  public bool collapseDuplicateSamples;//do this whenever you have a lot of data-- for example, time-series data-- to prevent your computer from freezing

  public int totalNodes = 0;//we don't use this information for anything, but it's cool to know
  public Text debugText;//leaving this null won't throw errors or break anything

  public GameObject rootNode;

  //variables for random guesses about how to read xml data on an android device below.
  private TextAsset XMLObject;
  StringReader xml;
  string extractedContent;


  void Awake(){//awake runs before the first frame, so that other things can use this data
    //we're setting this here because doing it in the inspector is annoying
    if(useStaticSampleData){
      ReadStaticSampleData();
    }
  }

  private void ReadStaticSampleData(){
    XmlDocument xmlDoc = new XmlDocument();

    //the following works on both quest and desktop
    TextAsset textAsset = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
    xmlDoc.LoadXml ( textAsset.text );
    XmlNodeList topLevelNodes = xmlDoc.ChildNodes; //List of all devices
    XmlNode allContent = topLevelNodes[1];//we can know that topLevelNodes will be a header and content because of the standard
    CreateNodeGameObject(allContent, true);
    Debug.Log("Total nodes: " + totalNodes);
  }

  private AbstractNode CreateNodeGameObject(XmlNode node, bool doRecursion){//there should probably be separate recursive and non-recursive versions
    if(node == null){
      return null;
    }
    if(node.Name!="#text"){//trims white space
      totalNodes++;//for debugging, cut later
      AbstractNode thisNodeUnity = CreateNodeHelperFunction(node);
      if(doRecursion && node.Name!="Samples"){//don't do recursion on sets of samples
        NodeRecursion(node, thisNodeUnity);
      }
      return thisNodeUnity;
    }
    return null;
  }

  private AbstractNode CreateNodeGameObject(XmlNode node, AbstractNode parentNode, bool doRecursion){//this is a big vague and misleading
    if(parentNode==null || node == null){
      return null;
    }
    if(node.Name!="#text"){//trims white space
      totalNodes++;//for debugging, cut later
      AbstractNode thisNodeUnity = CreateNodeHelperFunction(node);
      if(thisNodeUnity == null){//the xml node we found should not get a unity node
        return null;
      }
      thisNodeUnity.parentNode = parentNode;
      thisNodeUnity.gameObject.transform.parent = parentNode.gameObject.transform;//stacks them in the hierarchy-- optional
      if(doRecursion && node.Name!="Samples"){//don't do recursion on sets of samples
        NodeRecursion(node, thisNodeUnity);
      }
      return thisNodeUnity;
    }
    return null;
  }

  //there are a bunch of things we do across the different overloads of createnodegameobject in exactly the same way
  //these are all of our helper functions for making out code dryer.
  private AbstractNode CreateNodeHelperFunction(XmlNode node){
    GameObject thisNodeGo = Instantiate(nodePrefab);//instantiate an empty game object
    AbstractNode thisNodeUnity = AddCorrectNodeType(node, thisNodeGo);
    if(thisNodeUnity==null){//add correct node type doesn't always return a node
      Destroy(thisNodeGo);//don't leave an abandoned game object-- obviously some refactoring could make all of this more efficient
      return null;
    }
    if(rootNode==null){
      rootNode=thisNodeGo;
    }
    thisNodeUnity.nodeName = node.Name;
    if(node.Attributes["dataItemId"]!=null){
      thisNodeUnity.nodeID = node.Attributes["dataItemId"].Value;
    }
    return thisNodeUnity;
  }

  private void NodeRecursion(XmlNode node, AbstractNode thisNodeUnity){
    XmlNodeList childNodes = node.ChildNodes;
    thisNodeUnity.childNodes = new List<AbstractNode>();
    if(collapseDuplicateSamples && node.Name == "Samples"){//don't do recursion on children of samples
      List<string> nodeNames = new List<string>();
      foreach(XmlNode childNode in childNodes){
        if(!nodeNames.Contains(childNode.Name)){
          nodeNames.Add(childNode.Name);
          thisNodeUnity.childNodes.Add(CreateNodeGameObject(childNode, thisNodeUnity, false));
        }
      }//end for
    }//end if
    else{
        foreach(XmlNode childNode in childNodes){
          thisNodeUnity.childNodes.Add(CreateNodeGameObject(childNode, thisNodeUnity, true));
        }//end for
    }//end else
  }

  //a node could need an abstract node script, 
  //or a more specific scripts which inherits from it and has special information.
  //we hide checking for every possible node type we care about down here
  private AbstractNode AddCorrectNodeType(XmlNode node, GameObject thisNodeGo){
    AbstractNode thisNodeUnity;
    if(node.Name == "DeviceStream"){
      thisNodeUnity = thisNodeGo.AddComponent<Device>();//inherits from abstract node
      thisNodeUnity.GetComponent<Device>().deviceName = node.Attributes["name"].Value;//we assume that all devices will have a name in this format. do they?
    }
    else if(node.Name == "ComponentStream"){
      thisNodeUnity = thisNodeGo.AddComponent<Component>();//inherits from abstact node
      thisNodeUnity.GetComponent<Component>().componentName = node.Attributes["component"].Value;
    }
    else if (node.Name == "Samples"){
      thisNodeUnity = thisNodeGo.AddComponent<SamplesHolder>();//inherits from abstact node
      SamplesAggregator(node, thisNodeUnity, thisNodeGo);
    }
    else{
      thisNodeUnity = thisNodeGo.AddComponent<AbstractNode>();
    }
    return thisNodeUnity;
  }

  //how to make sure that these all get the right parent nodes is actually quite the issue
  private void SamplesAggregator(XmlNode holderNode, AbstractNode holderNodeUnity, GameObject holderGO){
    //get all children of the node
    XmlNodeList childNodes = holderNode.ChildNodes;

    //make a samples type for every sample name-- makes it easier to assign samples to their sampleTypes
    List<string> sampleNames = new List<string>();   
    //make a list of sampletypes, gameobjects they are attached to
    List<SampleType> sampleTypes = new List<SampleType>();
    List<AbstractNode> samplesButAbstract = new List<AbstractNode>();//this just prevents us from casting in 187
    List<GameObject> sampleTypeGOs = new List<GameObject>();

    foreach(XmlNode childNode in childNodes){
      SampleType thisSampleType;
      if(!sampleNames.Contains(childNode.Name)){//if we haven't seen this name before
        sampleNames.Add(childNode.Name);
        GameObject thisSampleTypeGO = Instantiate(nodePrefab);
        sampleTypeGOs.Add(thisSampleTypeGO);

        //add sample node of correct type
        thisSampleType = AddSampleTypeOfCorrectType(childNode, thisSampleTypeGO); 
        sampleTypes.Add(thisSampleType); 
        samplesButAbstract.Add(thisSampleType);
        //set its name
        thisSampleType.sampleTypeName = childNode.Name;
        thisSampleType.parentNode = holderNodeUnity;
        thisSampleTypeGO.transform.parent = holderGO.transform;
      }
      else{//if we've seen this sampletype before, find it
        thisSampleType = FindSampleTypeByName(sampleTypes, childNode.Name);
      }
      //then, now that we know that the sample type exists, do things with the data from this sample:
      //increase the number of samples we've found
      thisSampleType.numberOfSamples++;

      //time stamp things
      bool updateVal = false;//flag for a few lines later
      try{
        System.DateTime timeStamp = System.DateTime.Parse(childNode.Attributes["timestamp"].Value);
        if(thisSampleType.lastTimeStamp == null || timeStamp>thisSampleType.lastTimeStamp){
          //Debug.Log("timestamp: " + timeStamp);
          thisSampleType.lastTimeStamp = timeStamp;
          if(thisSampleType is SampleTypeFloat){
            updateVal = true;
          }//end if
        }//end if
      }//end try
      catch{
        Debug.Log("no correctly formatted timestamp for this sample");
      }


      //special things we only do for floats
      if(thisSampleType is SampleTypeFloat){
        float f = float.Parse(childNode.InnerText);
        SampleTypeFloat thisSampleTypeFloat = (SampleTypeFloat)thisSampleType;
        thisSampleTypeFloat.total += f;
        if(updateVal){
          thisSampleTypeFloat.lastSampleValue = f;
        }
        if(f>thisSampleTypeFloat.maxVal){
          thisSampleTypeFloat.maxVal = f;
        }
        if(f<thisSampleTypeFloat.minVal){
          thisSampleTypeFloat.minVal = f;
        }
        thisSampleTypeFloat.meanVal = thisSampleTypeFloat.total/(float)thisSampleTypeFloat.numberOfSamples;
        //Debug.Log("The new mean of " + thisSampleTypeFloat.sampleTypeName + " is " + thisSampleTypeFloat.meanVal);
      }//end special float things
    }//end foreach
    holderNodeUnity.childNodes = samplesButAbstract;
  }//end samples aggregator

  //this is an inefficient, really brute force method and someone else should fix it later.
  //maybe you in the future when you're better at this
  private SampleType FindSampleTypeByName(List<SampleType> list, string name){
    foreach(SampleType sampleType in list){
      if(sampleType.sampleTypeName == name){
        return sampleType;
      }
    }
    return null;
  }

  //this is structured a bit stupidly because addcomponent is very picky--
  //there doesn't seem to be a way to add a component to a gameobject /after/ it's created
  private SampleType AddSampleTypeOfCorrectType(XmlNode node, GameObject go){
    //note that InnerText will return /all/ of the inner text of our node, so if out node isn't a leaf, this will get weird
    //check if it's a float
    float value;
    if(float.TryParse(node.InnerText, out value)==true){
      SampleTypeFloat newFloat = go.AddComponent<SampleTypeFloat>();
      return newFloat;
    }
    //if you were to treat other values as special, you would check for them here
    //if it isn't anything specific that we care about, return an abstract sample type
    SampleType newSampleType = go.AddComponent<SampleType>();
    return newSampleType;
  }

  //consider cutting this. We don't use it right now.
  private bool AreSameNamedSiblings(string name, AbstractNode parentNode){
    foreach(AbstractNode sibling in parentNode.childNodes){
      if(sibling.nodeName == name){
        return true;
      }
    }//end for
    return false;
  }//end check for same named siblings
}
