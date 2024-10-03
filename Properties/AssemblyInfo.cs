using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(SinmaiAssist.BuildInfo.Description)]
[assembly: AssemblyDescription(SinmaiAssist.BuildInfo.Description)]
[assembly: AssemblyCompany(SinmaiAssist.BuildInfo.Company)]
[assembly: AssemblyProduct(SinmaiAssist.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + SinmaiAssist.BuildInfo.Author)]
[assembly: AssemblyTrademark(SinmaiAssist.BuildInfo.Company)]
[assembly: AssemblyVersion(SinmaiAssist.BuildInfo.Version)]
[assembly: AssemblyFileVersion(SinmaiAssist.BuildInfo.Version)]
[assembly: MelonInfo(typeof(SinmaiAssist.SinmaiAssist), SinmaiAssist.BuildInfo.Name, SinmaiAssist.BuildInfo.Version, SinmaiAssist.BuildInfo.Author, SinmaiAssist.BuildInfo.DownloadLink)]
[assembly: MelonColor()]
[assembly: MelonOptionalDependencies( "ChimeLib.NET")]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]