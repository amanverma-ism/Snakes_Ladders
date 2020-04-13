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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snakes_and_Ladders
{
    public enum DiceFace
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6
    }
    /// <summary>
    /// Interaction logic for Dice.xaml
    /// </summary>
    public partial class Dice : UserControl
    {
        protected Viewport3D Viewport = null;      //Viewport3D class provides a rendering surface for 3-D visual content
        protected GeometryModel3D surface1 = null, surface2 = null, surface3 = null, surface4 = null, surface5 = null, surface6 = null;     //GeometryModel3D class object provides generalized transformation support for 3-D objects
        protected PerspectiveCamera Camera = null;       //PerspectiveCamera class object represents a perspective projection camera
        protected DirectionalLight myDLight = null;      //DirectionalLight class object provides effect along a direction
        protected AmbientLight AmLight = null;       //AmbientLight class objects provides Lights every surface uniformly a bright AmbientLight creates  because of lack of shading, but a low-intensity AmbientLight approximatesthe effect of light that has been scattered by reflecting between diffuse surfaces in the scene.
        protected Transform3DGroup Transgroup = null;        //Transform3DGroup class contains a collection of Transform3Ds.

        protected MeshGeometry3D surface1Plane = null, surface2Plane = null, surface3Plane = null, surface4Plane = null, surface5Plane = null, surface6Plane = null;    // MeshGeometry3D class represents a set of 3D surfaces

        protected DiffuseMaterial surface1Material = null, surface2Material = null, surface3Material = null, surface4Material = null, surface5Material = null, surface6Material = null; //DiffuseMaterial class provides scatters light striking the surface in all directions.

        protected Model3DGroup cube = null;
        protected Model3DGroup Modelgroup = null;
        protected ModelVisual3D ModelVisualD = null; //ModelVisual3D class contains 3-D models
        protected double camerafactor = 0;
        protected DoubleAnimation myAnimation;
        public Dice()
        {
            InitializeComponent();
            myAnimation = new DoubleAnimation();
            myAnimation.From = 0;
            myAnimation.To = 360;
            myAnimation.RepeatBehavior = new RepeatBehavior(2);
            //myAnimation.RepeatBehavior = RepeatBehavior.Forever;
            myAnimation.AutoReverse = true;
            myAnimation.FillBehavior = FillBehavior.Stop;

        }

        public DoubleAnimation DiceAnimation
        {
            get { return myAnimation; }
        }
        

        public void InitializeDice()
        {
            Viewport = new Viewport3D(); //Viewport3D class provides a rendering surface for 3-D visual content
            surface1 = new GeometryModel3D();//GeometryModel3D class object provides generalized transformation support for 3-D objects
            surface2 = new GeometryModel3D();
            surface3 = new GeometryModel3D();
            surface4 = new GeometryModel3D();
            surface5 = new GeometryModel3D();
            surface6 = new GeometryModel3D();
            Camera = new PerspectiveCamera();//PerspectiveCamera class object represents a perspective projection camera

            myDLight = new DirectionalLight();//DirectionalLight class object provides effect along a direction

            AmLight = new AmbientLight();//AmbientLight class objects provides Lights every surface uniformly a bright AmbientLight creates  because of lack of shading, but a low-intensity AmbientLight approximatesthe effect of light that has been scattered by reflecting between diffuse surfaces in the scene.

            MaterialGroup myMaterials = new MaterialGroup();//MaterialGroup class objects applies multiple Materials to a model each Material is rendered in order, with the last Material in the group appearing on top.
            Transgroup = new Transform3DGroup();//Transform3DGroup class contains a collection of Transform3Ds.
            Model3DGroup Modelgroup = new Model3DGroup();//The Model3DGroup class is itself a Model3D and is often used to group multiple GeometryModel3Ds
            cube = new Model3DGroup();
            surface1Plane = new MeshGeometry3D();// MeshGeometry3D class represents a set of 3D surfaces
            surface2Plane = new MeshGeometry3D();
            surface3Plane = new MeshGeometry3D();
            surface4Plane = new MeshGeometry3D();
            surface5Plane = new MeshGeometry3D();
            surface6Plane = new MeshGeometry3D();

            ModelVisual3D ModelVisualD = new ModelVisual3D();//ModelVisual3D class contains 3-D models

            //Camera.Position = new Point3D(-5, 2, 3);//setting the camera position in world coordinates
            //Camera.LookDirection = new Vector3D(5, -2, -3);//defininig the direction in which the camera looking in world coordinates
            Camera.Position = new Point3D(0, 0, 2);//setting the camera position in world coordinates
            Camera.LookDirection = new Vector3D(0, 0, -1);//defininig the direction in which the camera looking in world coordinates
            AmLight.Color = Colors.White;//setting the color of light

            //set Geometry property of MeshGeometry3D

            surface1.Geometry = surface1Plane;
            surface2.Geometry = surface2Plane;
            surface3.Geometry = surface3Plane;
            surface4.Geometry = surface4Plane;
            surface5.Geometry = surface5Plane;
            surface6.Geometry = surface6Plane;

            cube.Children.Add(surface1);
            cube.Children.Add(surface2);
            cube.Children.Add(surface3);
            cube.Children.Add(surface4);
            cube.Children.Add(surface5);
            cube.Children.Add(surface6);

            Modelgroup.Transform = Transgroup;

            Modelgroup.Children.Add(cube);

            Modelgroup.Children.Add(AmLight);
            Modelgroup.Children.Add(myDLight);

            Viewport.Camera = Camera;

            ModelVisualD.Content = Modelgroup;

            Viewport.Children.Add(ModelVisualD);

            //Defining surface position in world coordinates
            //-----------------------surface1------------------------


            surface1Plane.Positions.Add(new Point3D(-0.5, -0.5, -0.5));
            surface1Plane.Positions.Add(new Point3D(-0.5, 0.5, -0.5));
            surface1Plane.Positions.Add(new Point3D(0.5, 0.5, -0.5));
            surface1Plane.Positions.Add(new Point3D(0.5, 0.5, -0.5));
            surface1Plane.Positions.Add(new Point3D(0.5, -0.5, -0.5));
            surface1Plane.Positions.Add(new Point3D(-0.5, -0.5, -0.5));


            //TriangleIndices—Describes the connections between the vertices to form triangles if TriangleIndices is not specified, it is implied that the positions should beconnected in the order they appear: 0 1 2, then 3 4 5, and so on.

            surface1Plane.TriangleIndices.Add(0);
            surface1Plane.TriangleIndices.Add(1);
            surface1Plane.TriangleIndices.Add(2);
            surface1Plane.TriangleIndices.Add(3);
            surface1Plane.TriangleIndices.Add(4);
            surface1Plane.TriangleIndices.Add(5);

            surface1Plane.Normals.Add(new Vector3D(0, 0, -1));
            surface1Plane.Normals.Add(new Vector3D(0, 0, -1));
            surface1Plane.Normals.Add(new Vector3D(0, 0, -1));
            surface1Plane.Normals.Add(new Vector3D(0, 0, -1));
            surface1Plane.Normals.Add(new Vector3D(0, 0, -1));
            surface1Plane.Normals.Add(new Vector3D(0, 0, -1));

            surface1Plane.TextureCoordinates.Add(new Point(1, 0));
            surface1Plane.TextureCoordinates.Add(new Point(1, 1));
            surface1Plane.TextureCoordinates.Add(new Point(0, 1));
            surface1Plane.TextureCoordinates.Add(new Point(0, 1));
            surface1Plane.TextureCoordinates.Add(new Point(0, 0));
            surface1Plane.TextureCoordinates.Add(new Point(1, 0));

            //-----------------------surface2------------------------


            surface2Plane.Positions.Add(new Point3D(-0.5, -0.5, 0.5));
            surface2Plane.Positions.Add(new Point3D(0.5, -0.5, 0.5));
            surface2Plane.Positions.Add(new Point3D(0.5, 0.5, 0.5));
            surface2Plane.Positions.Add(new Point3D(0.5, 0.5, 0.5));
            surface2Plane.Positions.Add(new Point3D(-0.5, 0.5, 0.5));
            surface2Plane.Positions.Add(new Point3D(-0.5, -0.5, 0.5));

            surface2Plane.TriangleIndices.Add(0);
            surface2Plane.TriangleIndices.Add(1);
            surface2Plane.TriangleIndices.Add(2);
            surface2Plane.TriangleIndices.Add(3);
            surface2Plane.TriangleIndices.Add(4);
            surface2Plane.TriangleIndices.Add(5);

            surface2Plane.Normals.Add(new Vector3D(0, 0, 1));
            surface2Plane.Normals.Add(new Vector3D(0, 0, 1));
            surface2Plane.Normals.Add(new Vector3D(0, 0, 1));
            surface2Plane.Normals.Add(new Vector3D(0, 0, 1));
            surface2Plane.Normals.Add(new Vector3D(0, 0, 1));
            surface2Plane.Normals.Add(new Vector3D(0, 0, 1));

            surface2Plane.TextureCoordinates.Add(new Point(0, 0));
            surface2Plane.TextureCoordinates.Add(new Point(1, 0));
            surface2Plane.TextureCoordinates.Add(new Point(1, 1));
            surface2Plane.TextureCoordinates.Add(new Point(1, 1));
            surface2Plane.TextureCoordinates.Add(new Point(0, 1));
            surface2Plane.TextureCoordinates.Add(new Point(0, 0));

            //-----------------------surface3------------------------


            surface3Plane.Positions.Add(new Point3D(-0.5, -0.5, -0.5));
            surface3Plane.Positions.Add(new Point3D(0.5, -0.5, -0.5));
            surface3Plane.Positions.Add(new Point3D(0.5, -0.5, 0.5));
            surface3Plane.Positions.Add(new Point3D(0.5, -0.5, 0.5));
            surface3Plane.Positions.Add(new Point3D(-0.5, -0.5, 0.5));
            surface3Plane.Positions.Add(new Point3D(-0.5, -0.5, -0.5));

            surface3Plane.TriangleIndices.Add(0);
            surface3Plane.TriangleIndices.Add(1);
            surface3Plane.TriangleIndices.Add(2);
            surface3Plane.TriangleIndices.Add(3);
            surface3Plane.TriangleIndices.Add(4);
            surface3Plane.TriangleIndices.Add(5);

            surface3Plane.Normals.Add(new Vector3D(0, -1, 0));
            surface3Plane.Normals.Add(new Vector3D(0, -1, 0));
            surface3Plane.Normals.Add(new Vector3D(0, -1, 0));
            surface3Plane.Normals.Add(new Vector3D(0, -1, 0));
            surface3Plane.Normals.Add(new Vector3D(0, -1, 0));
            surface3Plane.Normals.Add(new Vector3D(0, -1, 0));

            surface3Plane.TextureCoordinates.Add(new Point(0, 0));
            surface3Plane.TextureCoordinates.Add(new Point(1, 0));
            surface3Plane.TextureCoordinates.Add(new Point(1, 1));
            surface3Plane.TextureCoordinates.Add(new Point(1, 1));
            surface3Plane.TextureCoordinates.Add(new Point(0, 1));
            surface3Plane.TextureCoordinates.Add(new Point(0, 0));

            //-----------------------surface4------------------------


            surface4Plane.Positions.Add(new Point3D(0.5, -0.5, -0.5));
            surface4Plane.Positions.Add(new Point3D(0.5, 0.5, -0.5));
            surface4Plane.Positions.Add(new Point3D(0.5, 0.5, 0.5));
            surface4Plane.Positions.Add(new Point3D(0.5, 0.5, 0.5));
            surface4Plane.Positions.Add(new Point3D(0.5, -0.5, 0.5));
            surface4Plane.Positions.Add(new Point3D(0.5, -0.5, -0.5));

            surface4Plane.TriangleIndices.Add(0);
            surface4Plane.TriangleIndices.Add(1);
            surface4Plane.TriangleIndices.Add(2);
            surface4Plane.TriangleIndices.Add(3);
            surface4Plane.TriangleIndices.Add(4);
            surface4Plane.TriangleIndices.Add(5);

            surface4Plane.Normals.Add(new Vector3D(1, 0, 0));
            surface4Plane.Normals.Add(new Vector3D(1, 0, 0));
            surface4Plane.Normals.Add(new Vector3D(1, 0, 0));
            surface4Plane.Normals.Add(new Vector3D(1, 0, 0));
            surface4Plane.Normals.Add(new Vector3D(1, 0, 0));
            surface4Plane.Normals.Add(new Vector3D(1, 0, 0));

            surface4Plane.TextureCoordinates.Add(new Point(1, 0));
            surface4Plane.TextureCoordinates.Add(new Point(1, 1));
            surface4Plane.TextureCoordinates.Add(new Point(0, 1));
            surface4Plane.TextureCoordinates.Add(new Point(0, 1));
            surface4Plane.TextureCoordinates.Add(new Point(0, 0));
            surface4Plane.TextureCoordinates.Add(new Point(1, 0));

            //-----------------------surface5------------------------


            surface5Plane.Positions.Add(new Point3D(0.5, 0.5, -0.5));
            surface5Plane.Positions.Add(new Point3D(-0.5, 0.5, -0.5));
            surface5Plane.Positions.Add(new Point3D(-0.5, 0.5, 0.5));
            surface5Plane.Positions.Add(new Point3D(-0.5, 0.5, 0.5));
            surface5Plane.Positions.Add(new Point3D(0.5, 0.5, 0.5));
            surface5Plane.Positions.Add(new Point3D(0.5, 0.5, -0.5));

            surface5Plane.TriangleIndices.Add(0);
            surface5Plane.TriangleIndices.Add(1);
            surface5Plane.TriangleIndices.Add(2);
            surface5Plane.TriangleIndices.Add(3);
            surface5Plane.TriangleIndices.Add(4);
            surface5Plane.TriangleIndices.Add(5);

            surface5Plane.Normals.Add(new Vector3D(0, 1, 0));
            surface5Plane.Normals.Add(new Vector3D(0, 1, 0));
            surface5Plane.Normals.Add(new Vector3D(0, 1, 0));
            surface5Plane.Normals.Add(new Vector3D(0, 1, 0));
            surface5Plane.Normals.Add(new Vector3D(0, 1, 0));
            surface5Plane.Normals.Add(new Vector3D(0, 1, 0));

            surface5Plane.TextureCoordinates.Add(new Point(1, 1));
            surface5Plane.TextureCoordinates.Add(new Point(0, 1));
            surface5Plane.TextureCoordinates.Add(new Point(0, 0));
            surface5Plane.TextureCoordinates.Add(new Point(0, 0));
            surface5Plane.TextureCoordinates.Add(new Point(1, 0));
            surface5Plane.TextureCoordinates.Add(new Point(1, 1));

            //-----------------------surface6------------------------


            surface6Plane.Positions.Add(new Point3D(-0.5, 0.5, -0.5));
            surface6Plane.Positions.Add(new Point3D(-0.5, -0.5, -0.5));
            surface6Plane.Positions.Add(new Point3D(-0.5, -0.5, 0.5));
            surface6Plane.Positions.Add(new Point3D(-0.5, -0.5, 0.5));
            surface6Plane.Positions.Add(new Point3D(-0.5, 0.5, 0.5));
            surface6Plane.Positions.Add(new Point3D(-0.5, 0.5, -0.5));

            surface6Plane.TriangleIndices.Add(0);
            surface6Plane.TriangleIndices.Add(1);
            surface6Plane.TriangleIndices.Add(2);
            surface6Plane.TriangleIndices.Add(3);
            surface6Plane.TriangleIndices.Add(4);
            surface6Plane.TriangleIndices.Add(5);

            surface6Plane.Normals.Add(new Vector3D(-1, 0, 0));
            surface6Plane.Normals.Add(new Vector3D(-1, 0, 0));
            surface6Plane.Normals.Add(new Vector3D(-1, 0, 0));
            surface6Plane.Normals.Add(new Vector3D(-1, 0, 0));
            surface6Plane.Normals.Add(new Vector3D(-1, 0, 0));
            surface6Plane.Normals.Add(new Vector3D(-1, 0, 0));

            surface6Plane.TextureCoordinates.Add(new Point(0, 1));
            surface6Plane.TextureCoordinates.Add(new Point(0, 0));
            surface6Plane.TextureCoordinates.Add(new Point(1, 0));
            surface6Plane.TextureCoordinates.Add(new Point(1, 0));
            surface6Plane.TextureCoordinates.Add(new Point(1, 1));
            surface6Plane.TextureCoordinates.Add(new Point(0, 1));

            //Accessing ImageBrush tags from App.xaml file with their keys name
            ImageBrush imgBrush1 = new ImageBrush((ImageSource)WPFBitmapConverter.Convert(Properties.Resources.One));
            ImageBrush imgBrush2 = new ImageBrush((ImageSource)WPFBitmapConverter.Convert(Properties.Resources.Six));
            ImageBrush imgBrush3 = new ImageBrush((ImageSource)WPFBitmapConverter.Convert(Properties.Resources.Four));
            ImageBrush imgBrush4 = new ImageBrush((ImageSource)WPFBitmapConverter.Convert(Properties.Resources.Five));
            ImageBrush imgBrush5 = new ImageBrush((ImageSource)WPFBitmapConverter.Convert(Properties.Resources.Three));
            ImageBrush imgBrush6 = new ImageBrush((ImageSource)WPFBitmapConverter.Convert(Properties.Resources.Two));
            surface1Material = new DiffuseMaterial((Brush)imgBrush1);
            surface2Material = new DiffuseMaterial((Brush)imgBrush2);
            surface3Material = new DiffuseMaterial((Brush)imgBrush3);
            surface4Material = new DiffuseMaterial((Brush)imgBrush4);
            surface5Material = new DiffuseMaterial((Brush)imgBrush5);
            surface6Material = new DiffuseMaterial((Brush)imgBrush6);
            
            surface1.Material = surface1Material;
            surface2.Material = surface2Material;
            surface3.Material = surface3Material;
            surface4.Material = surface4Material;
            surface5.Material = surface5Material;
            surface6.Material = surface6Material;
            this.Content = Viewport;
        }

        public void StartAnimation(double time)
        {
            (new System.Media.SoundPlayer(Properties.Resources.DiceAudio)).Play();
            System.Threading.Thread.Sleep(200);

            Random random = new Random();

            Vector3D vecD = new Vector3D(-random.NextDouble(), -random.NextDouble(), random.NextDouble());
            Vector3D vecD2 = new Vector3D(-random.NextDouble(), random.NextDouble(), random.NextDouble());

            Rotation3D RD = new AxisAngleRotation3D(vecD, 0);
            Rotation3D RD2 = new AxisAngleRotation3D(vecD2, 0);

            RotateTransform3D RotateTrans = new RotateTransform3D(RD);//RotateTransform3D class specify rotation transformation that is parameterized
            RotateTransform3D RotateTrans2 = new RotateTransform3D(RD2);//RotateTransform3D class specify rotation transformation that is parameterized
            //Define an animation for the rotation
            myAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(time));//Defining time in second for cube that will be rotate

            RotateTrans.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, myAnimation);
            RotateTrans2.Rotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, myAnimation);
            //Add transformation to the model

            Transgroup.Children.Add(RotateTrans);
            Transgroup.Children.Add(RotateTrans2);
        }

        public void EndAnimation()
        {
            Transgroup.Children.Clear();
        }

        public void SetFace(DiceFace face, double cameraDistance = 2.2)
        {
            switch(face)
            {
                case DiceFace.One:
                    Camera.Position = new Point3D(0, 0, -cameraDistance);//setting the camera position in world coordinates
                    Camera.LookDirection = new Vector3D(0, 0, 1);//defininig the direction in which the camera looking in world coordinates
                    Camera.UpDirection = new Vector3D(0, 1, 0);
                    myDLight.Direction = new Vector3D(0, 0, 1);
                    break;
                case DiceFace.Two:
                    Camera.Position = new Point3D(-cameraDistance, 0, 0);//setting the camera position in world coordinates
                    Camera.LookDirection = new Vector3D(1, 0, 0);//defininig the direction in which the camera looking in world coordinates
                    Camera.UpDirection = new Vector3D(0, 1, 0);
                    myDLight.Direction = new Vector3D(1, 0, 0);
                    break;
                case DiceFace.Three:
                    Camera.Position = new Point3D(0, cameraDistance, 0);//setting the camera position in world coordinates
                    Camera.LookDirection = new Vector3D(0, -1, 0);//defininig the direction in which the camera looking in world coordinates
                    Camera.UpDirection = new Vector3D(-1, 0, 0);
                    myDLight.Direction = new Vector3D(0, -1, 0);
                    break;
                case DiceFace.Four:
                    Camera.Position = new Point3D(0, -cameraDistance, 0);//setting the camera position in world coordinates
                    Camera.LookDirection = new Vector3D(0, 1, 0);//defininig the direction in which the camera looking in world coordinates
                    Camera.UpDirection = new Vector3D(1, 0, 0);
                    myDLight.Direction = new Vector3D(0, 1, 0);
                    break;
                case DiceFace.Five:
                    Camera.Position = new Point3D(cameraDistance, 0, 0);//setting the camera position in world coordinates
                    Camera.LookDirection = new Vector3D(-1, 0, 0);//defininig the direction in which the camera looking in world coordinates
                    Camera.UpDirection = new Vector3D(0, 1, 0);
                    myDLight.Direction = new Vector3D(-1, 0, 0);
                    break;
                case DiceFace.Six:
                    Camera.Position = new Point3D(0, 0, cameraDistance);//setting the camera position in world coordinates
                    Camera.LookDirection = new Vector3D(0, 0, -1);//defininig the direction in which the camera looking in world coordinates
                    Camera.UpDirection = new Vector3D(0, 1, 0);
                    myDLight.Direction = new Vector3D(0, 0, -1);
                    break;
            }
        }

    }
}
