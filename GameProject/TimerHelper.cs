using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameProject
{
    public static class TimerHelper
    {
        public static void StartTimer(int milliseconds, Action callback)
        {
            Timer tempTimer = new Timer();
            tempTimer.Interval = milliseconds;
            tempTimer.Tick += (sender, e) =>
            {
                tempTimer.Stop();
                tempTimer.Dispose(); // Clean up memory
                callback(); // Invoke your action
            };
            tempTimer.Start();
        }
    }

}
