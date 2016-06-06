## VersionTracking

Track which versions of your Xamarin.iOS, Xamarin.Mac, Xamarin.Android, or Windows app a user has previously installed.

_Inspired by [GBVersionTracking](https://github.com/lmirosevic/GBVersionTracking)_


### Xamarin, Windows, and Xamarin.Forms
This NuGet can be used for classic Xamarin and Windows development with or without Xamarin.Forms. There is no requirement of a dependency service as it has a built in Singleton to access the functionality.

## Setup
* Available on NuGet: (soon)
* Install into your PCL project and Client projects.

**Platform Support**

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


### API Usage

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


#### Contributors
* [colbylwilliams](https://github.com/colbylwilliams)


#### License
The MIT License (MIT)
Copyright (c) 2016 Colby Williams