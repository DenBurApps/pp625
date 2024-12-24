using System;
using System.Collections.Generic;

[Serializable]
public class WateringData : CareData
{
    public string Time;
    public string Date;
    public List<string> Days;

    public WateringData(string time, string date, List<string> wateringDays)
    {
        FilledType = CareType.Watering;
        Time = time;
        Date = date;
        Days = wateringDays;
        
        FilledTime = Time;
        FilledDays = Days;
        FilledDate = Date;
    }
}
