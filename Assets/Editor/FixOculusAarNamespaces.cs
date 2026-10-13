using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEditor.Android;
using Debug = UnityEngine.Debug;

// AGP 9 (Unity 6.3+) fails the build when several AARs share a namespace.
// Meta XR SDK AARs (InteractionSdk, SDKTelemetry, OVRPlugin) all declare
// package="com.oculus.Integration". This gives each copied AAR a unique one.
// The AAR is updated with the JDK's jar tool, because zips rewritten by .NET
// are rejected by Jetifier's ZipInputStream.
public class FixOculusAarNamespaces : IPostGenerateGradleAndroidProject
{
    const string SharedNamespace = "package=\"com.oculus.Integration\"";

    public int callbackOrder => 10000;

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        var libs = Path.Combine(path, "libs");
        if (!Directory.Exists(libs))
            return;

        var jar = Path.Combine(AndroidExternalToolsSettings.jdkRootPath, "bin", "jar.exe");
        if (!File.Exists(jar))
        {
            Debug.LogError($"[FixOculusAarNamespaces] jar tool not found at {jar}");
            return;
        }

        foreach (var aar in Directory.GetFiles(libs, "*.aar"))
        {
            string manifest;
            using (var zip = ZipFile.OpenRead(aar))
            {
                var entry = zip.GetEntry("AndroidManifest.xml");
                if (entry == null)
                    continue;
                using (var reader = new StreamReader(entry.Open(), Encoding.UTF8))
                    manifest = reader.ReadToEnd();
            }

            if (!manifest.Contains(SharedNamespace))
                continue;

            var suffix = Path.GetFileNameWithoutExtension(aar).ToLowerInvariant().Replace("-", "").Replace("_", "");
            var ns = $"com.oculus.Integration.{suffix}";

            var tempDir = Path.Combine(Path.GetTempPath(), "FixOculusAarNamespaces", suffix);
            Directory.CreateDirectory(tempDir);
            File.WriteAllText(Path.Combine(tempDir, "AndroidManifest.xml"),
                manifest.Replace(SharedNamespace, $"package=\"{ns}\""), new UTF8Encoding(false));

            var psi = new ProcessStartInfo(jar, $"uf \"{aar}\" -C \"{tempDir}\" AndroidManifest.xml")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
            };
            using (var process = Process.Start(psi))
            {
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (process.ExitCode != 0)
                    Debug.LogError($"[FixOculusAarNamespaces] Failed to patch {Path.GetFileName(aar)}: {error}");
                else
                    Debug.Log($"[FixOculusAarNamespaces] {Path.GetFileName(aar)} -> {ns}");
            }
        }
    }
}
