using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class which all representations should inherit from.
/// Mostly for being able to put them all in a list of the same type--
/// They should be able to have basically nothing in common.
///
/// ??--> simonson.au@northeastern.edu
/// </summary>

public class AbstractRepresentation : MonoBehaviour
{
    //representations should generally be able to set themselves up from just this information
    public void Initialize(SampleType associatedNode){

    }
}
