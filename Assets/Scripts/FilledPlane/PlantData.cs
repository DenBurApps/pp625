using System;
using System.Collections.Generic;

[Serializable]
public class PlantData
{
    public string Name;
    public PlantCategory Category;
    public string Date;
    public string Description;
    public byte[] ImagePath;
    public List<CareData> CareDatas;

    public PlantData(string name, PlantCategory category, string date)
    {
        Name = name;
        Category = category;
        Date = date;
        CareDatas = new List<CareData>();
    }
}

public enum PlantCategory
{
    Plant,
    Flowers,
    Fruits,
    Trees,
    Seed,
    Seaweed,
    Cactus,
    None
}
