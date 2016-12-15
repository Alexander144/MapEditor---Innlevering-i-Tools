﻿using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using CodeMvvm.View;
using CodeMvvm.ViewModel;

namespace MVVM_Light_eksempel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        protected int columnWidth = 50;
        protected int rowHeight = 50;
        private Tile[,] fields;

        protected TreeViewItem lastHierarchyImageClicked;
        private List<Tile> tileList;

        private Tile currentPreviewTile;

        private Point mouseDragStartingPosition;

        private Image currentPreviewImg;
        private RotateTransform rotate;

        private UniformGrid tileGrid;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            fields = new Tile[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    fields[i, j] = new Tile("defaultTile.png", 20 + i * columnWidth, 40 + j * rowHeight);
                    fields[i, j].Path = "pack://application:,,,/View/Sprites/defaultTile.png";
                }
            }


            tileList = new List<Tile>();

            InitGrid();
            InitTreeView();

        }

        public void InitGrid()
        {
            tileGrid = new UniformGrid();
            tileGrid.Columns = 10;
            tileGrid.Rows = 10;
            tileGrid.Width = columnWidth * 10;
            tileGrid.Height = rowHeight * 10;
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {

                    Image img = new Image();
                    BitmapImage imageBitmap = new BitmapImage(new Uri(fields[x, y].Path));

                    img.Source = imageBitmap;
                    img.Stretch = System.Windows.Media.Stretch.Fill;


                    img.AddHandler(MouseEnterEvent, new RoutedEventHandler(MouseHoveringOverTile));
                    img.AddHandler(MouseLeaveEvent, new RoutedEventHandler(MouseExitingTile));

                    img.AllowDrop = true;
                    img.AddHandler(DragEnterEvent, new DragEventHandler(DragEnteringTile));
                    img.AddHandler(DragLeaveEvent, new DragEventHandler(DragLeavingTile));
                    img.AddHandler(DropEvent, new DragEventHandler(DroppingOnTile));

                    fields[x, y].Image = img;

                    tileGrid.Children.Add(fields[x, y].Image);
                }
            }

            Canvas.SetLeft(tileGrid, 0);
            Canvas.SetTop(tileGrid, 0);
            MainCanvas.Children.Add(tileGrid);
        }

        public void InitTreeView()
        {
            foreach (string s in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;
                item.FontWeight = FontWeights.Normal;
                item.Items.Add(new object());
                item.Expanded += new RoutedEventHandler(folder_Expanded);
                DirectoryTreeView.Items.Add(item);
            }
        }


        //Events

        private void MouseHoveringOverTile(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(Image))
            {
                Image senderImg = (Image)sender;
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

        void folder_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(TreeViewItem))
            {
                TreeViewItem item = (TreeViewItem)sender;
                if (item.Items.Count == 1)
                {
                    item.Items.Clear();
                    try
                    {
                        foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                        {
                            TreeViewItem subitem = new TreeViewItem();
                            subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                            subitem.Tag = s;
                            subitem.FontWeight = FontWeights.Normal;
                            subitem.Items.Add(new object());
                            subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                            item.Items.Add(subitem);
                        }

                        DirectoryInfo dinfo = new DirectoryInfo(item.Tag.ToString());
                        FileInfo[] Files = dinfo.GetFiles("*.png");


                        foreach (FileInfo file in Files)
                        {

                            TreeViewItem subitem = new TreeViewItem();
                            string s = file.FullName;
                            subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                            subitem.Tag = s;
                            subitem.FontWeight = FontWeights.Normal;

                            subitem.AddHandler(PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(MouseClickedOnFileName));

                            item.Items.Add(subitem);
                        }

                    }
                    catch (Exception) { }
                }
            }

        }

        private void MouseClickedOnFileName(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("User clicked on image");
            if (sender.GetType() == typeof(TreeViewItem))
            {
                TreeViewItem senderTreeViewItem = (TreeViewItem)sender;
                lastHierarchyImageClicked = senderTreeViewItem;

            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (lastHierarchyImageClicked != null)
            {
                if (!tileList.Any())
                {
                    Tile t = CreateTileFromTreeViewItem(true);


                    tileList.Add(t);
                    ImageViewPanel.Children.Add(t.Image);

                    TextBlock textBlock = CreateTextBlockFromImageName(true, lastHierarchyImageClicked.Header as string);
                    TextPanel.Children.Add(textBlock);



                    Console.WriteLine(tileList[0].Name);
                }
                else
                {
                    bool tileIsInList = false;

                    foreach (Tile t in tileList)
                    {

                        Console.WriteLine(t.Path);
                        Console.WriteLine(lastHierarchyImageClicked.Tag as string);
                        if (t.Path == lastHierarchyImageClicked.Tag as string)
                        {
                            tileIsInList = true;
                            break;
                        }
                    }
                    if (!tileIsInList)
                    {
                        Tile t = CreateTileFromTreeViewItem(false);
                        tileList.Add(t);
                        ImageViewPanel.Children.Add(t.Image);


                        TextBlock textBlock = CreateTextBlockFromImageName(true, lastHierarchyImageClicked.Header as string);
                        TextPanel.Children.Add(textBlock);
                    }
                }
            }
        }

        private Tile CreateTileFromTreeViewItem(bool isLeftmostImage)
        {
            BitmapImage imageBitmap = new BitmapImage(new Uri(lastHierarchyImageClicked.Tag as string, UriKind.Absolute));
            Tile t = new Tile(lastHierarchyImageClicked.Header as string, lastHierarchyImageClicked.Tag as string, new Image());
            t.Image.Source = imageBitmap;

            Console.WriteLine("Ye");
            t.Image.Height = 70;
            t.Image.Width = 70;
            t.Image.AddHandler(PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(MouseClickedOnCollectionImage));

            if (isLeftmostImage)
            {
                t.Image.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                t.Image.Margin = new Thickness(5, 0, 0, 0);
            }

            return t;
        }

        private TextBlock CreateTextBlockFromImageName(bool isLeftmostText, string imageName)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = imageName;
            textBlock.Height = 25;
            textBlock.Width = 75;

            textBlock.Padding = new Thickness(10, 0, 0, 0);

            if (isLeftmostText)
            {
                textBlock.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                textBlock.Margin = new Thickness(5, 0, 0, 0);
            }

            return textBlock;

        }

        private void MouseClickedOnCollectionImage(object sender, RoutedEventArgs e)
        {
            PreviewImage.RenderTransform = new RotateTransform(0);

            PreviewImage.Source = (sender as Image).Source;

            PreviewImage.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseClickedOnPreviewImage));
            PreviewImage.AddHandler(PreviewMouseMoveEvent, new MouseEventHandler(MouseDraggingPreviewImage));


            foreach (Tile t in tileList)
            {
                if (PreviewImage.Source.Equals(t.Image.Source))
                {
                    currentPreviewTile = t;
                }
            }


        }

        private void MouseClickedOnPreviewImage(object sender, MouseButtonEventArgs e)
        {
            mouseDragStartingPosition = e.GetPosition(null);
        }

        private void MouseDraggingPreviewImage(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = mouseDragStartingPosition - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                Image img = sender as Image;

                DataObject dragData = new DataObject("image", img);


                DragDrop.DoDragDrop(img, dragData, DragDropEffects.Move);

                /*
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);

                // Find the data behind the ListViewItem
                Contact contact = (Contact)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", contact);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            */
            }
        }


        private void DragEnteringTile(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("image") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }

            Console.WriteLine("TileHovering");
            (sender as Image).Opacity = 0.5;
        }

        private void DragLeavingTile(object sender, DragEventArgs e)
        {
            (sender as Image).Opacity = 1.0;
        }

        private void DroppingOnTile(object sender, DragEventArgs e)
        {

            Point imageLocation = (sender as Image).TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
            Console.WriteLine(imageLocation.ToString());

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (imageLocation == new Point(fields[y, x].PositionX, fields[y, x].PositionY))
                    {
                        Console.WriteLine("Hmm?");

                        BitmapImage imageBitmap = new BitmapImage(new Uri((e.Data.GetData("image") as Image).Source.ToString()));

                        fields[x, y].Image.Source = imageBitmap;
                        //fields[x, y].Image.Source = (e.Data.GetData("image") as Image).Source;

                        float RenderTransformOriginX = float.Parse((e.Data.GetData("image") as Image).RenderTransformOrigin.ToString().Substring(0, (e.Data.GetData("image") as Image).RenderTransformOrigin.ToString().LastIndexOf(",")));
                        float renderTransformOriginY = float.Parse((e.Data.GetData("image") as Image).RenderTransformOrigin.ToString().Substring((e.Data.GetData("image") as Image).RenderTransformOrigin.ToString().LastIndexOf(",") + 1));

                        Point renderTransformOrigin = new Point(RenderTransformOriginX, renderTransformOriginY);
                        fields[x, y].Image.RenderTransformOrigin = renderTransformOrigin;


                        RotateTransform rotateTransform = (e.Data.GetData("image") as Image).RenderTransform as RotateTransform;

                        double angle;

                        if (rotateTransform == null)
                        {
                            angle = 0;
                        }
                        else
                        {
                            angle = double.Parse(rotateTransform.Angle.ToString());
                        }
                        

                        fields[x, y].Image.RenderTransform = new RotateTransform(angle);
                        
                        break;
                    }
                }
            }

            //(sender as Image).Source = (e.Data.GetData("image") as Image).Source;
            //(sender as Image).RenderTransformOrigin = (e.Data.GetData("image") as Image).RenderTransformOrigin;
            //(sender as Image).RenderTransform = (e.Data.GetData("image") as Image).RenderTransform;

        }

        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {

            if (PreviewImage.Source != null)
            {
                PreviewImage.RenderTransformOrigin = new Point(0.5, 0.5);

                if (currentPreviewImg == null)
                {
                    Console.WriteLine("null");
                    currentPreviewImg = new Image();
                    currentPreviewImg.Source = PreviewImage.Source;
                    rotate = new RotateTransform(-90);
                }
                else
                {

                    Console.WriteLine(currentPreviewImg.Source.ToString() + " - " + PreviewImage.Source.ToString());
                    if (currentPreviewImg.Source != PreviewImage.Source)
                    {
                        Console.WriteLine("Check");
                        rotate.Angle = -90;
                        //currentPreviewImg = new Image();
                        currentPreviewImg.Source = PreviewImage.Source;
                    }
                    else
                    {
                        Console.WriteLine("What?");
                        rotate.Angle -= 90;
                    }
                }

                PreviewImage.RenderTransform = rotate;

            }
        }

        private double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private double RadiansToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

		private void button1_Click(object sender, RoutedEventArgs e)
		{

		}

	}
}