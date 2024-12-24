using System;
using System.Collections.Generic;

[Serializable]
public class ManuringData : CareData
{
    public string Time;
    public string Date;
    public List<string> Days;

    public ManuringData(string time, string date, List<string> days)
    {
        FilledType = CareType.Manuring;
        Time = time;
        Date = date;
        Days = days;

        FilledTime = Time;
        FilledDays = Days;
        FilledDate = Date;
    }
}