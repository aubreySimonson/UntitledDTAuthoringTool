using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This script is not part of Theo's original repo.
///
/// Assembles the menu of devices.
/// Should go on top-level Create Devices gameobject, 
/// which is a parent of the relevant canvas.
///
/// It and ComponentsMenu should probably both inherit from 
/// an abstract class called "Add Nodes Menu" or something, 
/// but you're going to leave that problem until it's harder to fix.
///
/// </summary>

public class DevicesMenu : MonoBehaviour
{
    public MTConnectParser parser;//just for getting the root node. To make this more general, we should probably have the parser give this to something else.
    public GameObject rootNode;
    public List<AbstractNode> allDevices;
    public GameObject devicePrefab;
    public Transform putDevicesHere;

    //positioning stuff
    private float currentY;//where we put the most recent menu option
    public float yInterval;//distance between menu options


    // Start is called before the first frame update
    void Start()
    {
      rootNode = parser.rootNode;
      currentY = 1.428f;
      AssembleDevices();
      if(parser == null){
        parser = GameObject.FindObjectOfType<MTConnectParser>();
      }
    }

    public void AssembleDevices(){
      FindDevices(rootNode.GetComponent<AbstractNode>());//use this if we can't be sure that devices will have a device component instead of a generic abstract node
      foreach(Device device in allDevices){
        GameObject newDevice = Instantiate(devicePrefab);//runs
        //...and then you need to do some magic to make them stack correctly, and get the name right...
        newDevice.transform.position = new Vector3(-0.224f, currentY, 1.458f);
        currentY-=yInterval;
        //change the label to the name-- there must be better ways of doing this...
        newDevice.transform.GetComponent<VarFinder>().label.SetText(device.deviceName);
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
