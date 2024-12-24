using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CareData
{
    public CareType FilledType;
    public string FilledTime { get; protected set; }
    public string FilledDate { get; protected set; }
    public List<string> FilledDays { get; protected set; }
}