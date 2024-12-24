using System;
using System.Collections.Generic;

[Serializable]
public class PlantCareData : CareData
{
    public string Time;
    public string Date;
    public List<string> Days;

    public PlantCareData(string time, string date, List<string> days)
    {
        FilledType = CareType.PlantCare;
        Time = time;
        Date = date;
        Days = days;
        
        FilledTime = Time;
        FilledDays = Days;
        FilledDate = Date;
    }
}
