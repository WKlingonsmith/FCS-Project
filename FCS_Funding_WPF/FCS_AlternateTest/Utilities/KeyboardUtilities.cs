using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UnitTestUtilities
{
    class KeyboardUtilities
    {
        public static void SendKeys(string value)
        {
            System.Windows.Forms.SendKeys.SendWait(value);
        }

        internal static void SendEnter()
        {
            SendKeys("~");
        }

		internal static void SendBackspace()
		{
			SendKeys("{BACKSPACE}");
		}
    }
}
