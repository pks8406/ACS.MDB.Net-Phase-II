using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace ACS.MDB.OA.Windows.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            TopshelfExitCode exitcode = HostFactory.Run(x =>
            {
                x.RunAsLocalSystem();
                x.SetDescription("Open Account Windows Service");
                x.SetDisplayName("Open Account Service");
                x.SetServiceName("OpenAccountService");

                x.Service<OAService>(service =>
                {
                    service.ConstructUsing(name => new OAService());

                    service.WhenStarted(tc =>
                    {


                        tc.Start();
                        x.StartAutomatically();

                    });

                    service.WhenStopped(tc =>
                    {
                        tc.Stop();
                    });

                });
            });
        }
    }
}
