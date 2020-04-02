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

namespace Snakes_and_Ladders
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
        Dictionary<int, Box> _boxes;
        Dictionary<Box, int> _boxesX;
        Dictionary<Box, int> _boxesY;

        private Brush defaultBoxColor;
        public Board()
        {
            InitializeComponent();
            _boxes = new Dictionary<int, Box>();
            _boxesX = new Dictionary<Box, int>();
            _boxesY = new Dictionary<Box, int>();
        }

        public Dictionary<int, Box> Boxes { get => _boxes; set => _boxes = value; }

        public void fillNumbers()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    double ct = (Math.Pow(-1, i+1)*j) + (100 - (i*10) - ((i%2)*9));
                    
                    Box box = new Box();
                    box.BoxTextBlock.Text = ct.ToString();
                    _boxes.Add((int)ct, box);
                    _boxesX.Add(box, i);
                    _boxesY.Add(box, j);
                    numberPanel.Children.Add(box);
                    box.SetValue(Grid.RowProperty, i);
                    box.SetValue(Grid.ColumnProperty, j);
                }
            }
            defaultBoxColor = _boxes[1].Background;
        }

        public Point GetCenterCoordinates(int num)
        {
            Point retPoint = new Point();
            int row = _boxesX[Boxes[num]];
            int column = _boxesY[Boxes[num]];
            retPoint.Y = Boxes[1].ActualWidth * row + Boxes[1].ActualWidth / 2;
            retPoint.X = Boxes[1].ActualHeight * column + Boxes[1].ActualHeight / 2;
            return retPoint;
        }

        public void OnSizeChanged(double constant1)
        {
            MainGrid.Height = constant1;
            MainGrid.Width = constant1;
            numberPanel.Height = constant1;
            numberPanel.Width = constant1;
            double height = constant1 / 10;
            double width = constant1 / 10;
            foreach (KeyValuePair<int, Box> box_itr in _boxes)
            {
                box_itr.Value.Height = height;
                box_itr.Value.Width = width;
                box_itr.Value.TextFontSize = constant1 / 40;
                box_itr.Value.BoxBorderThickness = constant1 / 125;
                //box.BoxPanel.RenderSize = box.MainGrid.RenderSize =  box.RenderSize = new Size(width, height);
            }
        }

        internal void Reset()
        {
            foreach (KeyValuePair<int, Box> box_itr in _boxes)
            {
                box_itr.Value.Background = defaultBoxColor;
            }
        }

        internal void SetBoxColor(IEnumerable<int> numbers, Brush color)
        {
            foreach(int number in numbers)
            {
                _boxes[number].Background = color;
            }
        }
        
    }
}
