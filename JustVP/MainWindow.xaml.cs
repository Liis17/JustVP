using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace JustVP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Obsolete]
        public MainWindow()
        {
            InitializeComponent();
            Start();
            SetAudio();
        }
        [Obsolete]
        void Start()
        {
            try
            {
                Uri uri = new Uri(App.Pathfile, false);
                videoelement.Source = uri;
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(500);
                timer.Tick += timer_Tick;
                timer.Start();
                OFButton.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                videotextpanel.Text = ex.Message;
            }

        }

        private void timer_Tick(object? sender, EventArgs e)
        {
            if (videoelement.Source != null)
            {
                if (videoelement.NaturalDuration.HasTimeSpan)
                {
                    videotextpanel.Text = String.Format("{0} / {1}", videoelement.Position.ToString(@"hh\:mm\:ss"), videoelement.NaturalDuration.TimeSpan.ToString(@"hh\:mm\:ss"));

                    videoslider.Value = videoelement.Position.TotalSeconds;
                    string[] a = videotextpanel.Text.Split('/');
                    string[] b = a[1].Split(':');
                    int second = int.Parse(b[2]);
                    int minutes = int.Parse(b[1]) * 60;
                    int hour = int.Parse(b[0]) * 60 * 60;
                    int allsecond = second + minutes + hour;
                    videoslider.Maximum = allsecond;

                    if (videoelement.Position.TotalSeconds >= allsecond)
                    {
                        Close();
                    }
                }
                    
                
            }
        }

        void SetAudio()
        {
            audioslider.Value = 0.25;
            audioslider.Maximum = 1;
            videoelement.Volume = 0.25;
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    this.DragMove();
                }
            }
            catch
            {

            }
        }
        [Obsolete]
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            Stream? myStream = null;
            OpenFileDialog FileDialog = new OpenFileDialog();

            FileDialog.Title = "Выбрать файл";
            FileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\"; //3;
            FileDialog.Filter = "Выбрать файл |*.mp4; *.avi; *.MKV; *.MOV; | Все файлы | *.*";
            FileDialog.FilterIndex = 1;
            FileDialog.RestoreDirectory = true;
            FileDialog.RestoreDirectory = true;
            FileDialog.ShowDialog();
            try
            {
                if ((myStream = FileDialog.OpenFile()) != null)
                {
                    using (myStream)
                    {
                        App.Pathfile = FileDialog.FileName;
                        OFButton.Visibility = Visibility.Hidden;
                        Start();
                    }
                }
            }
            catch (Exception ex)
            {
                videotextpanel.Text = ex.Message;
            }
        }

        private void audioslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            videoelement.Volume = audioslider.Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}
