using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;//for file reading
using UnityEngine.UI;

/// <summary>
/// This script is not part of Theo's original repo.
///
/// This reads data, and assembles it into a graph of Nodes (which really need a better name...)
/// It should be possible to replace this with other parsers from other sources,
/// and have the rest of the project not particularly care
///
/// </summary>

public class MTConnectParser : MonoBehaviour
{
  //our nice data structure is for interface and people things.
  //the only thing the parser needs to know is what numbers need updated, and what to put there.

  public bool useStaticSampleData;//if true, load from a file. Otherwise, look at the url.
  public List<AbstractValue> values;//all values that could possibly be updated. maybe trying to have this list at all is unworkable...
  public string filePath;

  public GameObject nodePrefab;

  public bool collapseDuplicateSamples;//do this whenever you have a lot of data-- for example, time-series data-- to prevent your computer from freezing

  public int totalNodes = 0;//for debugging
  public Text debugText;

  public GameObject rootNode;

  //variables for random guesses about how to read xml data on an android device below.
  public TextAsset XMLObject;
  StringReader xml;
  string extractedContent;


  void Awake(){//awake runs before the first frame, so that other things can use this data
    //we're setting this here because doing it in the inspector is annoying
    //filePath = "Assets/Resources/data_ur.xml";
    filePath = Application.streamingAssetsPath + "/data_ur.xml";
    if(useStaticSampleData){
      ReadStaticSampleData();
    }
  }

  private void ReadStaticSampleData(){
    XmlDocument xmlDoc = new XmlDocument();

    //commented out lines below this are a solution that works on desktop but not Quest
    //xmlDoc.Load(filePath);

    //the following works on both quest and desktop
    TextAsset textAsset = (TextAsset)Resources.Load("data_ur", typeof(TextAsset));
    xmlDoc.LoadXml ( textAsset.text );
    XmlNodeList topLevelNodes = xmlDoc.ChildNodes; //List of all devices
    XmlNode allContent = topLevelNodes[1];
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
  //these are all of our helper functions for making out code drier.
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
    //make a samples type for every sample name
    List<string> sampleNames = new List<string>();    
    foreach(XmlNode childNode in childNodes){
      if(!sampleNames.Contains(childNode.Name)){
        sampleNames.Add(childNode.Name);
        //Debug.Log("Added Sample Type " + childNode.Name);
      }
    }
    List<SampleType> sampleTypes = new List<SampleType>();
    List<AbstractNode> samplesButAbstract = new List<AbstractNode>();//this just prevents us from casting in 187
    List<GameObject> sampleTypeGOs = new List<GameObject>();
    foreach(string sampleTypeName in sampleNames){
      GameObject newSampleTypeGO = Instantiate(nodePrefab);
      sampleTypeGOs.Add(newSampleTypeGO);
      SampleType newSampleType = newSampleTypeGO.AddComponent<SampleType>();
      sampleTypes.Add(newSampleType);
      newSampleType.sampleTypeName = sampleTypeName;
      samplesButAbstract.Add(newSampleType);
      newSampleType.parentNode = holderNodeUnity;
      newSampleTypeGO.transform.parent = holderGO.transform;
    }
    holderNodeUnity.childNodes = samplesButAbstract;
    //aggreggate all samples of that name for each sample name
    Debug.Log("Sample node aggregator called");
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
