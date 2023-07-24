using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class which any representation that uses a float to initialize itself. 
/// It's a bit irritating that this exists, but after several tries at getting things connected 
/// without creating it, this was the least of many possible evils
///
/// ??--> simonson.au@northeastern.edu
/// </summary>
public abstract class FloatRepresentation : AbstractRepresentation
{
    public abstract void Initialize(SampleTypeFloat associatedNode);
}
