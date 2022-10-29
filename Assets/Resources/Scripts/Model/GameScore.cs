using System;
using System.Collections;
using System.Collections.Generic;
using Huawei.Agconnect.CloudDB;
using UnityEngine;

public class GameScore : CloudDBZoneObject
{
    public string id { get; set; }
    public string userid { get; set; }
    public int score { get; set; }
    public string category { get; set; }

    public GameScore() : base(typeof(GameScore))
    {
        id = Guid.NewGuid().ToString();
    }
}
