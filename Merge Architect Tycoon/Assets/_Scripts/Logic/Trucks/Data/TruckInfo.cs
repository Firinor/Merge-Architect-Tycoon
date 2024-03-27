﻿using UnityEngine;

[CreateAssetMenu(fileName = "TruckConfig", menuName = "StaticData/TruckConfig")]
public class TruckInfo : ScriptableObject
{
    public TruckUpdates[] TruckUpdates;
    public int BoostLimit;
    public TruckResources[] MergeItems;
}