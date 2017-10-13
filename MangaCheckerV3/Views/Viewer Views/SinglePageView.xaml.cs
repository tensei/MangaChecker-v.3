using System.Windows.Controls;
using System.Windows.Input;

namespace MangaCheckerV3.Views.Viewer_Views
{
    /// <summary>
    ///     Interaction logic for SinglePageView.xaml
    /// </summary>
    public partial class SinglePageView : UserControl
    {
        public SinglePageView()
        {
            InitializeComponent();
        }

        private void Flip_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D:
                case Key.Space:
                    Flip.GoForward();
                    break;
                case Key.A:
                case Key.Back:
                    Flip.GoBack();
                    break;
            }
        }
    }
}