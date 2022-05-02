using System;
using System.Collections.Generic;
using System.Timers;
using TetrisLibrary;
using TetrisLibrary.Misc;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace TetrisUWP
{
    public sealed partial class GamePage : Page
    {
        private GameEngine game;
        private Timer gameTimer;
        private Timer elapsedTimer;
        private DateTime gameTime;
        private List<KeyboardAccelerator> buttons;
        private GUIData data;

        public GamePage()
        {
            Init();
            StartGame();
        }

        private void Init()
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

            data = new GUIData();
            game = new GameEngine();
            DataContext = data;

            //GoBack button windows
            var view = SystemNavigationManager.GetForCurrentView();
            view.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            view.BackRequested += GoBack;
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
                KeyboardAccelerator moveLeftArrow = new KeyboardAccelerator
                {
                    Key = Windows.System.VirtualKey.Left
                };
                moveLeftArrow.Invoked += MoveLeft;
                buttons.Add(moveLeftArrow);
                KeyboardAccelerator moveLeftKeyboard = new KeyboardAccelerator
                {
                    Key = Windows.System.VirtualKey.A
                };
                moveLeftKeyboard.Invoked += MoveLeft;
                buttons.Add(moveLeftKeyboard);

                //Move right
                KeyboardAccelerator moveRightArrow = new KeyboardAccelerator
                {
                    Key = Windows.System.VirtualKey.Right
                };
                moveRightArrow.Invoked += MoveRight;
                buttons.Add(moveRightArrow);
                KeyboardAccelerator moveRightKeyboard = new KeyboardAccelerator
                {
                    Key = Windows.System.VirtualKey.D
                };
                moveRightKeyboard.Invoked += MoveRight;
                buttons.Add(moveRightKeyboard);

                //Move down
                KeyboardAccelerator moveDownArrow = new KeyboardAccelerator
                {
                    Key = Windows.System.VirtualKey.Down
                };
                moveDownArrow.Invoked += MoveDown;
                buttons.Add(moveDownArrow);
                KeyboardAccelerator moveDownKeyboard = new KeyboardAccelerator
                {
                    Key = Windows.System.VirtualKey.S
                };
                moveDownKeyboard.Invoked += MoveDown;
                buttons.Add(moveDownKeyboard);

                //Rotate
                KeyboardAccelerator rotateArrow = new KeyboardAccelerator
                {
                    Key = Windows.System.VirtualKey.Up
                };
                rotateArrow.Invoked += Rotate;
                buttons.Add(rotateArrow);
                KeyboardAccelerator rotateKeyboard = new KeyboardAccelerator
                {
                    Key = Windows.System.VirtualKey.W
                };
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
            _ = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
           {
               //Data
               if (game.Score != data.Score)
               {
                   data.Score = game.Score;
               }
               if (game.Lines != data.Lines)
               {
                   data.Lines = game.Lines;
               }
               if (game.NextShape != data.NextShape)
               {
                   data.NextShape = game.NextShape;
               }

               //Main canvas
               canvas.Children.Clear();
               double pixelWidth = canvas.Width;
               double pixelHeight = canvas.Height;
               try
               {
                   foreach (TetrisLibrary.Shape shape in game.Shapes)
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
               }
               catch (Exception e)
               {
                   e.ToString();
               }

               //Next shape

               canvas_nextShape.Children.Clear();
               while (game.NextShape == null) { }
               foreach (ShapeComponent component in game.NextShape.Components)
               {
                   Rectangle rct = new Rectangle
                   {
                       Width = 30,
                       Height = 30,
                       Fill = GetColor(game.NextShape),
                       StrokeThickness = 1,
                       Stroke = new SolidColorBrush(Windows.UI.Colors.Black)

                   };
                   Canvas.SetLeft(rct, component.X * 30);
                   Canvas.SetTop(rct, 90 - component.Y * 30);
                   canvas_nextShape.Children.Add(rct);
               }

           });
        }

        private SolidColorBrush GetColor(TetrisLibrary.Shape shape)
        {
            SolidColorBrush color = null;
            switch (shape.Type)
            {
                case ShapeEnum.I:
                case ShapeEnum.D:
                    color = new SolidColorBrush(Windows.UI.Colors.LightBlue);
                    break;
                case ShapeEnum.O:
                case ShapeEnum.Smiley:
                    color = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    break;
                case ShapeEnum.T:
                    color = new SolidColorBrush(Windows.UI.Colors.MediumPurple);
                    break;
                case ShapeEnum.N:
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
                case ShapeEnum.E:
                    color = new SolidColorBrush(Windows.UI.Colors.Orange);
                    break;
                default:
                    break;
            }
            return color;
        }

        public void OnTimerTick(Object source, ElapsedEventArgs e)
        {
            if (game.SimulateTimerTick())
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
            game.MoveShape(game.CurrentShape, Direction.LEFT);
            UpdateGUI();
        }

        private void MoveRight(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            game.MoveShape(game.CurrentShape, Direction.RIGHT);
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
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

    }
}
