using Plugin.VersionTracking.Abstractions;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Windows.ApplicationModel;

#if SILVERLIGHT
using System.IO.IsolatedStorage;
#else
using Windows.Storage;
#endif

namespace Plugin.VersionTracking
{
    /// <summary>
    /// Implementation for VersionTracking
    /// </summary>
    public class VersionTrackingImplementation : IVersionTracking
    {
#if SILVERLIGHT
        private static IsolatedStorageSettings AppSettings {
            get { return IsolatedStorageSettings.ApplicationSettings; }
        }
#else
        private static ApplicationDataContainer AppSettings {
            get { return ApplicationData.Current.LocalSettings; }
        }
#endif

        private readonly object locker = new object();

        Dictionary<string, List<string>> versionTrail;

        bool isFirstLaunchEver;
        bool isFirstLaunchForVersion;
        bool isFirstLaunchForBuild;

        const string xamUserDefaultsVersionTrailKey = @"xamVersionTrail";
        const string xamVersionsKey = @"xamVersion";
        const string xamBuildsKey = @"xamBuild";


        /// <summary>
        /// Call this as the VERY FIRST THING in FinishedLaunching
        /// </summary>
        public void Track()
        {
            var needsSync = false;

            // load history
#if SILVERLIGHT
            var noValues = (!AppSettings.Contains(xamVersionsKey) || !AppSettings.Contains(xamBuildsKey));
#else
            var noValues = (!AppSettings.Values.ContainsKey(xamVersionsKey) || !AppSettings.Values.ContainsKey(xamBuildsKey));
#endif

            if (noValues) {

                isFirstLaunchEver = true;

                versionTrail = new Dictionary<string, List<string>> {
                        { xamVersionsKey, new List<string>() },
                        { xamBuildsKey, new List<string>() }
                    };

            } else {

#if SILVERLIGHT
                var oldVersionList = AppSettings[xamVersionsKey] as List<string>;

                var oldBuildList = AppSettings[xamBuildsKey] as List<string>;
#else
                var oldVersionList = deserializeStringToList(AppSettings.Values[xamVersionsKey] as string);

                var oldBuildList = deserializeStringToList(AppSettings.Values[xamBuildsKey] as string);
#endif

                versionTrail = new Dictionary<string, List<string>> {
                        { xamVersionsKey, oldVersionList },
                        { xamBuildsKey, oldBuildList }
                    };

                isFirstLaunchEver = false;

                needsSync = true;
            }


            //check if this version was previously launched
            if (versionTrail[xamVersionsKey].Contains(CurrentVersion)) {

                isFirstLaunchForVersion = false;

            } else {

                isFirstLaunchForVersion = true;

                versionTrail[xamVersionsKey].Add(CurrentVersion);

                needsSync = true;
            }

            //check if this build was previously launched
            if (versionTrail[xamBuildsKey].Contains(CurrentBuild)) {

                isFirstLaunchForBuild = false;

            } else {

                isFirstLaunchForBuild = true;

                versionTrail[xamBuildsKey].Add(CurrentBuild);

                needsSync = true;
            }

            //store the new version stuff
            if (needsSync) {

                lock (locker) {
#if SILVERLIGHT
                    if (!AppSettings.Contains(xamVersionsKey)) {
                        AppSettings.Add(xamVersionsKey, versionTrail[xamVersionsKey]);
                    } else {
                        AppSettings[xamVersionsKey] = versionTrail[xamVersionsKey];
                    }

                    if (!AppSettings.Contains(xamBuildsKey)) {
                        AppSettings.Add(xamBuildsKey, versionTrail[xamBuildsKey]);
                    } else {
                        AppSettings[xamBuildsKey] = versionTrail[xamBuildsKey];
                    }

                    AppSettings.Save();
#else
                    if (!AppSettings.Values.ContainsKey(xamVersionsKey)) {
                        AppSettings.CreateContainer(xamVersionsKey, ApplicationDataCreateDisposition.Always);
                    }
                    if (AppSettings.Values.ContainsKey(xamBuildsKey)) {
                        AppSettings.CreateContainer(xamBuildsKey, ApplicationDataCreateDisposition.Always);
                    }

                    AppSettings.Values[xamVersionsKey] = serializeList(versionTrail[xamVersionsKey].ToList());
                    AppSettings.Values[xamBuildsKey] = serializeList(versionTrail[xamBuildsKey].ToList());
#endif
                }
            }
        }


        /// <summary>
        /// Check if this is the first time ever that the app is launched.
        /// </summary>
        /// <value>The is first launch ever.</value>
        public bool IsFirstLaunchEver => isFirstLaunchEver;


        /// <summary>
        /// Check if this is the first time that this particular version is being launched.
        /// </summary>
        /// <value>The is first launch for current version.</value>
        public bool IsFirstLaunchForVersion => isFirstLaunchForVersion;


        /// <summary>
        /// Check if this is the first time that this particular build is being launched.
        /// </summary>
        /// <value>The is first launch for current build.</value>
        public bool IsFirstLaunchForBuild => isFirstLaunchForBuild;


        private Package CurrentPackage
        {
            get
            {
#if SILVERLIGHT
                return Windows.Phone.Management.Deployment.InstallationManager.FindPackagesForCurrentPublisher().First();
#else
                return Package.Current;
#endif
            }
        }


        /// <summary>
        /// Returns the current version of the app, as defined in the PList, e.g. "4.3".
        /// </summary>
        /// <value>The current version.</value>
        public string CurrentVersion {
            get {
                var version = CurrentPackage.Id.Version;

                return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }


        /// <summary>
        /// Returns the current build of the app, as defined in the PList, e.g. "4300".
        /// </summary>
        /// <value>The current build.</value>
        public string CurrentBuild => CurrentPackage.Id.Version.Build.ToString();


        /// <summary>
        /// Returns the previous version of the app, e.g. "4.3".
        /// </summary>
        /// <value>The previous version.</value>
        public string PreviousVersion {
            get {

                var count = versionTrail?[xamVersionsKey]?.Count ?? 0;

                return (count >= 2) ? versionTrail[xamVersionsKey][count - 2] : null;
            }
        }


        /// <summary>
        /// Returns the previous build of the app, "4300".
        /// </summary>
        /// <value>The previous build.</value>
        public string PreviousBuild {
            get {

                var count = versionTrail?[xamBuildsKey]?.Count ?? 0;

                return (count >= 2) ? versionTrail[xamBuildsKey][count - 2] : null;
            }
        }


        /// <summary>
        /// Returns the version which the user first installed the app.
        /// </summary>
        /// <value>The first installed version.</value>
        public string FirstInstalledVersion => versionTrail?[xamVersionsKey]?.FirstOrDefault();


        /// <summary>
        /// Returns the build which the user first installed the app.
        /// </summary>
        /// <value>The first installed build.</value>
        public string FirstInstalledBuild => versionTrail?[xamBuildsKey]?.FirstOrDefault();


        /// <summary>
        /// Returns a List of versions which the user has had installed, e.g. ["3.5", "4.0", "4.1"]. 
        /// The List is ordered from first version installed to (including) the current version
        /// </summary>
        /// <value>The version history.</value>
        public List<string> VersionHistory => versionTrail?[xamVersionsKey] ?? new List<string>();


        /// <summary>
        /// Returns a List of builds which the user has had installed, e.g. ["3500", "4000", "4100"].
        /// The List is ordered from first build installed to (including) the current build
        /// </summary>
        /// <value>The build history.</value>
        public List<string> BuildHistory => versionTrail?[xamBuildsKey] ?? new List<string>();


        /// <summary>
        /// Check if this is the first launch for a particular version number. 
        /// Useful if you want to execute some code for first time launches of a particular version 
        /// (like db migrations?).
        /// </summary>
        /// <returns>The first launch for version.</returns>
        /// <param name="version">Version.</param>
        public bool FirstLaunchForVersion(string version) =>
            (CurrentVersion.Equals(version, StringComparison.OrdinalIgnoreCase)) && isFirstLaunchForVersion;


        /// <summary>
        /// Check if this is the first launch for a particular build number. 
        /// Useful if you want to execute some code for first time launches of a particular version 
        /// (like db migrations?).
        /// </summary>
        /// <returns>The first launch for build.</returns>
        /// <param name="build">Build.</param>
        public bool FirstLaunchForBuild(string build) =>
            (CurrentBuild.Equals(build, StringComparison.OrdinalIgnoreCase)) && isFirstLaunchForBuild;


        /// <summary>
        /// Calls block if the condition is satisfied that the current version matches `version`, 
        /// and this is the first time this app version is being launched.
        /// </summary>
        /// <returns>The first launch of version.</returns>
        /// <param name="version">Version.</param>
        /// <param name="block">Block.</param>
        public void OnFirstLaunchOfVersion(string version, Action block)
        {
            if (FirstLaunchForVersion(version)) block?.Invoke();
        }


        /// <summary>
        /// Calls block if the condition is satisfied that the current build matches `build`, 
        /// and this is the first time this app build is being launched.
        /// </summary>
        /// <returns>The first launch of build.</returns>
        /// <param name="build">Build.</param>
        /// <param name="block">Block.</param>
        public void OnFirstLaunchOfBuild(string build, Action block)
        {
            if (FirstLaunchForBuild(build)) block?.Invoke();
        }

        private string serializeList(List<string> list)
        {
            return string.Join(",", list.ToArray());
        }

        private List<string> deserializeStringToList(string listAsString)
        {
            return listAsString.Split(',').ToList();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("VersionTracking");
            sb.AppendFormat("  {0}{1}\n", "IsFirstLaunchEver".PadRight(25), IsFirstLaunchEver);
            sb.AppendFormat("  {0}{1}\n", "IsFirstLaunchForVersion".PadRight(25), IsFirstLaunchForVersion);
            sb.AppendFormat("  {0}{1}\n", "IsFirstLaunchForBuild".PadRight(25), IsFirstLaunchForBuild);
            sb.AppendFormat("  {0}{1}\n", "CurrentVersion".PadRight(25), CurrentVersion);
            sb.AppendFormat("  {0}{1}\n", "PreviousVersion".PadRight(25), PreviousVersion);
            sb.AppendFormat("  {0}{1}\n", "FirstInstalledVersion".PadRight(25), FirstInstalledVersion);
            sb.AppendFormat("  {0}[ {1} ]\n", "VersionHistory".PadRight(25), string.Join(", ", VersionHistory));
            sb.AppendFormat("  {0}{1}\n", "CurrentBuild".PadRight(25), CurrentBuild);
            sb.AppendFormat("  {0}{1}\n", "PreviousBuild".PadRight(25), PreviousBuild);
            sb.AppendFormat("  {0}{1}\n", "FirstInstalledBuild".PadRight(25), FirstInstalledBuild);
            sb.AppendFormat("  {0}[ {1} ]\n", "BuildHistory".PadRight(25), string.Join(", ", BuildHistory));
            return sb.ToString();
        }
    }
}