using Fiddler;
using System.Security.Cryptography.X509Certificates;

namespace era_connection_fix
{
    public static class FiddlerCore
    {
        public static string rootCertFile = $"{Globals.SelfDataFolder}\\EraCert.p12";
        public const string rootCertificatePassword = "agab8HJBHVub75HVghv2ZWdf7GJji0CL9KttDht";

        private static bool EnsureRootCert()
        {
            CertMaker.createRootCert();

            X509Certificate2 rootCert = CertMaker.GetRootCertificate();
            rootCert.FriendlyName = "Era Connection Fix";
            File.WriteAllBytes(rootCertFile, rootCert.Export(X509ContentType.Cert));

            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            store.Add(rootCert);
            store.Close();

            return true;
        }

        public static bool Start()
        {
            if (EnsureRootCert())
            {
                CONFIG.IgnoreServerCertErrors = true;
                CONFIG.EnableIPv6 = true;
                CONFIG.QuietMode = true;
                FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming", true);

                var settings = new FiddlerCoreStartupSettingsBuilder()
                    .ListenOnPort((ushort)GetPort.GetAvailablePort(8888))
                    .RegisterAsSystemProxy()
                    .ChainToUpstreamGateway()
                    .DecryptSSL()
                    .OptimizeThreadPool()
                    .AllowRemoteClients()
                    .Build();

                FiddlerApplication.Startup(settings);
            }
            return true;
        }
        public static void Stop()
        {
            FiddlerApplication.Shutdown();
        }
        public static bool GetIsRunning()
        {
            if (!FiddlerApplication.IsStarted())
            {
                Console.WriteLine("[!] Proxy did not start properly\nPress any key to continue...");
                Console.ReadKey();
                FiddlerApplication.Shutdown();
                Program.Main();
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void Debug(Session oSession)
        {
            if (Globals.hostNames.Contains(oSession.hostname))
            {
                if (oSession.responseCode == 400 || 
                    oSession.responseCode == 404 || 
                    oSession.responseCode == 403 || 
                    oSession.responseCode == 401 || 
                    oSession.responseCode == 409 ||
                    oSession.responseCode == 422)
                {
                    Console.WriteLine($"[!] {oSession.url} returned code {oSession.responseCode}!");
                }
            }
        }

    }
}