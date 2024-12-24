using System;
using UnityEngine;

[Serializable]
public class TransplantationData : CareData
{
    public string Date;
    public string Time;

    public TransplantationData(string date, string time)
    {
        FilledType = CareType.Transplantation;
        Date = date;
        Time = time;
    }
}
