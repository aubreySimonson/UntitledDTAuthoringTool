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
      thisNodeUnity.gameObject.transform.parent = parentNode.gameObject.transform;
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
    GameObject thisNodeGo = Instantiate(nodePrefab);//instantiate an empty game object
    AbstractNode thisNodeUnity;
    if(node.Name == "DeviceStream"){
      thisNodeUnity = thisNodeGo.AddComponent<Device>();//inherits from abstract node
    }
    else{
      thisNodeUnity = thisNodeGo.AddComponent<AbstractNode>();//inherits from abstract node
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
