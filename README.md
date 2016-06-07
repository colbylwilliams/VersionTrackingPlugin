## Version Tracking Plugin for Xamarin and Windows 

Track which versions of your Xamarin.iOS, Xamarin.Mac, Xamarin.Android, or Windows app a user has previously installed.

## Setup ![NuGet](https://img.shields.io/nuget/v/Plugin.VersionTracking.svg?label=NuGet)
* Available on NuGet: https://www.nuget.org/packages/Plugin.VersionTracking/1.0.1
* Install into your PCL project and Client projects.

##### Compatible with Xamarin and Windows projects with or without Xamarin.Forms
There is no requirement of a dependency service; a built-in Singleton exposes the functionality.   

##### Platform Support

|Platform|Supported|Version|
| ------------------- | :-----------: | :------------------: |
|Xamarin.iOS|Yes|iOS 7+|
|Xamarin.iOS Unified|Yes|iOS 7+|
|Xamarin.Android|Yes|API 10+|
|Windows Phone Silverlight|Yes|8.0+|
|Windows Phone RT|Yes|8.1+|
|Windows Store RT|Yse|8.1+|
|Windows 10 UWP|Yes|10+|
|Xamarin.Mac|Yes||


## API Usage

Call this on each app launch inside `DidFinishLaunching` (Xamarin.iOS), `FinishedLaunching` (Xamarin.Mac), or the MainActivity's `OnCreate` (Xamarin.Android)

```C#
CrossVersionTracking.Current.Track ();
```

Then call these whenever you want (in these examples the user has launched a bunch of previous versions, and this is the first time he's launched the new version 1.0.11):

```C#
var vt = CrossVersionTracking.Current;

vt.IsFirstLaunchEver;        //Returns: False
vt.IsFirstLaunchForVersion;  //Returns: True
vt.IsFirstLaunchForBuild;    //Returns: True

vt.CurrentVersion;           //Returns: 1.0.11
vt.PreviousVersion;          //Returns: 1.0.10
vt.FirstInstalledVersion;    //Returns: 1.0.0
vt.VersionHistory;           //Returns: [ 1.0.0, 1.0.1, 1.0.2, 1.0.3, 1.0.10, 1.0.11 ]

vt.CurrentBuild;             //Returns: 18
vt.PreviousBuild;            //Returns: 15
vt.FirstInstalledBuild;      //Returns: 1
vt.BuildHistory;             //Returns: [ 1, 2, 3, 4, 5, 8, 9, 10, 11, 13, 15, 18 ]
 ```

Or set up actions to be called on the first lauch of a specific version or build:

```C#
var vt = CrossVersionTracking.Current;

vt.OnFirstLaunchOfBuild ("18", () => Console.WriteLine ("First time Build 18 launched!"));
vt.OnFirstLaunchOfVersion ("1.0.11", () => Console.WriteLine ("First time Version 1.0.11 launched!"));
```


## Contributors
* [Colby Williams](https://github.com/colbylwilliams)
* _Originally inspired by [GBVersionTracking](https://github.com/lmirosevic/GBVersionTracking)_


#### License
The MIT License (MIT)
Copyright © 2016 Colby Williams
