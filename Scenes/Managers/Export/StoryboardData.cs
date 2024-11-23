using System;
using System.Collections.Generic;

public class StoryboardData
{
    public StoryboardDataProject Project { get; set; }
    public StoryboardDataSb Storyboard {  get; set; }
}

public class StoryboardDataProject
{
    public DataEditor Editor { get; set; }
    public DataOsu Osu { get; set; }
}

public class StoryboardDataSb
{
    public List<KeyValuePair<string, DataObject>> Group { get; set; }
}


