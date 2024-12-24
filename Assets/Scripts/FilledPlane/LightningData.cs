using System;

[Serializable]
public class LightningData : CareData
{
    public LightType Type;

    public LightningData(LightType type)
    {
        FilledType = CareType.Lightning;
        Type = type;
    }
}

public enum LightType
{
    Sunlight,
    Lamp,
    Ultraviolet,
    None
}
