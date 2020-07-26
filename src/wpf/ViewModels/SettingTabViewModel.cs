using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Extensions.Configuration;
using MaterialDesignThemes.Wpf;
using Mitheti.Core;
using Mitheti.Core.Services;
using Mitheti.Wpf.Models;
using Mitheti.Wpf.Services;

namespace Mitheti.Wpf.ViewModels
{
    public class SettingTabViewModel
    {
        private const string TrackingDelayConfigKey = WatcherService.DelayConfigKey;
        private const string SavingDelayConfigKey = SavingService.DelayConfigKey;
        private const string DatabaseSizeConfigKey = SizeLimitDatabaseService.ConfigKey;
        private const string FilterModeConfigKey = AddFilterService.ModeConfigKey;
        private const string FilterListConfigKey = AddFilterService.ListConfigKey;

        private readonly IConfiguration _config;
        private readonly ISettingsManager _manager;

        private NumberRangeSetting _trackingDelaySetting;
        private NumberRangeSetting _savingDelaySetting;
        private NumberRangeSetting _databaseSizeSetting;

        public Dictionary<string, string> Localization { get; }

        public List<NumberRangeSetting> NumberRangeSettingsList { get; set; }
        public bool FilterMode { get; set; }
        public ObservableCollection<string> FilterList { get; set; }
        public TimePeriod ClearSetting { get; set; }
        public string ClearButtonLabel { get; set; }

        public SettingTabViewModel(ILocalizationService localization, IConfiguration config, ISettingsManager manager)
        {
            _config = config;
            _manager = manager;
            Localization = localization.Data;

            SetSettings();
        }

        private void SetSettings()
        {
            //TODO:FIXME:all method;
            const string templateBase = "Window:Setting:Option:{0}:Label:";
            const string labelConfigTemplateBegin = templateBase + "Begin";
            const string labelConfigTemplateEnd = templateBase + "End";

            _trackingDelaySetting = new NumberRangeSetting
            {
                Value = _config.GetValue<int>(TrackingDelayConfigKey),
                Min = WatcherService.MinDelay,
                Max = WatcherService.MaxDelay,
                Labels = new List<string>(new[]
                {
                    Localization[string.Format(labelConfigTemplateBegin, "TrackingDelay")],
                    Localization[string.Format(labelConfigTemplateEnd, "TrackingDelay")]
                })
            };

            _savingDelaySetting = new NumberRangeSetting
            {
                Value = _config.GetValue<int>(SavingDelayConfigKey),
                Min = SavingService.MinDelay,
                Max = SavingService.MaxDelay,
                Labels = new List<string>(new[]
                {
                    Localization[string.Format(labelConfigTemplateBegin, "SavingDelay")],
                    Localization[string.Format(labelConfigTemplateEnd, "SavingDelay")]
                })
            };

            _databaseSizeSetting = new NumberRangeSetting
            {
                Value = _config.GetValue<int>(DatabaseSizeConfigKey),
                Min = SizeLimitDatabaseService.MinSize,
                Max = SizeLimitDatabaseService.MaxSize,
                Labels = new List<string>(new[]
                {
                    Localization[string.Format(labelConfigTemplateBegin, "DatabaseSize")],
                    Localization[string.Format(labelConfigTemplateEnd, "DatabaseSize")]
                })
            };

            NumberRangeSettingsList = new List<NumberRangeSetting>(new[]
                {_trackingDelaySetting, _savingDelaySetting, _databaseSizeSetting});

            ClearSetting = new TimePeriod
            {
                Begin = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
                End = DateTime.Now
            };

            FilterMode = _config.GetValue<bool>(FilterModeConfigKey);
            FilterList = new ObservableCollection<string>(
                _config.GetList<string>(FilterListConfigKey));

            UpdateClearButtonLabel();
        }

        public void OnSaveClick(object sender, RoutedEventArgs e) => Save();

        private void Save()
        {
            var data = _manager.GetConfigAsDictionary(_config);

            data[FilterModeConfigKey] = FilterMode.ToString();
            for (var i = 1; i < FilterList.Count; i++)
            {
                data[$"{FilterListConfigKey}:{i}"] = FilterList[i];
            }

            data[TrackingDelayConfigKey] = _trackingDelaySetting.Value.ToString();
            data[SavingDelayConfigKey] = _savingDelaySetting.Value.ToString();
            data[DatabaseSizeConfigKey] = _databaseSizeSetting.Value.ToString();

            _manager.SaveConfiguration(data);
        }

        public void DeleteFilterListItem(object sender, RoutedEventArgs args)
            => FilterList.Remove((sender as Chip).Content as string);

        public void AddFilterListItem(string value, DialogClosingEventArgs arg)
        {
            if (!(bool) arg.Parameter || string.IsNullOrEmpty(value) || FilterList.Contains(value))
            {
                return;
            }

            FilterList.Add(value);
        }

        private string GetDatabaseSizeString()
        {
            var bytes = Localization["Window:Setting:Action:Clear:Info:Bytes"];
            var size = _manager.GetDatabaseSize() / int.Parse(bytes);
            return string.Format(Localization["Window:Setting:Action:Clear:Info:Label"], size);
        }

        public void UpdateClearButtonLabel()
        {
            var size = GetDatabaseSizeString();
            ClearButtonLabel = string.Format(Localization["Window:Setting:Action:Clear:Button"], size);
        }
    }
}