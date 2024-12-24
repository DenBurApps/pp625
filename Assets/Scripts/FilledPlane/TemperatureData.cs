using System;

[Serializable]
public class TemperatureData : CareData
{
    public string Temperature;

    public TemperatureData(string temperature = null)
    {
        FilledType = CareType.Temperature;
        Temperature = temperature;
    }
}