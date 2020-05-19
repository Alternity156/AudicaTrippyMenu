using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;

[assembly: AssemblyTitle(TrippyMenu.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(TrippyMenu.BuildInfo.Company)]
[assembly: AssemblyProduct(TrippyMenu.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + TrippyMenu.BuildInfo.Author)]
[assembly: AssemblyTrademark(TrippyMenu.BuildInfo.Company)]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
//[assembly: Guid("")]
[assembly: AssemblyVersion(TrippyMenu.BuildInfo.Version)]
[assembly: AssemblyFileVersion(TrippyMenu.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]
[assembly: MelonModInfo(typeof(TrippyMenu.TrippyMenu), TrippyMenu.BuildInfo.Name, TrippyMenu.BuildInfo.Version, TrippyMenu.BuildInfo.Author, TrippyMenu.BuildInfo.DownloadLink)]


// Create and Setup a MelonModGame to mark a Mod as Universal or Compatible with specific Games.
// If no MelonModGameAttribute is found or any of the Values for any MelonModGame on the Mod is null or empty it will be assumed the Mod is Universal.
// Values for MelonModGame can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonModGame(null, null)]