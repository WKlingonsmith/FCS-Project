using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace UnitTestUtilities
{
    internal static class ThreadUtilities
    {
        private static readonly object m_appInitializingSync = new object();
        private static void EnsureApplication()
        {
            lock(m_appInitializingSync)
            {
                if(Application.Current != null)
                {
                    return;
                }

                var waitForApplication = new TaskCompletionSource<bool>();
                Thread thread = new Thread(new ThreadStart(() =>
                    {
                        var app = new Application();
                        app.Startup += (s, e) => { waitForApplication.SetResult(true); };
                        app.Run();
                    }));

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                //Task task = new Task(new Action(() =>
                //{
                //    var app = new Application();
                //    app.Startup += (s, e) => { waitForApplication.SetResult(true); };
                //    app.Run();
                //}));

                //task.Start();

                waitForApplication.Task.Wait();
            }
        }

        internal static void RunOnUIThread(Action action)
        {
            EnsureApplication();

            Exception exception = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }));

            if (exception != null)
            {
                throw exception;
            }
        }
    }
}
