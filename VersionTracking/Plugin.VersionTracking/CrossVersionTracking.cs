using Plugin.VersionTracking.Abstractions;
using System;

namespace Plugin.VersionTracking
{
  /// <summary>
  /// Cross platform VersionTracking implemenations
  /// </summary>
  public class CrossVersionTracking
  {
    static Lazy<IVersionTracking> Implementation = new Lazy<IVersionTracking>(() => CreateVersionTracking(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static IVersionTracking Current
    {
      get
      {
        var ret = Implementation.Value;
        if (ret == null)
        {
          throw NotImplementedInReferenceAssembly();
        }
        return ret;
      }
    }

    static IVersionTracking CreateVersionTracking()
    {
#if PORTABLE
        return null;
#else
        return new VersionTrackingImplementation();
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
}
