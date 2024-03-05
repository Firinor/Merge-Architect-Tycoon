﻿using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IStaticDataService
{
    UniTask Initialize();
    Sprite BuildInProgressSprite { get; }
    Dictionary<string, BuildingInfo> BuildingData { get; }
    BuildingInfo GetBuildingData(string buildingName);
    DistrictInfo GetDistrictData(int districtId);
}