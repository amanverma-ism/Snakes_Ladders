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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Snakes_and_Ladders
{

    /// <summary>
    /// Interaction logic for GameBoardSettings.xaml
    /// </summary>
    public partial class GameBoardSettings : Window, INotifyPropertyChanged
    {
        private Collection<PropertyChangedEventHandler> _Handlers = new Collection<PropertyChangedEventHandler>();
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
        private double _dbSnakeThicknessFactor = 40.0;
        private bool _bCanSnakesAndLaddersIntersect = true;
        private int _intMaxLadderLength = 60;
        private int _intMaxSnakeLength = 80;
        private Brush _LadderStartBoxColor;
        private Brush _LadderEndBoxColor;
        private Brush _SnakeTailBoxColor;
        private Brush _SnakeHeadBoxColor;
        private Brush _SnakeColor;
        private Brush _LadderColor;
        private int _intNumberOfSnakes = 5;
        private int _intNumberOfLadders = 5;
        private ObservableCollection<Brush> _SnakeBoxColors;
        private ObservableCollection<Brush> _LadderBoxColors;
        private ObservableCollection<Brush> _SnakeBodyColors;
        private ObservableCollection<Brush> _LadderBodyColors;
        public GameBoardSettings(Window owner)
        {
            DataContext = this;
            Owner = owner;
            _SnakeBoxColors = new ObservableCollection<Brush>() { Brushes.DarkRed, Brushes.Red, Brushes.PaleVioletRed, Brushes.OrangeRed, Brushes.MediumVioletRed, Brushes.IndianRed, Brushes.DarkOrange, Brushes.DeepSkyBlue, Brushes.Indigo };
            _SnakeBodyColors = new ObservableCollection<Brush>() { Brushes.DarkRed, Brushes.Red, Brushes.SkyBlue, Brushes.OrangeRed, Brushes.DarkGreen, Brushes.Black, Brushes.DarkMagenta, Brushes.DeepPink, Brushes.DarkGray };
            _LadderBoxColors = new ObservableCollection<Brush>() { Brushes.DarkGreen, Brushes.Purple, Brushes.DarkSeaGreen, Brushes.ForestGreen, Brushes.Green, Brushes.Blue, Brushes.LawnGreen, Brushes.LightGreen, Brushes.RosyBrown, Brushes.Moccasin, Brushes.MediumSpringGreen, Brushes.OldLace, Brushes.DarkSalmon, Brushes.Gold, Brushes.Teal };
            _LadderBodyColors = new ObservableCollection<Brush>() { Brushes.Cyan, Brushes.Blue, Brushes.DarkBlue, Brushes.Purple, Brushes.White, Brushes.HotPink, Brushes.Teal, Brushes.Navy, Brushes.Black };
            MainWindow window = Owner as MainWindow;

            _intNumberOfSnakes = window.NumberOfSnakes;
            _intNumberOfLadders = window.NumberOfLadders;
            _SnakeTailBoxColor = window.SnakeTailBoxColor;
            _SnakeHeadBoxColor = window.SnakeHeadBoxColor;
            _LadderStartBoxColor = window.LadderStartBoxColor;
            _LadderEndBoxColor = window.LadderEndBoxColor;
            _SnakeColor = window.SnakeColor;
            _LadderColor = window.LadderColor;
            _bCanSnakesAndLaddersIntersect = window.CanSnakeLadderIntersect;
            _intMaxLadderLength = window.MaxLadderLength;
            _intMaxSnakeLength = window.MaxSnakeLength;
            _dbSnakeThicknessFactor = window.SnakeThicknessFactor;

            InitializeComponent();
        }

        #region Properties for GameBoardSettings

        public int NumberOfSnakes
        {
            get
            {
                return _intNumberOfSnakes;
            }
            set
            {
                _intNumberOfSnakes = value;
            }
        }

        public int NumberOfLadders
        {
            get
            {
                return _intNumberOfLadders;
            }
            set
            {
                _intNumberOfLadders = value;
            }
        }

        public Brush SnakeTailBoxColor
        {
            get
            { return _SnakeTailBoxColor; }
            set
            {
                _SnakeTailBoxColor = value;
            }
        }

        public Brush SnakeHeadBoxColor
        {
            get
            { return _SnakeHeadBoxColor; }
            set
            {
                _SnakeHeadBoxColor = value;
            }
        }

        public Brush LadderStartBoxColor
        {
            get
            { return _LadderStartBoxColor; }
            set
            {
                _LadderStartBoxColor = value;
            }
        }

        public Brush LadderEndBoxColor
        {
            get
            { return _LadderEndBoxColor; }
            set
            {
                _LadderEndBoxColor = value;
            }
        }

        public Brush SnakeColor
        {
            get
            { return _SnakeColor; }
            set
            {
                _SnakeColor = value;
            }
        }

        public Brush LadderColor
        {
            get
            { return _LadderColor; }
            set
            {
                _LadderColor = value;
            }
        }

        public bool CanSnakeLadderIntersect
        {
            get
            {
                return _bCanSnakesAndLaddersIntersect;
            }
            set
            {
                _bCanSnakesAndLaddersIntersect = value;
                OnPropertyChanged("SnakeLadderIntersection");
            }
        }
        public string SnakeLadderIntersection
        {
            get
            {
                if (_bCanSnakesAndLaddersIntersect == true)
                    return "Yes";
                else return "No";
            }
        }

        public int MaxLadderLength
        {
            get
            {
                return _intMaxLadderLength;
            }
            set
            {
                _intMaxLadderLength = value;
            }
        }

        public int MaxSnakeLength
        {
            get
            {
                return _intMaxSnakeLength;
            }
            set
            {
                _intMaxSnakeLength = value;
            }
        }
        public double SnakeThicknessFactor
        {
            get { return _dbSnakeThicknessFactor; }
            set
            {
                _dbSnakeThicknessFactor = value;
            }
        }

        public ObservableCollection<Brush> SnakeBoxColors
        {
            get { return _SnakeBoxColors; }
        }

        public ObservableCollection<Brush> LadderBoxColors
        {
            get { return _LadderBoxColors; }
        }

        public ObservableCollection<Brush> SnakeBodyColors
        {
            get { return _SnakeBodyColors; }
        }

        public ObservableCollection<Brush> LadderBodyColors
        {
            get { return _LadderBodyColors; }
        }

        #endregion

        #region Properties

        public double WindowHeight
        {
            get
            {
                return 400.0 + (Owner.ActualHeight - 400.0) * 0.375;
            }
        }

        public double WindowWidth
        {
            get
            {
                return 650.0 + (Owner.ActualWidth - 650.0) * 0.6667;
            }
        }
        public double Column1Width
        {
            get
            {
                return this.ActualWidth * 0.4;
            }
        }

        public double Column2Width
        {
            get
            {
                return this.ActualWidth * 0.6;
            }
        }

        public double RowHeight
        {
            get
            {
                return this.ActualHeight / 7.0;
            }
        }

        public double ColorBoxWidth
        {
            get
            {
                return this.ActualHeight * 0.05;
            }
        }

        public double ItemPanelMaxWidth
        {
            get
            {
                return this.ActualHeight * 0.23;
            }
        }

        public double TextFontSize
        {
            get
            {
                return this.ActualHeight * 0.0314;
            }
        }

        public double ComboBoxWidth
        {
            get
            {
                return this.ActualHeight * 0.125;
            }
        }

        public double ComboBoxHeight
        {
            get
            {
                return this.ActualHeight * 0.0625;
            }
        }

        public double ResetButtonWidth
        {
            get
            {
                return this.ActualWidth * 0.1538;
            }
        }

        public double CancelButtonWidth
        {
            get
            {
                return this.ActualWidth * 0.1154;
            }
        }
        #endregion

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

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetSettings();
            OnPropertyChanged("SnakeThicknessFactor");
            OnPropertyChanged("MaxSnakeLength");
            OnPropertyChanged("MaxLadderLength");
            OnPropertyChanged("SnakeLadderIntersection");
            OnPropertyChanged("CanSnakeLadderIntersect");
            OnPropertyChanged("SnakeColor");
            OnPropertyChanged("LadderColor");
            OnPropertyChanged("LadderEndBoxColor");
            OnPropertyChanged("LadderStartBoxColor");
            OnPropertyChanged("SnakeHeadBoxColor");
            OnPropertyChanged("SnakeTailBoxColor");
            OnPropertyChanged("NumberOfLadders");
            OnPropertyChanged("NumberOfSnakes");
        }

        public void ResetSettings()
        {
            Properties.Settings.Default.Reset();

            _SnakeTailBoxColor = Properties.Settings.Default.SnakeTailBoxColor.ConvertToWindowsBrush();
            _SnakeHeadBoxColor = Properties.Settings.Default.SnakeHeadBoxColor.ConvertToWindowsBrush();
            _LadderEndBoxColor = Properties.Settings.Default.LadderEndBoxColor.ConvertToWindowsBrush();
            _LadderStartBoxColor = Properties.Settings.Default.LadderStartBoxColor.ConvertToWindowsBrush();
            _SnakeColor = Properties.Settings.Default.SnakeColor.ConvertToWindowsBrush();
            _LadderColor = Properties.Settings.Default.LadderColor.ConvertToWindowsBrush();

            _intMaxLadderLength = Properties.Settings.Default.MaxLadderLength;
            _intMaxSnakeLength = Properties.Settings.Default.MaxSnakeLength;
            _dbSnakeThicknessFactor = Properties.Settings.Default.SnakeThicknessFactor;
            _intNumberOfSnakes = Properties.Settings.Default.NumberOfSnakes;
            _intNumberOfLadders = Properties.Settings.Default.NumberOfLadders;
            _bCanSnakesAndLaddersIntersect = Properties.Settings.Default.CanSnakesAndLaddersIntersect;
        }

        private void _tbSnakeWidth_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SnakeThicknessFactor < 30)
                    SnakeThicknessFactor = 30;
                if (SnakeThicknessFactor > 70)
                    SnakeThicknessFactor = 70;
                OnPropertyChanged("SnakeThicknessFactor");
            }
        }

        private void _tbSnakeWidth_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SnakeThicknessFactor < 20)
                SnakeThicknessFactor = 20;
            if (SnakeThicknessFactor > 70)
                SnakeThicknessFactor = 70;
            OnPropertyChanged("SnakeThicknessFactor");
        }

        private void _tbMaximumLadderLeap_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (MaxLadderLength < 50)
                    MaxLadderLength = 50;
                if (MaxLadderLength > 90)
                    MaxLadderLength = 90;
                OnPropertyChanged("MaxLadderLength");
            }
        }

        private void _tbMaximumLadderLeap_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MaxLadderLength < 50)
                MaxLadderLength = 50;
            if (MaxLadderLength > 90)
                MaxLadderLength = 90;
            OnPropertyChanged("MaxLadderLength");
        }

        private void _tbMaximumSnakeLeap_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (MaxSnakeLength < 50)
                    MaxSnakeLength = 50;
                if (MaxSnakeLength > 90)
                    MaxSnakeLength = 90;
                OnPropertyChanged("MaxSnakeLength");
            }
        }

        private void _tbMaximumSnakeLeap_LostFocus(object sender, RoutedEventArgs e)
        {
            if (MaxSnakeLength < 50)
                MaxSnakeLength = 50;
            if (MaxSnakeLength > 90)
                MaxSnakeLength = 90;
            OnPropertyChanged("MaxSnakeLength");
        }

        private void Double_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            double val = 0;
            if (double.TryParse(e.Text, out val) == false)
                e.Handled = true;
        }

        private void _tbNumberofSnakes_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (NumberOfSnakes < 4)
                    NumberOfSnakes = 4;
                if (NumberOfSnakes > 9)
                    NumberOfSnakes = 9;
                OnPropertyChanged("NumberOfSnakes");
            }
        }

        private void _tbNumberofSnakes_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NumberOfSnakes < 4)
                NumberOfSnakes = 4;
            if (NumberOfSnakes > 9)
                NumberOfSnakes = 9;
            OnPropertyChanged("NumberOfSnakes");
        }

        private void _tbNumberofLadders_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (NumberOfLadders < 4)
                    NumberOfLadders = 4;
                if (NumberOfLadders > 9)
                    NumberOfLadders = 9;
                OnPropertyChanged("NumberOfLadders");
            }
        }

        private void _tbNumberofLadders_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NumberOfLadders < 4)
                NumberOfLadders = 4;
            if (NumberOfLadders > 9)
                NumberOfLadders = 9;
            OnPropertyChanged("NumberOfLadders");
        }

        private void Int_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val = 0;
            if (int.TryParse(e.Text, out val) == false)
                e.Handled = true;
        }


        private void ApplySettings()
        {
            MainWindow window = Owner as MainWindow;
            if (window.NumberOfSnakes != NumberOfSnakes)
            {
                Properties.Settings.Default.NumberOfSnakes = NumberOfSnakes;
                window.NumberOfSnakes = NumberOfSnakes;
            }
            if (window.NumberOfLadders != NumberOfLadders)
            {
                Properties.Settings.Default.NumberOfLadders = NumberOfLadders;
                window.NumberOfLadders = NumberOfLadders;
            }

            if (window.SnakeTailBoxColor != SnakeTailBoxColor)
            {
                Properties.Settings.Default.SnakeTailBoxColor = SnakeTailBoxColor.ToString();
                window.SnakeTailBoxColor = SnakeTailBoxColor;
            }

            if (window.SnakeHeadBoxColor != SnakeHeadBoxColor)
            {
                Properties.Settings.Default.SnakeHeadBoxColor = SnakeHeadBoxColor.ToString();
                window.SnakeHeadBoxColor = SnakeHeadBoxColor;
            }

            if (window.LadderStartBoxColor != LadderStartBoxColor)
            {
                Properties.Settings.Default.LadderStartBoxColor = LadderStartBoxColor.ToString();
                window.LadderStartBoxColor = LadderStartBoxColor;
            }

            if (window.LadderEndBoxColor != LadderEndBoxColor)
            {
                Properties.Settings.Default.LadderEndBoxColor = LadderEndBoxColor.ToString();
                window.LadderEndBoxColor = LadderEndBoxColor;
            }

            if (window.SnakeColor != SnakeColor)
            {
                Properties.Settings.Default.SnakeColor = SnakeColor.ToString();
                window.SnakeColor = SnakeColor;
            }

            if (window.LadderColor != LadderColor)
            {
                Properties.Settings.Default.LadderColor = LadderColor.ToString();
                window.LadderColor = LadderColor;
            }

            if (window.CanSnakeLadderIntersect != CanSnakeLadderIntersect)
            {
                Properties.Settings.Default.CanSnakesAndLaddersIntersect = CanSnakeLadderIntersect;
                window.CanSnakeLadderIntersect = CanSnakeLadderIntersect;
            }

            if (window.MaxLadderLength != MaxLadderLength)
            {
                Properties.Settings.Default.MaxLadderLength = MaxLadderLength;
                window.MaxLadderLength = MaxLadderLength;
            }

            if (window.MaxSnakeLength != MaxSnakeLength)
            {
                Properties.Settings.Default.MaxSnakeLength = MaxSnakeLength;
                window.MaxSnakeLength = MaxSnakeLength;
            }

            if (window.SnakeThicknessFactor != SnakeThicknessFactor)
            {
                Properties.Settings.Default.SnakeThicknessFactor = SnakeThicknessFactor;
                window.SnakeThicknessFactor = SnakeThicknessFactor;
            }

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ApplySettings();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("RowHeight");
            OnPropertyChanged("Column1Width");
            OnPropertyChanged("Column2Width");
            OnPropertyChanged("ColorBoxWidth");
            OnPropertyChanged("ItemPanelMaxWidth");
            OnPropertyChanged("TextFontSize");
            OnPropertyChanged("ComboBoxWidth");
            OnPropertyChanged("ComboBoxHeight");
            OnPropertyChanged("ResetButtonWidth");
            OnPropertyChanged("CancelButtonWidth");

        }

        private DataTemplate getDataTemplate()
        {
            DataTemplate retVal = null;
            String markup = String.Empty;

            markup = "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:local=\"clr-namespace:Snakes_and_Ladders;assembly=Snakes_and_Ladders\" DataType=	\"{x:Type SolidColorBrush}\">";
            markup += "<Rectangle Width=\"";
            markup += 20.ToString();
            markup += "\" Height=\"{Binding RelativeSource={RelativeSource Mode=Self}, Path=Width}\" Fill=\"{Binding}\"/>";
            markup += "</DataTemplate>";

            retVal = (DataTemplate)XamlReader.Parse(markup);

            return retVal;
        }
        

        private DataTemplate GetColorDataTemplate()
        {
            return getDataTemplate();
            ////create the data template
            //DataTemplate colorboxtemplate = new DataTemplate();
            //colorboxtemplate.DataType = ;

            ////set up the stack panel
            //FrameworkElementFactory spFactory = new FrameworkElementFactory(typeof(StackPanel));
            //spFactory.Name = "myComboFactory";
            //spFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            ////set up the card holder textblock
            //FrameworkElementFactory cardHolder = new FrameworkElementFactory(typeof(TextBlock));
            //cardHolder.SetBinding(TextBlock.TextProperty, new Binding("BillToName"));
            //cardHolder.SetValue(TextBlock.ToolTipProperty, "Card Holder Name");
            //spFactory.AppendChild(cardHolder);

            ////set up the card number textblock
            //FrameworkElementFactory cardNumber = new FrameworkElementFactory(typeof(TextBlock));
            //cardNumber.SetBinding(TextBlock.TextProperty, new Binding("SafeNumber"));
            //cardNumber.SetValue(TextBlock.ToolTipProperty, "Credit Card Number");
            //spFactory.AppendChild(cardNumber);

            ////set up the notes textblock
            //FrameworkElementFactory notes = new FrameworkElementFactory(typeof(TextBlock));
            //notes.SetBinding(TextBlock.TextProperty, new Binding("Notes"));
            //notes.SetValue(TextBlock.ToolTipProperty, "Notes");
            //spFactory.AppendChild(notes);

            ////set the visual tree of the data template
            //cardLayout.VisualTree = spFactory;

            ////set the item template to be our shiny new data template
            //drpCreditCardNumberWpf.ItemTemplate = cardLayout;
        }
    }
}
