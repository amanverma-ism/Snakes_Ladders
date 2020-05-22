using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for Box.xaml
    /// </summary>
    public partial class Box : UserControl, INotifyPropertyChanged
    {
        private Collection<PropertyChangedEventHandler> _Handlers = new Collection<PropertyChangedEventHandler>();
        private double _TextFontSize = 10;
        private double _BorderThickness = 3;
        public Box()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _Handlers.Add(value);
            }
            remove
            {
                _Handlers.Remove(value);
            }
        }

        public TextBlock BoxTextBlock
        {
            get
            {
                return boxTextBlock;
            }
        }

        public double TextFontSize
        {
            get
            {
                return _TextFontSize;
            }
            set
            {
                _TextFontSize = value;
                OnPropertyChanged("TextFontSize");
            }
        }

        public double BoxBorderThickness
        {
            get
            {
                return _BorderThickness;
            }
            set
            {
                _BorderThickness = value;
                OnPropertyChanged("BorderThickness");
            }
        }

        /// <summary>
        /// PropertyChanged handler to send call to all the subscribers.
        /// </summary>
        /// <param name="_strProperty">PropertyName to be included in PropertyChangedEventArgs</param>
        public void OnPropertyChanged(string _strProperty)
        {
            if (_Handlers != null && _Handlers.Count != 0)
            {
                for (int i = 0; i < _Handlers.Count; i++)
                {
                    _Handlers[i].Invoke(this, new PropertyChangedEventArgs(_strProperty));
                }
            }
        }

        public void OnSizeChanged(double width, double height)
        {
            this.Height = height;
            this.Width = width;
            double constant1 = Math.Min(width, height);
            TextFontSize = constant1 / 4;
            BoxBorderThickness = constant1 / 12.5;
        }

    }
}
