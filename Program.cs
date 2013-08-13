using System;
using System.Windows.Forms;
using TwilioEmulator.Code;
using System.Linq;

namespace TwilioEmulator
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Count() > 1)
            {
                SystemController.ConsoleMode = true;
            }
            // when we go to console mode change things here
            var f = new MainForm();
            SystemController.Instance.theForm = f;
            SystemController.Instance.Logger = f;
            SystemController.Instance.StartUp();
            f.ControlerCreated();

            Application.Run(f);
        }
    }
}
