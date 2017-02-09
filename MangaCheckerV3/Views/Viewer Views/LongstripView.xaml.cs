using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MangaCheckerV3.Views.Viewer_Views {
    /// <summary>
    ///     Interaction logic for LongstripView.xaml
    /// </summary>
    public partial class LongstripView : UserControl {
        private static Timer _loopTimer;

        private int _direction;

        public LongstripView() {
            InitializeComponent();

            _loopTimer = new Timer {
                Interval = 10,
                Enabled = false,
                AutoReset = true
            };
            _loopTimer.Elapsed += LoopTimerEvent;
        }

        private void LoopTimerEvent(object source, ElapsedEventArgs e) {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                var x = scviewer.VerticalOffset;
                scviewer.ScrollToVerticalOffset(x + _direction);
                x = scviewer.VerticalOffset;
            }));
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                _loopTimer.Enabled = true;
                _direction = (int) SliderScrollSpeed.Value;
            }
            if (e.ChangedButton != MouseButton.Right) {
                return;
            }
            _loopTimer.Enabled = true;
            _direction = -(int) SliderScrollSpeed.Value;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e) {
            _loopTimer.Enabled = false;
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e) {
            _loopTimer.Enabled = false;
        }
    }
}