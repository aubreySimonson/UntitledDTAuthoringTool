using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;//for file reading

/// <summary>
/// This script is not part of Theo's original repo.
///
/// This reads data, and assembles it into a graph of Nodes (which really need a better name...)
/// It should be possible to replace this with other parsers from other sources,
/// and have the rest of the project not particularly care
///
/// Assumtions to check: can we collaps all nodes with the same name which are children on one node?
/// is Samples a reasonable name to expect?
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

  void Start(){
    //we're setting this here because doing it in the inspector is annoying
    filePath = "Assets/Resources/DATA-DO-NOT-COMMIT-TO-GIT/data_ur.xml";
    if(useStaticSampleData){
      ReadStaticSampleData();
    }
  }

  private void ReadStaticSampleData(){
    XmlDocument xmlDoc = new XmlDocument();
    xmlDoc.Load(filePath);
    XmlNodeList topLevelNodes = xmlDoc.ChildNodes; //List of all devices
    XmlNode header = topLevelNodes[0];
    Debug.Log("This should be the header: " + header);
    XmlNode allContent = topLevelNodes[1];
    Debug.Log("This should be everything else: " + allContent);
    Debug.Log("attributes? " + allContent.Name);//<-----THATS HOW YOU GET THE NAME
    CreateNodeGameObject(allContent, true);
    Debug.Log("Total nodes: " + totalNodes);
    Debug.Log("that worked?");
  }

  private AbstractNode CreateNodeGameObject(XmlNode node, bool doRecursion){//there should probably be separate recursive and non-recursive versions
    if(node.Name!="#text"){//trims white space
      totalNodes++;//for debugging, cut later
      AbstractNode thisNodeUnity = CreateNodeHelperFunction(node);
      if(doRecursion){
        NodeRecursion(node, thisNodeUnity);
      }
      return thisNodeUnity;
    }
    return null;
  }

  private AbstractNode CreateNodeGameObject(XmlNode node, AbstractNode parentNode, bool doRecursion){//this is a big vague and misleading
    if(node.Name!="#text"){//trims white space
      totalNodes++;//for debugging, cut later
      AbstractNode thisNodeUnity = CreateNodeHelperFunction(node);
      thisNodeUnity.parentNode = parentNode;
      if(doRecursion){
        NodeRecursion(node, thisNodeUnity);
      }
      return thisNodeUnity;
    }
    return null;
  }

  //there are a bunch of things we do across the different overloads of createnodegameobject in exactly the same way
  //these are all of our helper functions for making out code drier.
  private AbstractNode CreateNodeHelperFunction(XmlNode node){
    //Debug.Log("CreateNodeGameObject called again");//having this turned on will slow things down-- it's the 8k library books problem
    //Debug.Log("Node created: " + node.Name);
    GameObject thisNodeGo = Instantiate(nodePrefab);//instantiate an empty game object
    AbstractNode thisNodeUnity = thisNodeGo.AddComponent<AbstractNode>();
    thisNodeUnity.nodeName = node.Name;
    return thisNodeUnity;
  }

  private void NodeRecursion(XmlNode node, AbstractNode thisNodeUnity){
    XmlNodeList childNodes = node.ChildNodes;
    thisNodeUnity.childNodes = new List<AbstractNode>();
    if(collapseDuplicateSamples && node.Name == "Samples"){
      List<string> nodeNames = new List<string>();
      foreach(XmlNode childNode in childNodes){
        if(!nodeNames.Contains(childNode.Name)){
          nodeNames.Add(childNode.Name);
          thisNodeUnity.childNodes.Add(CreateNodeGameObject(childNode, thisNodeUnity));
        }
      }//end for
    }//end if
    else{
        foreach(XmlNode childNode in childNodes){
          thisNodeUnity.childNodes.Add(CreateNodeGameObject(childNode, thisNodeUnity));
        }//end for
    }//end else
  }

  private bool AreSameNamedSiblings(string name, AbstractNode parentNode){
    foreach(AbstractNode sibling in parentNode.childNodes){
      if(sibling.nodeName == name){
        return true;
      }
    }//end for
    return false;
  }//end check for same named siblings
}
