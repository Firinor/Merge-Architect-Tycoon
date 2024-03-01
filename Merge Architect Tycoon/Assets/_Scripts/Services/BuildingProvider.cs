﻿using System.Collections.Generic;
using _Scripts.Logic;
using _Scripts.Logic.Buildings;
using _Scripts.Services.PlayerProgressService;
using Zenject;

namespace _Scripts.Services
{
    public class BuildingProvider
    {
        private readonly Dictionary<string, BuildingPlace> _sceneBuildingsDictionary = new();

        private IPlayerProgressService _playerProgressService;

        [Inject]
        void Construct(IPlayerProgressService playerProgressService)
        {
            _playerProgressService = playerProgressService;
        }

        public void LoadCreatedBuildings()
        {
            Buldings buildings = _playerProgressService.Progress.Buldings;

            foreach (var buildingName in _sceneBuildingsDictionary.Keys)
            {
                if (buildings.CreatedBuildings.Contains(buildingName))
                {
                    CreateBuildingOnStart(buildingName);
                }
            }
        }

        public void AddBuildingPlaceToSceneDictionary(string buildingName, BuildingPlace buildingPlace)
        {
            _sceneBuildingsDictionary.Add(buildingName, buildingPlace);
        }

        public async void CreateBuildingInTimeAsync(string buildingName)
        {
            if (_sceneBuildingsDictionary.TryGetValue(buildingName, out var buildingPlace))
            {
                await buildingPlace.StartCreatingBuilding();
                _playerProgressService.Progress.AddBuilding(buildingName);
            }
        }

        private void CreateBuildingOnStart(string buildingName)
        {
            _sceneBuildingsDictionary[buildingName].SetBuildingState(BuildingStateEnum.BuildingFinished);
        }
    }
}