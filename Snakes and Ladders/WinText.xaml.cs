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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snakes_and_Ladders
{
    /// <summary>
    /// Interaction logic for WinText.xaml
    /// </summary>
    public partial class WinText : UserControl, INotifyPropertyChanged
    {
        private Collection<PropertyChangedEventHandler> _Handlers = new Collection<PropertyChangedEventHandler>();
        private DoubleAnimation _winAnimation;
        private DoubleAnimation shadowAnimation;
        private ColorAnimation _colorAnimation;
        private string _text;
        private double _canvasWidth;

        public WinText(enGameToken winningToken)
        {
            InitializeComponent();
            DataContext = this;
            _winAnimation = new DoubleAnimation();
            _winAnimation.FillBehavior = FillBehavior.Stop;
            _winAnimation.Duration = TimeSpan.FromSeconds(2);
            _winAnimation.AutoReverse = true;
            _winAnimation.RepeatBehavior = new RepeatBehavior(2);
            _winAnimation.From = 20;
            _winAnimation.To = 35;

            _colorAnimation = new ColorAnimation();
            _colorAnimation.FillBehavior = FillBehavior.Stop;
            _colorAnimation.Duration = TimeSpan.FromSeconds(2);
            _colorAnimation.AutoReverse = true;
            _colorAnimation.RepeatBehavior = new RepeatBehavior(2);
            _colorAnimation.From = Colors.Black;
            _colorAnimation.To = (Color)System.Windows.Media.ColorConverter.ConvertFromString(winningToken.ToString());

            DropShadowEffect shadowEffect = new DropShadowEffect();
            shadowEffect.ShadowDepth = 0;
            shadowEffect.Color = System.Windows.Media.Colors.LightGoldenrodYellow;// (System.Windows.Media.Color)(_color.GetValue(SolidColorBrush.ColorProperty));
            shadowEffect.BlurRadius = 20;
            _WinText.Effect = shadowEffect;


            shadowAnimation = new DoubleAnimation();
            shadowAnimation.From = 20;
            shadowAnimation.To = 0;
            shadowAnimation.Duration = TimeSpan.FromSeconds(2);
            shadowAnimation.RepeatBehavior = new RepeatBehavior(2);
            shadowAnimation.AutoReverse = true;
            shadowAnimation.FillBehavior = FillBehavior.Stop;
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

        public double CanvasWidth
        {
            get { return _canvasWidth; }
            set { _canvasWidth = value; }
        }
        public DoubleAnimation WinAnimation
        {
            get { return _winAnimation; }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        public void StartAnimation()
        {
            double diff = ((_canvasWidth / 10) - (_canvasWidth / 25))*2.0;
            _WinText.FontSize = _canvasWidth / 25;
            _winAnimation.From = _canvasWidth/25;
            _winAnimation.To = _canvasWidth / 15;
            _WinText.BeginAnimation(TextBlock.FontSizeProperty, _winAnimation);
            MySolidColorBrush.BeginAnimation(SolidColorBrush.ColorProperty, _colorAnimation);
            (_WinText.Effect as DropShadowEffect).BeginAnimation(DropShadowEffect.BlurRadiusProperty, shadowAnimation);

            DoubleAnimation anim = new DoubleAnimation();
            anim.FillBehavior = FillBehavior.Stop;
            anim.Duration = TimeSpan.FromSeconds(2);
            anim.AutoReverse = true;
            anim.RepeatBehavior = new RepeatBehavior(2);
            anim.From = Canvas.GetLeft(this) ;
            anim.To = Canvas.GetLeft(this) - diff;
            this.BeginAnimation(Canvas.LeftProperty, anim);

            anim.From = Canvas.GetTop(this);
            anim.To = Canvas.GetTop(this) - diff;
            this.BeginAnimation(Canvas.TopProperty, anim);

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
    }
}
