using System;
using System.Threading.Tasks;
using System.Timers;
using TetrisLibrary;
using TetrisLibrary.Misc;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Collections.Generic;

namespace TetrisUWP
{
    public sealed partial class GamePage : Page
    {
        private GameEngine game;
        private Timer gameTimer;
        private Timer elapsedTimer;
        private DateTime gameTime;
        private List<KeyboardAccelerator> buttons;

        public GamePage()
        {
            buttons = new List<KeyboardAccelerator>();
            gameTime = new DateTime(2022, 04, 28, 0, 0, 0);

            gameTimer = new Timer(500);
            gameTimer.Elapsed += OnTimerTick;
            gameTimer.AutoReset = true;

            elapsedTimer = new Timer(1000);
            elapsedTimer.Elapsed += OnElapsedTimerTick;
            elapsedTimer.AutoReset = true;

            InitializeComponent();
            InitKeyboard();

            game = new GameEngine();

            //GoBack button windows
            var view = SystemNavigationManager.GetForCurrentView();
            view.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            view.BackRequested += GoBack;

            StartGame();
        }

        private void StartGame()
        {
            ToggleKeyboard(true);
            game.Start();
            gameTimer.Start();
            elapsedTimer.Start();
        }

        private void EndGame()
        {
            gameTimer.Stop();
            elapsedTimer.Stop();
            ToggleKeyboard(false);
        }

        private void InitKeyboard()
        {
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //Move left
                KeyboardAccelerator moveLeftArrow = new KeyboardAccelerator();
                moveLeftArrow.Key = Windows.System.VirtualKey.Left;
                moveLeftArrow.Invoked += MoveLeft;
                buttons.Add(moveLeftArrow);
                KeyboardAccelerator moveLeftKeyboard = new KeyboardAccelerator();
                moveLeftKeyboard.Key = Windows.System.VirtualKey.A;
                moveLeftKeyboard.Invoked += MoveLeft;
                buttons.Add(moveLeftKeyboard);

                //Move right
                KeyboardAccelerator moveRightArrow = new KeyboardAccelerator();
                moveRightArrow.Key = Windows.System.VirtualKey.Right;
                moveRightArrow.Invoked += MoveRight;
                buttons.Add(moveRightArrow);
                KeyboardAccelerator moveRightKeyboard = new KeyboardAccelerator();
                moveRightKeyboard.Key = Windows.System.VirtualKey.D;
                moveRightKeyboard.Invoked += MoveRight;
                buttons.Add(moveRightKeyboard);

                //Move down
                KeyboardAccelerator moveDownArrow = new KeyboardAccelerator();
                moveDownArrow.Key = Windows.System.VirtualKey.Down;
                moveDownArrow.Invoked += MoveDown;
                buttons.Add(moveDownArrow);
                KeyboardAccelerator moveDownKeyboard = new KeyboardAccelerator();
                moveDownKeyboard.Key = Windows.System.VirtualKey.S;
                moveDownKeyboard.Invoked += MoveDown;
                buttons.Add(moveDownKeyboard);

                //Rotate
                KeyboardAccelerator rotateArrow = new KeyboardAccelerator();
                rotateArrow.Key = Windows.System.VirtualKey.Up;
                rotateArrow.Invoked += Rotate;
                buttons.Add(rotateArrow);
                KeyboardAccelerator rotateKeyboard = new KeyboardAccelerator();
                rotateKeyboard.Key = Windows.System.VirtualKey.W;
                rotateKeyboard.Invoked += Rotate;
                buttons.Add(rotateKeyboard);
            });
        }

        private void ToggleKeyboard(bool turnOn)
        {
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (turnOn)
                {
                    foreach (KeyboardAccelerator button in buttons)
                    {
                        KeyboardAccelerators.Add(button);
                    }
                }
                else
                {
                    KeyboardAccelerators.Clear();
                }
            });
        }

        private void UpdateGUI()
        {
            _ =  Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //Main canvas
                canvas.Children.Clear();
                double pixelWidth = canvas.Width;
                double pixelHeight = canvas.Height;
                try
                {
                    foreach (TetrisLibrary.Shape shape in game.GetShapes())
                    {
                        foreach (ShapeComponent component in shape.Components)
                        {
                            Rectangle rct = new Rectangle
                            {
                                Width = 30,
                                Height = 30,
                                Fill = GetColor(shape),
                                StrokeThickness = 1,
                                Stroke = new SolidColorBrush(Windows.UI.Colors.Black)

                            };
                            Canvas.SetLeft(rct, component.X * 30);
                            Canvas.SetTop(rct, 570 - component.Y * 30);
                            canvas.Children.Add(rct);
                        }
                    }
                } catch (Exception e) {}

                //Next shape
                canvas_nextShape.Children.Clear();
                foreach (ShapeComponent component in game.nextShape.Components)
                {
                    Rectangle rct = new Rectangle
                    {
                        Width = 30,
                        Height = 30,
                        Fill = GetColor(game.nextShape),
                        StrokeThickness = 1,
                        Stroke = new SolidColorBrush(Windows.UI.Colors.Black)

                    };
                    Canvas.SetLeft(rct, component.X * 30);
                    Canvas.SetTop(rct, 90 - component.Y * 30);
                    canvas_nextShape.Children.Add(rct);
                }

                //Score
                txt_score.Text = game.score.ToString();

                //Lines
                txt_lines.Text = game.lines.ToString();
            });
        }

        private SolidColorBrush GetColor(TetrisLibrary.Shape shape)
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
            return color;
        }

        public void OnTimerTick(Object source, ElapsedEventArgs e)
        {
            if(game.SimulateTimerTick())
            {
                EndGame();
            }
             UpdateGUI();
        }

        public void OnElapsedTimerTick(Object sender, ElapsedEventArgs e)
        {
            gameTime = gameTime.AddSeconds(1);
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                text_elapsedTime.Text = gameTime.ToString("mm:ss");
            });
        }


        //Controls
        private void MoveLeft(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            game.MoveShape(game.currentShape, Direction.LEFT);
            UpdateGUI();
        }

        private void MoveRight(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            game.MoveShape(game.currentShape, Direction.RIGHT);
            UpdateGUI();
        }

        private void MoveDown(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (game.SimulateTimerTick())
            {
                EndGame();
            }
            UpdateGUI();
        }

        private void Rotate(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            game.RotateRight();
            UpdateGUI();
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
