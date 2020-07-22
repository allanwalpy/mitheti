using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Extensions.Configuration;
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

        private readonly IConfiguration _config;
        private readonly ISettingsManager _manager;

        public Dictionary<string, string> Localization { get; }

        public LimitedCountableSetting<int> TrackingDelaySetting { get; set; }
        public LimitedCountableSetting<int> SavingDelaySetting { get; set; }
        public LimitedCountableSetting<int> DatabaseSizeSetting { get; set; }

        public TimePeriod ClearSetting { get; set; }

        public SettingTabViewModel(ILocalizationService localization, IConfiguration config, ISettingsManager manager)
        {
            _config = config;
            _manager = manager;
            Localization = localization.Data;

            TrackingDelaySetting = new LimitedCountableSetting<int>
            {
                Value = config.GetValue<int>(TrackingDelayConfigKey),
                Min = WatcherService.MinDelay,
                Max = WatcherService.MaxDelay
            };

            SavingDelaySetting = new LimitedCountableSetting<int>
            {
                Value = config.GetValue<int>(SavingDelayConfigKey),
                Min = SavingService.MinDelay,
                Max = SavingService.MaxDelay
            };

            DatabaseSizeSetting = new LimitedCountableSetting<int>
            {
                Value = config.GetValue<int>(DatabaseSizeConfigKey),
                Min = SizeLimitDatabaseService.MinSize,
                Max = SizeLimitDatabaseService.MaxSize
            };

            ClearSetting = new TimePeriod
            {
                Begin = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
                End = DateTime.Now
            };
        }

        public void OnSaveClick(object sender, RoutedEventArgs e) => Save();

        private void Save()
        {
            var data = _manager.GetConfigAsDictionary(_config);

            data[TrackingDelayConfigKey] = TrackingDelaySetting.Value.ToString();
            data[SavingDelayConfigKey] = SavingDelaySetting.Value.ToString();
            data[DatabaseSizeConfigKey] = DatabaseSizeSetting.Value.ToString();

            _manager.SaveConfiguration(data);

            //TODO: reload config;
        }
    }
}