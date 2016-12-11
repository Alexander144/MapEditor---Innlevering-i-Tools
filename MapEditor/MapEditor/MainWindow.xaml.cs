using System.Windows;
using MapEditor.ViewModel;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        protected int columnWidth = 50;
        protected int rowHeight = 50;
        protected string[,] fields;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            fields = new string[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    fields[i, j] = "Sprites/mini.jpg";
                }
            }

            InitGrid();



        }

        public void InitGrid()
        {
            UniformGrid grid = new UniformGrid();
            grid.Columns = 10;
            grid.Rows = 10;
            grid.Width = columnWidth * 10;
            grid.Height = rowHeight * 10;
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {

                    Image img = new Image();
                    BitmapImage imageBitmap = new BitmapImage(new Uri(fields[x, y], UriKind.Relative));

                    img.Source = imageBitmap;
                    img.Stretch = System.Windows.Media.Stretch.Fill;
                    img.AddHandler(MouseEnterEvent, new RoutedEventHandler(MouseHoveringOverTile));
                    img.AddHandler(MouseLeaveEvent, new RoutedEventHandler(MouseExitingTile));
                    grid.Children.Add(img);
                }
                /*
                {
                    Rectangle rect = new Rectangle();
                    rect.Fill = someColor;
                    grid.Children.Add(rect);
                }
                */
            }


            Canvas.SetLeft(grid, 0);
            Canvas.SetTop(grid, 0);
            MainCanvas.Children.Add(grid);
        }

        private void MouseHoveringOverTile(object sender, RoutedEventArgs e)
        {
                if (sender.GetType() == typeof(Image))
                {
                    Image senderImg = (Image) sender;
                    senderImg.Opacity = 0.5;
                
                }
        }

        private void MouseExitingTile(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(Image))
            {
                Image senderImg = (Image)sender;
                senderImg.Opacity = 1.0;

            }
        }

    }
}