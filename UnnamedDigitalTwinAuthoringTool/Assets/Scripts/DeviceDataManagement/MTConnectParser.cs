using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;//for file reading

public class MTConnectParser : MonoBehaviour
{
  //our nice data structure is for interface and people things.
  //the only thing the parser needs to know is what numbers need updated, and what to put there.

  public bool useStaticSampleData;//if true, load from a file. Otherwise, look at the url.
  public List<AbstractValue> values;
  public string filePath;

  public GameObject nodePrefab;

  //vars belod this are for debugging-- don't keep them
  public XmlNodeList topLevelNodes;

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
    topLevelNodes = xmlDoc.ChildNodes; //List of all devices
    XmlNode header = topLevelNodes[0];
    Debug.Log("This should be the header: " + header);
    XmlNode allContent = topLevelNodes[1];
    Debug.Log("This should be everything else: " + allContent);
    CreateNodeGameObject(topLevelNodes[1]);
    Debug.Log("that worked?");
  }

  private AbstractNode CreateNodeGameObject(XmlNode node){
    Debug.Log("CreateNodeGameObject called again");
    GameObject thisNodeGo = Instantiate(nodePrefab);//instantiate an empty game object
    AbstractNode thisNodeUnity = thisNodeGo.AddComponent<AbstractNode>();
    XmlNodeList childNodes = node.ChildNodes;
    thisNodeUnity.childNodes = new List<AbstractNode>();
    foreach(XmlNode childNode in childNodes){
      thisNodeUnity.childNodes.Add(CreateNodeGameObject(childNode, thisNodeUnity));
    }
    return thisNodeUnity;
  }
  private AbstractNode CreateNodeGameObject(XmlNode node, AbstractNode parentNode){//this is a big vague and misleading-- make having the parent node optional.
    Debug.Log("CreateNodeGameObject called again");
    GameObject thisNodeGo = Instantiate(nodePrefab);//instantiate an empty game object
    AbstractNode thisNodeUnity = thisNodeGo.AddComponent<AbstractNode>();
    thisNodeUnity.parentNode = parentNode;
    XmlNodeList childNodes = node.ChildNodes;
    thisNodeUnity.childNodes = new List<AbstractNode>();
    foreach(XmlNode childNode in childNodes){
      thisNodeUnity.childNodes.Add(CreateNodeGameObject(childNode, thisNodeUnity));
    }
    return thisNodeUnity;
  }
}
