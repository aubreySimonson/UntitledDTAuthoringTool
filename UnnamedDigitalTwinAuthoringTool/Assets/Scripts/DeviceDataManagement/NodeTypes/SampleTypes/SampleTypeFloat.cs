using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sub-type of sampletype for containing additional information about floats.
///
/// </summary>

public class SampleTypeFloat : SampleType
{
    public float minVal;
    public float maxVal;
    public float meanVal;
    public float total;//mostly useful for calculating the average
}
