using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevicesMenu : MonoBehaviour
{
    public MTConnectParser parser;//just for getting the root node. To make this more general, we should probably have the parser give this to something else.
    public GameObject rootNode;
    public List<AbstractNode> allDevices;
    public GameObject devicePrefab;

    // Start is called before the first frame update
    void Start()
    {
      rootNode = parser.rootNode;
      AssembleDevices();
    }

    public void AssembleDevices(){
      FindDevices(rootNode.GetComponent<AbstractNode>());
      foreach(AbstractNode device in allDevices){
        Instantiate(devicePrefab);
        //...and then you need to do some magic to make them stack correctly, and get the name right...
      }
    }

    public void FindDevices(AbstractNode thisNode){
      if(thisNode.nodeName == "DeviceStream"){
        allDevices.Add(thisNode);
      }
      else{
        foreach(AbstractNode childNode in thisNode.childNodes){//not relying on the scene hierarchy
          FindDevices(childNode);
        }
      }
    }
}
