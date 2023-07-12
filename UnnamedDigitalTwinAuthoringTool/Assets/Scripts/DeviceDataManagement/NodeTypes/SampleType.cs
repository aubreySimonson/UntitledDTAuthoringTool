using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Node Type for Sample Types, which are children of components.
/// SampleType nodes should be parents of individual samples.
/// Which samples to keep and which to cull, and the interface for setting those parameters
/// are still major open architecture questions.
///
/// </summary>

public class SampleType : AbstractNode
{
    public string sampleTypeName;
    public string dataItemId;

    //saving refs to time stamps and values here, rather than asking lastSample  for them
    //is only a choice you're making for efficiency reasons.
    //If it turns out to not be efficient, don't do it that way.
    public System.DateTime lastTimeStamp;//time stamp of most recent sample
    public Sample lastSample;//most recent sample
    public AbstractValue lastSampleValue;

    //information about samples in aggregate:
    public int numberOfSamples = 0;//initialize at 0
}
