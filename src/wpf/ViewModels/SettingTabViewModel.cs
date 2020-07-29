using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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

        public NumberRangeSetting TrackingDelaySetting { get; set; }
        public NumberRangeSetting SavingDelaySetting { get; set; }
        public NumberRangeSetting DatabaseSizeSetting { get; set; }
        public bool FilterMode { get; set; }
        public ObservableCollection<string> FilterList { get; set; }
        public TimePeriod ClearSetting { get; set; }
        public string ClearButtonLabel { get; set; }

        public SettingTabViewModel(IConfiguration config, ISettingsManager manager)
        {
            _config = config;
            _manager = manager;

            SetSettings();
        }

        private void SetSettings()
        {
            //TODO:FIXME:all method;
            //TODO:FIXME:??;
            const string keyBase = "Window:Setting:Option:";
            const string settingEnd = ":Label";
            const string valueEnd = ":Value:Label";

            TrackingDelaySetting = new NumberRangeSetting
            {
                Value = _config.GetValue<int>(TrackingDelayConfigKey),
                Min = WatcherService.MinDelay,
                Max = WatcherService.MaxDelay,
                SettingLabel = $"{keyBase}TrackingDelay{settingEnd}".Translate(),
                ValueLabel = $"{keyBase}TrackingDelay{valueEnd}".Translate()
            };

            SavingDelaySetting = new NumberRangeSetting
            {
                Value = _config.GetValue<int>(SavingDelayConfigKey),
                Min = SavingService.MinDelay,
                Max = SavingService.MaxDelay,
                SettingLabel = $"{keyBase}SavingDelay{settingEnd}".Translate(),
                ValueLabel = $"{keyBase}SavingDelay{valueEnd}".Translate()
            };

            DatabaseSizeSetting = new NumberRangeSetting
            {
                Value = _config.GetValue<int>(DatabaseSizeConfigKey),
                Min = SizeLimitDatabaseService.MinSize,
                Max = SizeLimitDatabaseService.MaxSize,
                SettingLabel = $"{keyBase}DatabaseSize{settingEnd}".Translate(),
                ValueLabel = $"{keyBase}DatabaseSize{valueEnd}".Translate()
            };

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

            data[TrackingDelayConfigKey] = TrackingDelaySetting.Value.ToString();
            data[SavingDelayConfigKey] = SavingDelaySetting.Value.ToString();
            data[DatabaseSizeConfigKey] = DatabaseSizeSetting.Value.ToString();

            _manager.SaveConfiguration(data);
        }

        public void DeleteFilterListItem(object sender, RoutedEventArgs args)
        {
            if (!((sender as ContentControl)?.Content is string value)) return;

            FilterList.Remove(value);
        }

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
            var bytes = "Window:Setting:Action:Clear:Info:Bytes".Translate();
            var size = _manager.GetDatabaseSize() / int.Parse(bytes);
            return string.Format("Window:Setting:Action:Clear:Info:Label".Translate(), size);
        }

        public void UpdateClearButtonLabel()
        {
            var size = GetDatabaseSizeString();
            ClearButtonLabel = string.Format("Window:Setting:Action:Clear:Button".Translate(), size);
        }
    }
}