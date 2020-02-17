using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace Obnoxious_Screenshot_Capturing_Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isCaptureOn = false;
        long numberOfScreenShotsTaken = 0;

        PrintScreen ps = new PrintScreen();

        string directoryPath;
        string screenshotNamingConvention;
        Thread screenshotCaptureThread;
        int screenshotSpeed;

        public MainWindow()
        {
            InitializeComponent();
            screenshotCaptureThread = new Thread(new ThreadStart(this.CaptureScreenShot));
            DirectoryPathTextBox.Text = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            screenshotCaptureThread.Start();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var FD = new System.Windows.Forms.FolderBrowserDialog();
            if (FD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(FD.SelectedPath);
                DirectoryPathTextBox.Text = directory.FullName.ToString();
            }
        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            directoryPath = DirectoryPathTextBox.Text.ToString() + "\\";
            directoryPath.Replace(@"\", @"\\");
            screenshotNamingConvention = ScreenShotName.Text.ToString();
            double tempSpeed;
            if (Double.TryParse(ScreenshotSpeed.Text.ToString(), out tempSpeed)) {
                tempSpeed *= 1000;
                screenshotSpeed = (int)tempSpeed;
            }

            isCaptureOn = !isCaptureOn;
            StartStopButton.Background = isCaptureOn ? Brushes.Red : Brushes.CornflowerBlue;
            StartStopButton.Content = isCaptureOn ? "Stop" : "Start";
        }

        private void CaptureScreenShot()
        {
            while(true)
            {
                if (isCaptureOn)
                {
                    ps.CaptureScreenToFile(directoryPath + screenshotNamingConvention + numberOfScreenShotsTaken.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    numberOfScreenShotsTaken++;
                }
                Thread.Sleep(screenshotSpeed);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            screenshotCaptureThread.Abort();
        }
    }
}
