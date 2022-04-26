using System;
using System.Windows;
using System.Threading.Tasks;
using System.Timers;
using TetrisLibrary;
using TetrisLibrary.Misc;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TetrisUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private GameEngine game;
        private Timer timer;

        public GamePage()
        {
            timer = new Timer(500);
            timer.Elapsed += OnTimerTick;
            timer.AutoReset = true;

            InitializeComponent();
            
            game = new GameEngine();


            //Controls
            //GoBack button windows
            var view = SystemNavigationManager.GetForCurrentView();
            view.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            view.BackRequested += GoBack;

            //GoBack button phone
            KeyboardAccelerator goBack = new KeyboardAccelerator();
            goBack.Key = Windows.System.VirtualKey.GoBack;
            goBack.Invoked += GoBack;
            KeyboardAccelerators.Add(goBack);

            //Move left
            KeyboardAccelerator moveLeftArrow = new KeyboardAccelerator();
            moveLeftArrow.Key = Windows.System.VirtualKey.Left;
            moveLeftArrow.Invoked += MoveLeft;
            KeyboardAccelerators.Add(moveLeftArrow);
            KeyboardAccelerator moveLeftKeyboard = new KeyboardAccelerator();
            moveLeftKeyboard.Key = Windows.System.VirtualKey.A;
            moveLeftKeyboard.Invoked += MoveLeft;
            KeyboardAccelerators.Add(moveLeftKeyboard);

            //Move right
            KeyboardAccelerator moveRightArrow = new KeyboardAccelerator();
            moveRightArrow.Key = Windows.System.VirtualKey.Right;
            moveRightArrow.Invoked += MoveRight;
            KeyboardAccelerators.Add(moveRightArrow);
            KeyboardAccelerator moveRightKeyboard = new KeyboardAccelerator();
            moveRightKeyboard.Key = Windows.System.VirtualKey.D;
            moveRightKeyboard.Invoked += MoveRight;
            KeyboardAccelerators.Add(moveRightKeyboard);

            //Move down
            KeyboardAccelerator moveDownArrow = new KeyboardAccelerator();
            moveDownArrow.Key = Windows.System.VirtualKey.Down;
            moveDownArrow.Invoked += MoveDown;
            KeyboardAccelerators.Add(moveDownArrow);
            KeyboardAccelerator moveDownKeyboard = new KeyboardAccelerator();
            moveDownKeyboard.Key = Windows.System.VirtualKey.S;
            moveDownKeyboard.Invoked += MoveDown;
            KeyboardAccelerators.Add(moveDownKeyboard);

            //Rotate
            KeyboardAccelerator rotateArrow = new KeyboardAccelerator();
            rotateArrow.Key = Windows.System.VirtualKey.Up;
            rotateArrow.Invoked += Rotate;
            KeyboardAccelerators.Add(rotateArrow);
            KeyboardAccelerator rotateKeyboard = new KeyboardAccelerator();
            rotateKeyboard.Key = Windows.System.VirtualKey.W;
            rotateKeyboard.Invoked += Rotate;
            KeyboardAccelerators.Add(rotateKeyboard);

            StartGame();
        }

        private void StartGame()
        {
            game.Start();
            timer.Enabled = true;
        }

        private async Task UpdateCanvasAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                canvas.Children.Clear();
                double pixelWidth = canvas.Width;
                double pixelHeight = canvas.Height;
                try
                {
                    foreach (TetrisLibrary.Shape shape in game.GetShapes())
                    {
                        SolidColorBrush color = null;
                        switch (shape.Type)
                        {
                            case ShapeEnum.I:
                                color = new SolidColorBrush(Windows.UI.Colors.LightBlue);
                                break;
                            case ShapeEnum.O:
                                color = new SolidColorBrush(Windows.UI.Colors.Yellow);
                                break;
                            case ShapeEnum.T:
                                color = new SolidColorBrush(Windows.UI.Colors.MediumPurple);
                                break;
                            case ShapeEnum.S:
                                color = new SolidColorBrush(Windows.UI.Colors.LightGreen);
                                break;
                            case ShapeEnum.Z:
                                color = new SolidColorBrush(Windows.UI.Colors.Red);
                                break;
                            case ShapeEnum.J:
                                color = new SolidColorBrush(Windows.UI.Colors.DeepSkyBlue);
                                break;
                            case ShapeEnum.L:
                                color = new SolidColorBrush(Windows.UI.Colors.Orange);
                                break;
                            default:
                                break;
                        }
                        foreach (ShapeComponent component in shape.Components)
                        {
                            Rectangle rct = new Rectangle
                            {
                                Width = 30,
                                Height = 30,
                                Fill = color,
                                StrokeThickness = 1,
                                Stroke = new SolidColorBrush(Windows.UI.Colors.Black)

                            };
                            Canvas.SetLeft(rct, component.X * 30);
                            Canvas.SetTop(rct, 570 - component.Y * 30);
                            canvas.Children.Add(rct);
                        }
                    }
                } catch (Exception e) {}
                
            });
        }


        public async void OnTimerTick(Object source, ElapsedEventArgs e)
        {
            game.SimulateTimerTick();
            await UpdateCanvasAsync();
        }

        //Controls
        private async void MoveLeft(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            game.MoveShape(game.current, Direction.LEFT);
            await UpdateCanvasAsync();
        }

        private async void MoveRight(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            game.MoveShape(game.current, Direction.RIGHT);
            await UpdateCanvasAsync();
        }

        private async void MoveDown(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            game.SimulateTimerTick();
            await UpdateCanvasAsync();
        }

        private async void Rotate(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            game.RotateRight();
            await UpdateCanvasAsync();
        }

        private void GoBack(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            GoBack(sender, args);
        }

        private void GoBack(object sender, BackRequestedEventArgs e)
        {
            if(Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            } else
            {
                e.Handled = false;
            }
        }

    }
}
