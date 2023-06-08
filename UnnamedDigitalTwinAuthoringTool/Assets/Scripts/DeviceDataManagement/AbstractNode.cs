using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// This script is not part of Theo's original repo.
///
/// The new data management structure works from the idea of MTConnect data as a hierarchy of nodes.
/// We don't know our data structure very well in advance.
/// We do know that machines tend to have parts, and those parts might have their own parts,
/// and at some levels, there will be some data we'll want to represent.
/// This is the abstract class that all other Nodes should inherrit from.
///
/// Gets the status of each machine from mtConnect, and updates materials based on availability
/// Also handles some navigation-related things.
///
/// This should probably be moved out of comments and into a real document at some point.
///
/// Next up--> figure out what the MTConnect url was
/// --> what the heck does the data the MTConnect script is getting look like
///
/// </summary>

public class AbstractNode : MonoBehaviour
{
  public List<AbstractNode> childNodes;
  public AbstractNode parentNode;//the choice to make this a single node or a list... is really important
  public string nodeName;
  public string nodeID;
  public AbstractValue value;
  //do you also add a dictionary of other values?
  public GameObject physicalRepresentation;
}
