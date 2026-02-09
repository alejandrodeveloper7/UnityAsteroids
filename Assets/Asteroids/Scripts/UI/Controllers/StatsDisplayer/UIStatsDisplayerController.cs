using ACG.Core.Extensions;
using Asteroids.Core.Interfaces;
using Asteroids.Core.Interfaces.Enums;
using Asteroids.Core.Interfaces.Models;
using Asteroids.Core.ScriptableObjects.Configurations;
using Asteroids.Core.Services;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Asteroids.UI.Controllers
{
    public class UIStatsDisplayerController : MonoBehaviour
    {
        #region Fields

        [Header("References")]
        private readonly List<UIStatsDisplayerRowController> _currentRowsList = new();
        [Inject] private readonly DiContainer _container;

        [Header("Data")]
        [Inject] private readonly SO_StatsDisplayerConfiguration _displayerConfiguration;
        [SerializeField] private SO_StatsConfiguration _statsConfiguration;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            CreateStatsRows();
        }

        #endregion

        #region Rows Management

        private void CreateStatsRows()
        {
            foreach (StatConfiguration configuration in _statsConfiguration.StatsConfiguration)
                AddStatRow(configuration);
        }

        private void AddStatRow(StatConfiguration statConfiguration)
        {
            if (StatExist(statConfiguration.Id))            
                return;            

            GameObject newRow = _container.InstantiatePrefab(_displayerConfiguration.StatsRowPrefab, transform);
            UIStatsDisplayerRowController newController = newRow.GetComponent<UIStatsDisplayerRowController>();
            _currentRowsList.Add(newController);
            newController.Initialize(statConfiguration);
        }

        private void RemoveStatRow(StatIdType id)
        {
            foreach (UIStatsDisplayerRowController row in _currentRowsList)
                if (row.Id == id)
                    Destroy(row);

            _currentRowsList.RemoveNulls();
        }

        private void RemoveAllStatsRows()
        {
            foreach (UIStatsDisplayerRowController row in _currentRowsList)
                Destroy(row);

            _currentRowsList.Clear();
        }

        #endregion

        #region Values Management

        public void SetStatsValues(IHasStats data, bool progressively)
        {
            foreach (StatData stat in data.Stats)
            {
                object propertyValue = ReflectionService.GetPropertyValue(data, stat.PropertyName);
                float value = Convert.ToSingle(propertyValue);
                SetStatValue(stat.Id, value, progressively);
            }
        }

        private void SetStatValue(StatIdType id, float value, bool progressively)
        {
            foreach (UIStatsDisplayerRowController row in _currentRowsList)
                if (row.Id == id)
                    row.SetValue(value, progressively);
        }

        #endregion

        #region Utility

        private bool StatExist(StatIdType id)
        {
            foreach (UIStatsDisplayerRowController row in _currentRowsList)
                if (row.Id == id)
                    return true;

            return false;
        }

        #endregion
    }
}