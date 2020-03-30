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
        List<Box> _boxes;
        Dictionary<Box, int> _boxesX;
        Dictionary<Box, int> _boxesY;
        public Board()
        {
            InitializeComponent();
            _boxes = new List<Box>();
            _boxesX = new Dictionary<Box, int>();
            _boxesY = new Dictionary<Box, int>();
        }

        public List<Box> Boxes { get => _boxes; set => _boxes = value; }

        public void fillNumbers()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    double ct = (Math.Pow(-1, i+1)*j) + (100 - (i*10) - ((i%2)*9));
                    
                    Box box = new Box();
                    box.boxTextBlock.Text = ct.ToString();
                    _boxes.Add(box);
                    _boxesX.Add(box, i);
                    _boxesY.Add(box, j);
                    numberPanel.Children.Add(box);
                    box.SetValue(Grid.RowProperty, i);
                    box.SetValue(Grid.ColumnProperty, j);
                }
            }
            
        }

        public void OnSizeChanged(double constant1)
        {
            MainGrid.Height = constant1;
            MainGrid.Width = constant1;
            numberPanel.Height = constant1;
            numberPanel.Width = constant1;
            double height = constant1 / 10;
            double width = constant1 / 10;
            foreach (Box box in _boxes)
            {
                box.boxBorder.Height = box.BoxPanel.Height = box.BoxControl.Height = box.MainGrid.Height = box.Height = height;
                box.boxBorder.Width = box.BoxPanel.Width = box.BoxControl.Width = box.MainGrid.Width = box.Width = width;
                //box.BoxPanel.RenderSize = box.MainGrid.RenderSize =  box.RenderSize = new Size(width, height);
            }
        }
    }
}
