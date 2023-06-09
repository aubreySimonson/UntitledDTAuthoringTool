using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assembles the menu of components.
/// Should go on top-level Create Components gameobject,
/// which is a parent of the relevant canvas.
///
/// It and DevicesMenu should probably both inherit from
/// an abstract class called "Add Nodes Menu" or something,
/// but you're going to leave that problem until it's harder to fix.
///
/// This should go on the components menu, which is a child of the device prefab.
/// </summary>

public class ComponentsMenu : MonoBehaviour
{
    //we're making the assumption that all components are children of devices-- that might be wrong
    public Device parentNode;
    public GameObject parentDevice;
    public List<AbstractNode> allComponents;//only the ones which are children of parent device
    public GameObject componentPrefab;
    public GeneratorMenu generatorMenu;

    public GameObject camera;//main camera, for the menus to face towards when generated


    //positioning stuff
    public float currentY;//where we put the most recent menu option
    public float yInterval;//distance between menu options

    void Start()
    {
        StartCoroutine(WaitForParentInfo());
        if(generatorMenu == null){
            generatorMenu = gameObject.GetComponent<GeneratorMenu>();
        }
        if(camera == null){
          camera = GameObject.FindWithTag("MainCamera");
        }
    }

    public void AssembleComponents(){
      FindComponents(parentNode);
      Debug.Log("in assemble components");
      foreach(Component component in allComponents){
        GameObject newComponent = Instantiate(componentPrefab);//runs
        generatorMenu.menuItems.Add(newComponent);
        //tell the samples menu of this component what component to start checking the node tree from. A bit messy.
        newComponent.GetComponentInChildren<SampleTypesMenu>(true).parentNode = component;//true = include inactive
        //...and then you need to do some magic to make them stack correctly, and get the name right...
        newComponent.transform.parent = gameObject.transform;
        newComponent.transform.localPosition = new Vector3(-0.25f, currentY, 0.0f);
        //newComponent.transform.LookAt(Vector3.zero);
        newComponent.transform.rotation = newComponent.transform.parent.rotation;
        currentY-=yInterval;
        //change the label to the name-- there must be better ways of doing this...
        newComponent.transform.GetComponent<VarFinder>().label.SetText(component.componentName);
      }
    }

    public void FindComponents(AbstractNode thisNode){
      if(thisNode.nodeName == "ComponentStream"){
        allComponents.Add(thisNode);
      }
      else{
        foreach(AbstractNode childNode in thisNode.childNodes){//not relying on the scene hierarchy
          FindComponents(childNode);
        }
      }
    }

    //we use a co-routine to avoid looking for all of the child components of the device
    //before being told what the parent device is.
    //this might be totally unnecessary to do.
    IEnumerator WaitForParentInfo()
    {
        yield return new WaitUntil(() => parentNode != null);
        AssembleComponents();
    }
}
