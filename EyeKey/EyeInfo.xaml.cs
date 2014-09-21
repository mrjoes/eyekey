using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TETControls.Calibration;
using TETCSharpClient;

namespace EyeKey
{
    /// <summary>
    /// Interaction logic for EyeInfo.xaml
    /// </summary>
    public partial class EyeInfo : UserControl
    {
        public EyeInfo()
        {
            InitializeComponent();

            MouseUp += new MouseButtonEventHandler(EyeInfo_MouseUp);
        }

        void EyeInfo_MouseUp(object sender, MouseButtonEventArgs e)
        {
            GazeHelper.Enabled = false;

            CalibrationRunner runner = new CalibrationRunner();
            runner.OnResult += new EventHandler<CalibrationRunnerEventArgs>(runner_OnResult);
            runner.Start();
        }

        void runner_OnResult(object sender, CalibrationRunnerEventArgs e)
        {
            GazeHelper.Enabled = true;
        }
    }
}
