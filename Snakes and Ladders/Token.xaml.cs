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
    /// Interaction logic for Token.xaml
    /// </summary>
    public partial class Token : UserControl, INotifyPropertyChanged
    {
        private Collection<PropertyChangedEventHandler> _Handlers = new Collection<PropertyChangedEventHandler>();
        private double _mainCircleDiameter = 5;
        private double _ringStrokeThickness = 2;
        private Point _centerPoint = new Point(0, 0);
        private double _canvasWidth = 500;
        private double _canvasHeight = 500;
        private Ellipse _outerRing;
        private Ellipse _innerRing;
        private Brush _color;
        private Brush _RingColor;
        private double durationfactor = 120.0;
        private DoubleAnimation tokenAnimation;
        private Window _parentWindow;
        private int _currentPos = 0;

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
        public event EventHandler<TokenEventArgs> MoveToken;

        #region Properties

        public int CurrentPosition
        {
            get
            {
                return _currentPos;
            }
            set
            {
                _currentPos = value;
            }
        }

        public Brush Color
        {
            get
            {
                return _color;
            }
        }

        public Brush RingColor
        {
            get
            {
                return _RingColor;
            }
        }

        public Ellipse InnerRing
        {
            get
            {
                return _innerRing;
            }
        }

        public Ellipse OuterRing
        {
            get
            {
                return _outerRing;
            }
        }
        public double MainCircleWidth
        {
            get { return _mainCircleDiameter; }
            set
            {
                _mainCircleDiameter = value;
                OnPropertyChanged("MainCircleWidth");
                OnPropertyChanged("OuterRingWidth");
                OnPropertyChanged("InnerRingWidth");
                OnPropertyChanged("MainCircleHeight");
                OnPropertyChanged("OuterRingHeight");
                OnPropertyChanged("InnerRingHeight");
                OnPropertyChanged("MainCircleTop");
                OnPropertyChanged("MainCircleLeft");
                OnPropertyChanged("InnerRingTop");
                OnPropertyChanged("InnerRingLeft");
                OnPropertyChanged("OuterRingTop");
                OnPropertyChanged("OuterRingLeft");
            }
        }

        public double MainCircleHeight
        {
            get { return _mainCircleDiameter; }
            set
            {
                _mainCircleDiameter = value;
                OnPropertyChanged("MainCircleHeight");
                OnPropertyChanged("OuterRingHeight");
                OnPropertyChanged("InnerRingHeight");
                OnPropertyChanged("MainCircleWidth");
                OnPropertyChanged("OuterRingWidth");
                OnPropertyChanged("InnerRingWidth");
                OnPropertyChanged("MainCircleTop");
                OnPropertyChanged("MainCircleLeft");
                OnPropertyChanged("InnerRingTop");
                OnPropertyChanged("InnerRingLeft");
                OnPropertyChanged("OuterRingTop");
                OnPropertyChanged("OuterRingLeft");
            }
        }

        public double OuterRingHeight
        {
            get { return _mainCircleDiameter * (2.0 / 3.0); }
        }

        public double OuterRingWidth
        {
            get { return _mainCircleDiameter * (2.0 / 3.0); }
        }

        public double InnerRingHeight
        {
            get { return _mainCircleDiameter * (1.0 / 3.0); }
        }

        public double InnerRingWidth
        {
            get { return _mainCircleDiameter * (1.0 / 3.0); }
        }

        public double RingStrokeThickness
        {
            get { return _ringStrokeThickness; }
            set
            {
                _ringStrokeThickness = value;
                OnPropertyChanged("RingStrokeThickness");
            }
        }

        public double EffectBlurRadius
        {
            get
            {
                return _ringStrokeThickness*2;
            }
        }


        public double MainCircleLeft
        {
            get
            {
                return _centerPoint.X - (_mainCircleDiameter / 2.0);
            }
        }

        public double MainCircleTop
        {
            get
            {
                return _centerPoint.Y - (_mainCircleDiameter / 2.0);
            }
        }

        public double OuterRingLeft
        {
            get
            {
                return _centerPoint.X - (OuterRingWidth / 2.0);
            }
        }

        public double OuterRingTop
        {
            get
            {
                return _centerPoint.Y - (OuterRingHeight / 2.0);
            }
        }

        public double InnerRingLeft
        {
            get
            {
                return _centerPoint.X - (InnerRingWidth / 2.0);
            }
        }

        public double InnerRingTop
        {
            get
            {
                return _centerPoint.Y - (InnerRingHeight / 2.0);
            }
        }

        public Point CenterPoint
        {
            get { return _centerPoint; }
            set
            {
                _centerPoint = value;
            }
        }

        public double CanvasWidth
        {
            get { return _canvasWidth; }
            set { _canvasWidth = value; }
        }

        public double CanvasHeight
        {
            get { return _canvasHeight; }
            set { _canvasHeight = value; }
        }
        #endregion

        public Token(object parentWindow, enGameToken engameToken, Point pos, double canvaswidth, double canvasheight)
        {
            InitializeComponent();
            DataContext = this;
            _parentWindow = parentWindow as Window;
            tokenAnimation = new DoubleAnimation();
            tokenAnimation.FillBehavior = FillBehavior.Stop;
            //tokenAnimation.Completed += Animation_Completed;
            switch (engameToken)
            {
                case enGameToken.Blue:
                    _color = Brushes.Blue;
                    _RingColor = Brushes.DarkBlue;
                    break;
                case enGameToken.Red:
                    _color = Brushes.Red;
                    _RingColor = Brushes.DarkRed;
                    break;
                case enGameToken.Green:
                    _color = Brushes.Green;
                    _RingColor = Brushes.ForestGreen;
                    break;
                case enGameToken.Yellow:
                    _color = Brushes.Yellow;
                    _RingColor = Brushes.Goldenrod;
                    break;
            }
            _centerPoint = pos;

            _outerRing = new Ellipse();
            _outerRing.DataContext = this;
            _outerRing.Stretch = Stretch.Uniform;
            _outerRing.Fill = Brushes.Transparent;
            _outerRing.SetBinding(Ellipse.StrokeProperty, "RingColor");
            _outerRing.SetBinding(Canvas.LeftProperty, "OuterRingLeft");
            _outerRing.SetBinding(Canvas.TopProperty, "OuterRingTop");
            _outerRing.SetBinding(Ellipse.HeightProperty, "OuterRingHeight");
            _outerRing.SetBinding(Ellipse.WidthProperty, "OuterRingWidth");
            _outerRing.SetBinding(Ellipse.StrokeThicknessProperty, "RingStrokeThickness");
            _outerRing.SetValue(Ellipse.NameProperty, "OuterRing");
            DropShadowEffect effect = new System.Windows.Media.Effects.DropShadowEffect();
            effect.BlurRadius = EffectBlurRadius;
            effect.Direction = 3;
            effect.ShadowDepth = 0;
            _outerRing.Effect = effect;
            _innerRing = new Ellipse();
            _innerRing.DataContext = this;
            _innerRing.Stretch = Stretch.Uniform;
            _innerRing.Fill = Brushes.Transparent;
            _innerRing.SetBinding(Ellipse.StrokeProperty, "RingColor");
            _innerRing.SetBinding(Canvas.LeftProperty, "InnerRingLeft");
            _innerRing.SetBinding(Canvas.TopProperty, "InnerRingTop");
            _innerRing.SetBinding(Ellipse.HeightProperty, "InnerRingHeight");
            _innerRing.SetBinding(Ellipse.WidthProperty, "InnerRingWidth");
            _innerRing.SetBinding(Ellipse.StrokeThicknessProperty, "RingStrokeThickness");
            _innerRing.SetValue(Ellipse.NameProperty, "InnerRing");
            _innerRing.Effect = effect;

            _canvasHeight = canvasheight;
            _canvasWidth = canvaswidth;


            MainCircleWidth = canvaswidth / 18.0;
            RingStrokeThickness = canvaswidth / 280.0;

            OnPropertyChanged("Color");
            OnPropertyChanged("RingColor");
        }

        protected virtual void OnMoveToken(object obj, TokenEventArgs e)
        {
            EventHandler<TokenEventArgs> handler = MoveToken;
            handler?.Invoke(obj, e);
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

        public void ResizeToken(double width, double height)
        {
            _centerPoint.X = (_centerPoint.X / _canvasWidth) * width;
            _centerPoint.Y = (_centerPoint.Y / _canvasHeight) * height;
            MainCircleWidth = width / 18.0;
            RingStrokeThickness = width / 280.0;
            _canvasHeight = height;
            _canvasWidth = width;
            (_outerRing.Effect as DropShadowEffect).BlurRadius = EffectBlurRadius;

        }

        /*public void MoveTo(Point newPos)
        {
            Storyboard story = new Storyboard();
            DoubleAnimation dbWidth = new DoubleAnimation();
            dbWidth.From = mediaElement1.Width;
            dbWidth.To = 600;
            dbWidth.Duration = new Duration(TimeSpan.FromSeconds(.25));

            DoubleAnimation dbHeight = new DoubleAnimation();
            dbHeight.From = mediaElement1.Height;
            dbHeight.To = 400;
            dbHeight.Duration = dbWidth.Duration;

            story.Children.Add(dbWidth);
            Storyboard.SetTargetName(dbWidth, mediaElement1.Name);
            Storyboard.SetTargetProperty(dbWidth, new PropertyPath(MediaElement.WidthProperty));

            story.Children.Add(dbHeight);
            Storyboard.SetTargetName(dbHeight, mediaElement1.Name);
            Storyboard.SetTargetProperty(dbHeight, new PropertyPath(MediaElement.HeightProperty));

            DoubleAnimation dbCanvasX = new DoubleAnimation();
            dbCanvasX.From = 0;
            dbCanvasX.To = 5;
            dbCanvasX.Duration = new Duration(TimeSpan.FromSeconds(.25));

            DoubleAnimation dbCanvasY = new DoubleAnimation();
            dbCanvasY.From = 0;
            dbCanvasY.To = 5;
            dbCanvasY.Duration = dbCanvasX.Duration;

            story.Children.Add(dbCanvasX);
            Storyboard.SetTargetName(dbCanvasX, mediaElement1.Name);
            Storyboard.SetTargetProperty(dbCanvasX, new PropertyPath(Canvas.LeftProperty));

            story.Children.Add(dbCanvasY);
            Storyboard.SetTargetName(dbCanvasY, mediaElement1.Name);
            Storyboard.SetTargetProperty(dbCanvasY, new PropertyPath(Canvas.TopProperty));

            story.Begin(this);
        }*/

        public void MoveToAlongPath(Point newPos, ref Path snakePath)
        {
            if (_parentWindow != null)
                _parentWindow.ResizeMode = ResizeMode.NoResize;

            double prevMainCircleTop = MainCircleTop, prevMainCircleLeft = MainCircleLeft;
            _centerPoint.X = newPos.X;
            _centerPoint.Y = newPos.Y;
            double duration = Math.Max(Math.Abs(MainCircleTop - prevMainCircleTop) / durationfactor, Math.Abs(MainCircleLeft - prevMainCircleLeft) / durationfactor);



            DoubleAnimationUsingPath anim = new DoubleAnimationUsingPath();
            anim.PathGeometry = snakePath.Data as PathGeometry;
            anim.Duration = TimeSpan.FromSeconds(duration > 0.0 ? duration : 0.1);
            anim.Source = PathAnimationSource.X;
            anim.FillBehavior = FillBehavior.Stop;
            this.BeginAnimation(Canvas.LeftProperty, anim);
            
            anim.Source = PathAnimationSource.Y;
            this.BeginAnimation(Canvas.TopProperty, anim);

            double marginVal = (_mainCircleDiameter - (_mainCircleDiameter * (2.0 / 3.0)))/2;
            Path outerRingPath = snakePath.Clone(marginVal, marginVal);
            anim.PathGeometry = outerRingPath.Data as PathGeometry;
            anim.Source = PathAnimationSource.X;
            _outerRing.BeginAnimation(Canvas.LeftProperty, anim);
            anim.Source = PathAnimationSource.Y;
            _outerRing.BeginAnimation(Canvas.TopProperty, anim);

            marginVal = (_mainCircleDiameter - (_mainCircleDiameter * (1.0 / 3.0)))/2;
            Path innerRingPath = snakePath.Clone(marginVal, marginVal);
            anim.PathGeometry = innerRingPath.Data as PathGeometry;
            anim.Source = PathAnimationSource.X;
            _innerRing.BeginAnimation(Canvas.LeftProperty, anim);
            anim.Source = PathAnimationSource.Y;
            _innerRing.BeginAnimation(Canvas.TopProperty, anim);

            DropShadowEffect shadowEffect = new DropShadowEffect();
            shadowEffect.ShadowDepth = 0;
            shadowEffect.Color = System.Windows.Media.Colors.OrangeRed;// (System.Windows.Media.Color)(_color.GetValue(SolidColorBrush.ColorProperty));
            shadowEffect.BlurRadius = MainCircleWidth;
            MainCircle.Effect = shadowEffect;

            DoubleAnimation shadowAnimation = new DoubleAnimation();
            shadowAnimation.From = MainCircleWidth;
            shadowAnimation.To = 0;
            shadowAnimation.Duration = TimeSpan.FromSeconds(duration / 4.0 > 0.0 ? duration / 4.0 : 0.1);
            shadowAnimation.RepeatBehavior = new RepeatBehavior(2);
            shadowAnimation.AutoReverse = true;
            shadowAnimation.Completed += Animation_Completed;
            shadowAnimation.FillBehavior = FillBehavior.Stop;
            shadowEffect.BeginAnimation(DropShadowEffect.BlurRadiusProperty, shadowAnimation);
        }

        public void MoveTo(double newX, double newY)
        {
            if (_parentWindow != null)
                _parentWindow.ResizeMode = ResizeMode.NoResize;

            double prevMainCircleTop = MainCircleTop, prevMainCircleLeft = MainCircleLeft, prevOuterRingTop = OuterRingTop, prevOuterRingLeft = OuterRingLeft, prevInnerRingTop = InnerRingTop, prevInnerRingLeft = InnerRingLeft;

            _centerPoint.X = newX;
            _centerPoint.Y = newY;

            double duration = Math.Max(Math.Abs(MainCircleTop - prevMainCircleTop) / durationfactor, Math.Abs(MainCircleLeft - prevMainCircleLeft) / durationfactor);

            tokenAnimation.Duration = TimeSpan.FromSeconds(duration > 0.0 ? duration : 0.1);

            tokenAnimation.From = prevMainCircleTop;
            tokenAnimation.To = MainCircleTop;
            this.BeginAnimation(Canvas.TopProperty, tokenAnimation);

            tokenAnimation.From = prevMainCircleLeft;
            tokenAnimation.To = MainCircleLeft;
            this.BeginAnimation(Canvas.LeftProperty, tokenAnimation);

            tokenAnimation.From = prevOuterRingTop;
            tokenAnimation.To = OuterRingTop;
            _outerRing.BeginAnimation(Canvas.TopProperty, tokenAnimation);

            tokenAnimation.From = prevOuterRingLeft;
            tokenAnimation.To = OuterRingLeft;
            _outerRing.BeginAnimation(Canvas.LeftProperty, tokenAnimation);

            tokenAnimation.From = prevInnerRingTop;
            tokenAnimation.To = InnerRingTop;
            _innerRing.BeginAnimation(Canvas.TopProperty, tokenAnimation);

            tokenAnimation.From = prevInnerRingLeft;
            tokenAnimation.To = InnerRingLeft;
            _innerRing.BeginAnimation(Canvas.LeftProperty, tokenAnimation);

            DropShadowEffect shadowEffect = new DropShadowEffect();
            shadowEffect.ShadowDepth = 0;
            shadowEffect.Color = System.Windows.Media.Colors.LightGoldenrodYellow;// (System.Windows.Media.Color)(_color.GetValue(SolidColorBrush.ColorProperty));
            shadowEffect.BlurRadius = MainCircleWidth;
            MainCircle.Effect = shadowEffect;

            DoubleAnimation shadowAnimation = new DoubleAnimation();
            shadowAnimation.From = MainCircleWidth;
            shadowAnimation.To = 0;
            shadowAnimation.Duration = TimeSpan.FromSeconds(duration / 4.0 > 0.0 ? duration / 4.0 : 0.1);
            shadowAnimation.RepeatBehavior = new RepeatBehavior(2);
            shadowAnimation.AutoReverse = true;
            shadowAnimation.Completed += Animation_Completed;
            shadowAnimation.FillBehavior = FillBehavior.Stop;
            shadowEffect.BeginAnimation(DropShadowEffect.BlurRadiusProperty, shadowAnimation);
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            (MainCircle.Effect as DropShadowEffect).BeginAnimation(DropShadowEffect.BlurRadiusProperty, null);
            MainCircle.Effect = null;
            if (_parentWindow != null)
                _parentWindow.ResizeMode = ResizeMode.CanResize;

            this.BeginAnimation(Canvas.LeftProperty, null);
            this.BeginAnimation(Canvas.TopProperty, null);
            _outerRing.BeginAnimation(Canvas.LeftProperty, null);
            _outerRing.BeginAnimation(Canvas.TopProperty, null);
            _innerRing.BeginAnimation(Canvas.LeftProperty, null);
            _innerRing.BeginAnimation(Canvas.TopProperty, null);

            OnPropertyChanged("MainCircleWidth");
            OnPropertyChanged("OuterRingWidth");
            OnPropertyChanged("InnerRingWidth");
            OnPropertyChanged("MainCircleHeight");
            OnPropertyChanged("OuterRingHeight");
            OnPropertyChanged("InnerRingHeight");
            OnPropertyChanged("MainCircleTop");
            OnPropertyChanged("MainCircleLeft");
            OnPropertyChanged("InnerRingTop");
            OnPropertyChanged("InnerRingLeft");
            OnPropertyChanged("OuterRingTop");
            OnPropertyChanged("OuterRingLeft");

            (_parentWindow as MainWindow).OnTokenAnimationComplete();
        }
    }
}
