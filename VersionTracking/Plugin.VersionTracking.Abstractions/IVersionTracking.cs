using System;
using System.Collections.Generic;

namespace Plugin.VersionTracking.Abstractions
{
  /// <summary>
  /// Interface for VersionTracking
  /// </summary>
  public interface IVersionTracking
  {
        /// <summary>
        /// Call this as the VERY FIRST THING in FinishedLaunching
        /// </summary>
        void Track();

        /// <summary>
        /// Check if this is the first time ever that the app is launched.
        /// </summary>
        /// <value>The is first launch ever.</value>
        bool IsFirstLaunchEver { get;  }


        /// <summary>
        /// Check if this is the first time that this particular version is being launched.
        /// </summary>
        /// <value>The is first launch for current version.</value>
        bool IsFirstLaunchForVersion { get; }


        /// <summary>
        /// Check if this is the first time that this particular build is being launched.
        /// </summary>
        /// <value>The is first launch for current build.</value>
        bool IsFirstLaunchForBuild { get; }


        /// <summary>
        /// Returns the current version of the app, as defined in the PList, e.g. "4.3".
        /// </summary>
        /// <value>The current version.</value>
        string CurrentVersion { get; }


        /// <summary>
        /// Returns the current build of the app, as defined in the PList, e.g. "4300".
        /// </summary>
        /// <value>The current build.</value>
        string CurrentBuild { get; }


        /// <summary>
        /// Returns the previous version of the app, e.g. "4.3".
        /// </summary>
        /// <value>The previous version.</value>
        string PreviousVersion { get; }


        /// <summary>
        /// Returns the previous build of the app, "4300".
        /// </summary>
        /// <value>The previous build.</value>
        string PreviousBuild { get; }


        /// <summary>
        /// Returns the version which the user first installed the app.
        /// </summary>
        /// <value>The first installed version.</value>
        string FirstInstalledVersion { get; }


        /// <summary>
        /// Returns the build which the user first installed the app.
        /// </summary>
        /// <value>The first installed build.</value>
        string FirstInstalledBuild { get; }


        /// <summary>
        /// Returns a List of versions which the user has had installed, e.g. ["3.5", "4.0", "4.1"]. 
        /// The List is ordered from first version installed to (including) the current version
        /// </summary>
        /// <value>The version history.</value>
        List<string> VersionHistory { get; }


        /// <summary>
        /// Returns a List of builds which the user has had installed, e.g. ["3500", "4000", "4100"].
        /// The List is ordered from first build installed to (including) the current build
        /// </summary>
        /// <value>The build history.</value>
        List<string> BuildHistory { get; }


        /// <summary>
        /// Check if this is the first launch for a particular version number. 
        /// Useful if you want to execute some code for first time launches of a particular version 
        /// (like db migrations?).
        /// </summary>
        /// <returns>The first launch for version.</returns>
        /// <param name="version">Version.</param>
        bool FirstLaunchForVersion(string version);


        /// <summary>
        /// Check if this is the first launch for a particular build number. 
        /// Useful if you want to execute some code for first time launches of a particular version 
        /// (like db migrations?).
        /// </summary>
        /// <returns>The first launch for build.</returns>
        /// <param name="build">Build.</param>
        bool FirstLaunchForBuild(string build);


        /// <summary>
        /// Calls block if the condition is satisfied that the current version matches `version`, 
        /// and this is the first time this app version is being launched.
        /// </summary>
        /// <returns>The first launch of version.</returns>
        /// <param name="version">Version.</param>
        /// <param name="block">Block.</param>
        void OnFirstLaunchOfVersion(string version, Action block);


        /// <summary>
        /// Calls block if the condition is satisfied that the current build matches `build`, 
        /// and this is the first time this app build is being launched.
        /// </summary>
        /// <returns>The first launch of build.</returns>
        /// <param name="build">Build.</param>
        /// <param name="block">Block.</param>
        void OnFirstLaunchOfBuild(string build, Action block);
    }
}
