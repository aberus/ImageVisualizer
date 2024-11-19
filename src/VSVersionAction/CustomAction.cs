using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.VisualStudio.Setup.Configuration;

namespace VSVersionAction
{
    public class CustomActions
    {
        private const int REGDB_E_CLASSNOTREG = unchecked((int)0x80040154);

        [CustomAction]
        public static ActionResult VSFindInstances2(Session session)
        {
            try
            {
                var query = new SetupConfiguration();
                var query2 = (ISetupConfiguration2)query;
                var e = query2.EnumAllInstances();

                var helper = (ISetupHelper)query;

                int fetched;
                var instances = new ISetupInstance[1];
                do
                {
                    e.Next(1, instances, out fetched);
                    if (fetched > 0)
                    {
                        PrintInstance(instances[0], helper, session);
                    }
                }
                while (fetched > 0);
            }
            catch (COMException ex) when (ex.HResult == REGDB_E_CLASSNOTREG)
            {
                session.Log("The query API is not registered. Assuming no instances are installed.");
            }
            catch (Exception ex)
            {
                session.Log($"Error 0x{ex.HResult:x8}: {ex.Message}");
            }
            return ActionResult.Success;
        }

        private static void PrintInstance(ISetupInstance instance, ISetupHelper helper, Session session)
        {
            var instance2 = (ISetupInstance2)instance;
            var state = instance2.GetState();
            session.Log($"InstanceId: {instance2.GetInstanceId()} ({(state == InstanceState.Complete ? "Complete" : "Incomplete")})");


            if ((state & InstanceState.Local) == InstanceState.Local)
            {
                string installationVersion = instance.GetInstallationVersion();
                ulong versionNumber = helper.ParseVersion(installationVersion);
                session.Log($"InstallationVersion: {installationVersion} ({versionNumber})");
                session.Log($"InstallationPath: {instance2.GetInstallationPath()}");

                var version = Version.Parse(instance2.GetInstallationVersion());

                //if (installationVersion.StartsWith("15.") && string.IsNullOrEmpty(session["VS2017_VERSION]"]))
                //{
                //    session["VS2017_VERSION"] = version.ToString(2);
                //}
                //if (installationVersion.StartsWith("16.") && string.IsNullOrEmpty(session["VS2019_VERSION"]))
                //{
                //    session["VS2019_VERSION"] = version.ToString(2);
                //}
                if (installationVersion.StartsWith("17.") && string.IsNullOrEmpty(session["VS2019_VERSION"]))
                {
                    session["VS2022_VERSION"] = version.ToString(2);
                }
            }
        }
    }
}
