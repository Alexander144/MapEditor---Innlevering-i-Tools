using System.Windows;
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

        //Public property so the view can access it and send it to the viewmodel
        public Tile[,] TileMap
        {
            get
            {
                return _tileMap;
            }
        }

        //Private fields

        int _numberOfXNodes = 10;
        int _numberOfYNodes = 10;
        private int _columnWidth = 50;
        private int _rowHeight = 50;
        private Tile[,] _tileMap;
        private TreeViewItem _lastHierarchyImageClicked;
        private List<Tile> _tileList;
        private Tile _currentPreviewTile;
        private Image _currentPreviewImg;
        private Point _mouseDragStartingPosition;
        private RotateTransform _rotate;
        private UniformGrid _tileGrid;
        private object[] _previewTileInformation;


        //Tile related commands
        public ICommand UpdateTilesCommand
        {
            get
            {
                return new RelayCommand<System.Data.Linq.Table<CodeMvvm.Tile>>(UpdateTiles);
            }
            set
            {

            }
        }


        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            InitTiles();
            InitPreviewTileInformation();

            InitGrid();
            InitTreeView();
            CreateCommands();

        }

        public void InitTiles()
        {
            _tileMap = new Tile[_numberOfXNodes, _numberOfYNodes];
            for (int i = 0; i < _numberOfXNodes; i++)
            {
                for (int j = 0; j < _numberOfYNodes; j++)
                {
                    _tileMap[i, j] = new Tile("defaultTile.png", 20 + i * _columnWidth, 40 + j * _rowHeight);
                    _tileMap[i, j].Path = "pack://application:,,,/View/Sprites/defaultTile.png";
                }
            }
            _tileList = new List<Tile>();
        }

        public void InitPreviewTileInformation()
        {
            _previewTileInformation = new object[8] { TopLeft.Content, TopMiddle.Content, TopRight.Content, LeftMiddle.Content, RightMiddle.Content, BottomLeft.Content, BottomMiddle.Content, BottomRight.Content };
        }

        public void InitGrid()
        {
            _tileGrid = new UniformGrid();
            _tileGrid.Columns = _numberOfXNodes;
            _tileGrid.Rows = _numberOfYNodes;
            _tileGrid.Width = _columnWidth * _numberOfXNodes;
            _tileGrid.Height = _rowHeight * _numberOfYNodes;
            for (int x = 0; x < _numberOfXNodes; x++)
            {
                for (int y = 0; y < _numberOfYNodes; y++)
                {

                    Image img = new Image();
                    BitmapImage imageBitmap = new BitmapImage(new Uri(_tileMap[x, y].Path));

                    img.Source = imageBitmap;
                    img.Stretch = System.Windows.Media.Stretch.Fill;


                    img.AddHandler(MouseEnterEvent, new RoutedEventHandler(MouseHoveringOverTile));
                    img.AddHandler(MouseLeaveEvent, new RoutedEventHandler(MouseExitingTile));

                    img.AllowDrop = true;
                    img.AddHandler(DragEnterEvent, new DragEventHandler(DragEnteringTile));
                    img.AddHandler(DragLeaveEvent, new DragEventHandler(DragLeavingTile));
                    img.AddHandler(DropEvent, new DragEventHandler(DroppingOnTile));

                    _tileMap[x, y].Image = img;

                    _tileGrid.Children.Add(_tileMap[x, y].Image);
                }
            }

            Canvas.SetLeft(_tileGrid, 0);
            Canvas.SetTop(_tileGrid, 0);
            MainCanvas.Children.Add(_tileGrid);
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
                item.Expanded += new RoutedEventHandler(FolderExpanded);
                DirectoryTreeView.Items.Add(item);
            }
        }

        public void CreateCommands()
        {
            UpdateTilesCommand = new RelayCommand<System.Data.Linq.Table<CodeMvvm.Tile>>(UpdateTiles);
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

        void FolderExpanded(object sender, RoutedEventArgs e)
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
                            subitem.Expanded += new RoutedEventHandler(FolderExpanded);
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
                _lastHierarchyImageClicked = senderTreeViewItem;

            }
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_lastHierarchyImageClicked != null)
            {
                if (!_tileList.Any())
                {
                    Tile t = CreateTileFromTreeViewItem(true);


                    _tileList.Add(t);
                    ImageViewPanel.Children.Add(t.Image);

                    TextBlock textBlock = CreateTextBlockFromImageName(true, _lastHierarchyImageClicked.Header as string);
                    TextPanel.Children.Add(textBlock);



                    Console.WriteLine(_tileList[0].Name);
                }
                else
                {
                    bool tileIsInList = false;

                    foreach (Tile t in _tileList)
                    {

                        Console.WriteLine(t.Path);
                        Console.WriteLine(_lastHierarchyImageClicked.Tag as string);
                        if (t.Path == _lastHierarchyImageClicked.Tag as string)
                        {
                            tileIsInList = true;
                            break;
                        }
                    }
                    if (!tileIsInList)
                    {
                        Tile t = CreateTileFromTreeViewItem(false);
                        _tileList.Add(t);
                        ImageViewPanel.Children.Add(t.Image);


                        TextBlock textBlock = CreateTextBlockFromImageName(true, _lastHierarchyImageClicked.Header as string);
                        TextPanel.Children.Add(textBlock);
                    }
                }
            }
        }

        private void MouseClickedOnCollectionImage(object sender, RoutedEventArgs e)
        {
            PreviewImage.RenderTransform = new RotateTransform(0);

            PreviewImage.Source = (sender as Image).Source;

            PreviewImage.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseClickedOnPreviewImage));
            PreviewImage.AddHandler(PreviewMouseMoveEvent, new MouseEventHandler(MouseDraggingPreviewImage));

            foreach (Tile t in _tileList)
            {
                if (PreviewImage.Source.Equals(t.Image.Source))
                {
                    _currentPreviewTile = t;
                }
            }
        }

        private void MouseClickedOnPreviewImage(object sender, MouseButtonEventArgs e)
        {
            _mouseDragStartingPosition = e.GetPosition(null);
        }

        private void MouseDraggingPreviewImage(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = _mouseDragStartingPosition - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                Image img = sender as Image;

                DataObject dragData = new DataObject("image", img);


				DragDrop.DoDragDrop(img, dragData, DragDropEffects.Move);
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

            for (int x = 0; x < _numberOfXNodes; x++)
            {
                for (int y = 0; y < _numberOfYNodes; y++)
                {
                    if (imageLocation == new Point(_tileMap[y, x].PositionX, _tileMap[y, x].PositionY))
                    {
                        Console.WriteLine("Hmm?");

                        TileMap[x, y].Path = (e.Data.GetData("image") as Image).Source.ToString();

                        BitmapImage imageBitmap = new BitmapImage(new Uri((e.Data.GetData("image") as Image).Source.ToString()));

                        _tileMap[x, y].Image.Source = imageBitmap;

						Console.WriteLine((e.Data.GetData("image") as Image).RenderTransformOrigin.ToString());

                        //This one can just be a reference in this assignment, since we never change it and for some (stupid) reason, Point.ToString() gives different outputs on different computers (yes we had to debug that).
                        _tileMap[x, y].Image.RenderTransformOrigin = (e.Data.GetData("image") as Image).RenderTransformOrigin;

                
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
                        

                        _tileMap[x, y].Image.RenderTransform = new RotateTransform(angle);
                        _tileMap[x, y].RotationAngle = angle;

                        SetTileInfo(Int32.Parse(_previewTileInformation[0].ToString()), ref _tileMap[x, y].TopLeft);
                        SetTileInfo(Int32.Parse(_previewTileInformation[1].ToString()), ref _tileMap[x, y].TopMiddle);
                        SetTileInfo(Int32.Parse(_previewTileInformation[2].ToString()), ref _tileMap[x, y].TopRight);
                        SetTileInfo(Int32.Parse(_previewTileInformation[3].ToString()), ref _tileMap[x, y].LeftMiddle);
                        SetTileInfo(Int32.Parse(_previewTileInformation[4].ToString()), ref _tileMap[x, y].RightMiddle);
                        SetTileInfo(Int32.Parse(_previewTileInformation[5].ToString()), ref _tileMap[x, y].BottomLeft);
                        SetTileInfo(Int32.Parse(_previewTileInformation[6].ToString()), ref _tileMap[x, y].BottomMiddle);
                        SetTileInfo(Int32.Parse(_previewTileInformation[7].ToString()), ref _tileMap[x, y].BottomRight);
                        break;
                        
                    }
                }
            }

        }

        private void RotateButton_Click(object sender, RoutedEventArgs e)
        {

            if (PreviewImage.Source != null)
            {
                PreviewImage.RenderTransformOrigin = new Point(0.5, 0.5);

                if (_currentPreviewImg == null)
                {
                    Console.WriteLine("null");
                    _currentPreviewImg = new Image();
                    _currentPreviewImg.Source = PreviewImage.Source;
                    _rotate = new RotateTransform(-90);
                }
                else
                {

                    Console.WriteLine(_currentPreviewImg.Source.ToString() + " - " + PreviewImage.Source.ToString());
                    if (_currentPreviewImg.Source != PreviewImage.Source)
                    {
                        Console.WriteLine("Check");
                        _rotate.Angle = -90;
                        _currentPreviewImg.Source = PreviewImage.Source;
                    }
                    else
                    {
                        Console.WriteLine("What?");
                        _rotate.Angle -= 90;
                    }
                }

                PreviewImage.RenderTransform = _rotate;

            }
        }

        private void TileInfo_Click(object sender, RoutedEventArgs e)
        {


            if (Int32.Parse((sender as Button).Content.ToString()) == 1)
            {
                (sender as Button).Content = 0;
            }
            else
            {
                (sender as Button).Content = 1;
            }

            _previewTileInformation = new object[8] { TopLeft.Content, TopMiddle.Content, TopRight.Content, LeftMiddle.Content, RightMiddle.Content, BottomLeft.Content, BottomMiddle.Content, BottomRight.Content };
        }



        //Helpers

        private Tile CreateTileFromTreeViewItem(bool isLeftmostImage)
        {
            BitmapImage imageBitmap = new BitmapImage(new Uri(_lastHierarchyImageClicked.Tag as string, UriKind.Absolute));
            Tile t = new Tile(_lastHierarchyImageClicked.Header as string, _lastHierarchyImageClicked.Tag as string, new Image());
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

        public void SetTileInfo(int infoInt, ref bool infoBool)
        {
            if (Int32.Parse(_previewTileInformation[0].ToString()) == 1)
            {
                infoBool = true;
            }
            else
            {
                infoBool = false;
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


        //Updates the tilemap to represent the tiles in the database. Called from view.

        public void UpdateTiles(System.Data.Linq.Table<CodeMvvm.Tile> convertableTiles)
        {
            int i = 0;
            int j = 0;

            foreach (CodeMvvm.Tile t in convertableTiles)
            {

                BitmapImage imageBitmap = new BitmapImage(new Uri(t.Path));
                _tileMap[i, j].Image.RenderTransform = new RotateTransform((double)t.RotationAngle);
                _tileMap[i, j].Image.RenderTransformOrigin = new Point(0.5, 0.5);

                _tileMap[i, j].Image.Source = imageBitmap;
                _tileMap[i, j].Path = t.Path;
                _tileMap[i, j].PositionX = (int)(_columnWidth * t.PositionX + 20);
                _tileMap[i, j].PositionY = (int)(_rowHeight * t.PositionY + 40);
                _tileMap[i, j].RotationAngle = (float)t.RotationAngle;
                _tileMap[i, j].TopLeft = (bool)t.TopLeft;
                _tileMap[i, j].TopMiddle = (bool)t.TopMiddle;
                _tileMap[i, j].TopRight = (bool)t.TopRight;
                _tileMap[i, j].LeftMiddle = (bool)t.LeftMiddle;
                _tileMap[i, j].RightMiddle = (bool)t.RightMiddle;
                _tileMap[i, j].BottomLeft = (bool)t.RightMiddle;
                _tileMap[i, j].BottomMiddle = (bool)t.BottomMiddle;
                _tileMap[i, j].BottomRight = (bool)t.BottomRight;



                if (i == _numberOfXNodes - 1 && j == _numberOfYNodes - 1)
                {
                    break;
                }

                if (j < _numberOfYNodes - 1)
                {
                    j++;
                }
                else
                {
                    i++;
                    j = 0;
                }
            }
        }
    }
}