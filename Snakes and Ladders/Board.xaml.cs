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
        #region Variables
        private Dictionary<int, Box> _boxes;
        private Dictionary<Box, int> _boxesX;
        private Dictionary<Box, int> _boxesY;
        private Brush defaultBoxColor;
        #endregion

        #region Constructor
        public Board()
        {
            InitializeComponent();
            _boxes = new Dictionary<int, Box>();
            _boxesX = new Dictionary<Box, int>();
            _boxesY = new Dictionary<Box, int>();
        }
        #endregion

        #region Properties
        public Dictionary<int, Box> Boxes { get => _boxes; set => _boxes = value; }
        #endregion

        #region Methods
        /// <summary>
        /// This method is used to fill the boxes of numbers to the game board.
        /// </summary>
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

        /// <summary>
        /// This method is used to get the co-ordinates of the box containing the number with respect to the Board canvas.
        /// </summary>
        /// <param name="num">The number whose position is required.</param>
        /// <returns>Center Point of the Box containing the number.</returns>
        public Point GetCenterCoordinates(int num)
        {
            Point retPoint = new Point();
            if (num > 0 && num <= 100)
            {
                int row = _boxesX[Boxes[num]];
                int column = _boxesY[Boxes[num]];
                retPoint.Y = Boxes[1].ActualWidth * row + Boxes[1].ActualWidth / 2;
                retPoint.X = Boxes[1].ActualHeight * column + Boxes[1].ActualHeight / 2;
            }
            else
            {
                retPoint.X = 0;
                retPoint.Y = 0;
            }
            return retPoint;
        }

        /// <summary>
        /// This method is called when the size of main board changes.
        /// </summary>
        /// <param name="constant1">The changed size.</param>
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
            }
        }

        /// <summary>
        /// This method resets the color of the Game board.
        /// </summary>
        internal void Reset()
        {
            foreach (KeyValuePair<int, Box> box_itr in _boxes)
            {
                box_itr.Value.Background = defaultBoxColor;
            }
        }

        /// <summary>
        /// This method is used to set the box color of the Game Board.
        /// </summary>
        /// <param name="numbers">List of numbers to be set the color sent in argument.</param>
        /// <param name="color">The color which is to be set.</param>
        internal void SetBoxColor(IEnumerable<int> numbers, Brush color)
        {
            foreach(int number in numbers)
            {
                _boxes[number].Background = color;
            }
        }
        #endregion
    }
}
