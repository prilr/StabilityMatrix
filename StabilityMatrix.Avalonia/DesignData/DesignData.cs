﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaEdit.Utils;
using Microsoft.Extensions.Logging;
using NLog;
using StabilityMatrix.Avalonia.ViewModels;
using StabilityMatrix.Core.Helper.Factory;
using StabilityMatrix.Core.Models;
using StabilityMatrix.Core.Models.Packages;
using StabilityMatrix.Core.Python;
using StabilityMatrix.Core.Services;

namespace StabilityMatrix.Avalonia.DesignData;

public static class DesignData
{
    static DesignData()
    {
        var settingsManager = new SettingsManager
        {
            Settings =
            {
                InstalledPackages = new List<InstalledPackage>
                {
                    new()
                    {
                        DisplayName = "My Installed Package",
                        PackageName = "stable-diffusion-webui",
                        PackageVersion = "v1.0.0",
                        LibraryPath = "Packages\\example-webui",
                        LastUpdateCheck = DateTimeOffset.Now
                    }
                },
                ActiveInstalledPackage = new Guid()
            }
        };
        var packages = new List<BasePackage>
        {
            new A3WebUI(null!, settingsManager, null!, null!),
            new ComfyUI(null!, settingsManager, null!, null!)
        };
        var packageFactory = new PackageFactory(packages);

        LaunchPageViewModel = new LaunchPageViewModel(
            null!, settingsManager, packageFactory, new PyRunner());
        
        LaunchPageViewModel.InstalledPackages.AddRange(settingsManager.Settings.InstalledPackages);
        LaunchPageViewModel.SelectedPackage = settingsManager.Settings.InstalledPackages[0];
        
        PackageManagerViewModel = new PackageManagerViewModel(settingsManager, packageFactory);
        SettingsViewModel = new SettingsViewModel();

        MainWindowViewModel = new MainWindowViewModel
        {
            Pages = new List<PageViewModelBase>
            {
                LaunchPageViewModel,
                PackageManagerViewModel,
                SettingsViewModel
            }
        };
    }
    
    public static MainWindowViewModel MainWindowViewModel { get; }
    public static LaunchPageViewModel LaunchPageViewModel { get; }
    public static PackageManagerViewModel PackageManagerViewModel { get; }
    public static SettingsViewModel SettingsViewModel { get; }
}
