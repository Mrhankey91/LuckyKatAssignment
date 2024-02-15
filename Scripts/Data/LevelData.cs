[System.Serializable]
public class LevelData
{
    public int level;
    public string[] background;

    public FloorData[] floors;
}

[System.Serializable]
public class FloorData
{
    public int floor;
    public PartData[] parts;
}


[System.Serializable]
public class PartData
{
    public string key = "";
    public float rotation = 0;
    public bool mirrored = false;
}