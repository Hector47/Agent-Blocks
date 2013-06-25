#define TinyCore2
using System;
using Microsoft.SPOT;
using AgentTetris001.GameLogic;
using System.Threading;
using AgentTetris001.Hardware;

namespace AgentTetris001.Presentation
{
    public class GameWindow
    {
        GameUniverse gameUniverse = new GameUniverse();
        UniverseView universeView;
#if !TinyCore
        Timer gameTimer;
#endif
#if TinyCore
        DispatcherTimer gameTimer;
#endif
        TimeSpan dueTime = new TimeSpan(0);

        public GameWindow()
        {
            // Tetris grid
            universeView = new UniverseView(gameUniverse);
            
            //Buttons
            ButtonHelper.ButtonSetup = new Buttons[]
            {
                Buttons.BottomRight, Buttons.MiddleRight, Buttons.TopRight
            };

            ButtonHelper.Current.OnButtonPress += Current_OnButtonPress;     

        }

        void Current_OnButtonPress(Buttons button, Microsoft.SPOT.Hardware.InterruptPort port, ButtonDirection direction, DateTime time)
        {
            // If game is over then every key close the window
            if (gameUniverse.Statistics.GameOver)
            {
                this.StartGame(1);
                //this.Close();
                //parentApp.SetFocus();

                //if (OnClose != null)
                //    OnClose(this, gameUniverse.Statistics);
            }
            // else handle the keys
            else
            {
                if (direction == ButtonDirection.Down)
                {
                    switch (button)
                    {
                        case Buttons.TopLeft:
                            break;
                        case Buttons.BottomLeft:
                            break;
                        case Buttons.TopRight:

                            gameUniverse.StepLeft();
                            break;
                        case Buttons.MiddleRight:
                            gameUniverse.Rotate();
                            break;
                        case Buttons.BottomRight:
                            gameUniverse.StepRight();
                            break;
                        default:
                            break;
                    }
                }
                //switch (button)
                //{
                //    case Button.Left:
                //        gameUniverse.StepLeft();
                //        break;
                //    case Button.Right:
                //        gameUniverse.StepRight();
                //        break;
                //    case Button.Up:
                //        gameUniverse.Rotate();
                //        break;
                //    case Button.Select:
                //        gameUniverse.DropDown();
                //        break;
                //}
            }

            universeView.Render();
        }

        /// <summary>
        /// Starts game on specified level
        /// </summary>
        /// <param name="startLevel">Leve to start game</param>
        public void StartGame(int startLevel)
        {
            // Prepare universe
            gameUniverse.Init();
            gameUniverse.StartLevel(startLevel);
            gameUniverse.StepUniverse();

#if !TinyCore
            // Start tick timer
            
            TimeSpan period = new TimeSpan(0, 0, 0, 0, gameUniverse.Statistics.Interval);
            gameTimer = new Timer(GameTimer_Tick, null, period, period);

            //Start tick timer
#endif
#if TinyCore
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, gameUniverse.Statistics.Interval);
            gameTimer.Tick += new EventHandler(GameTimer_Tick);
            gameTimer.Start();
#endif
        }

#if !TinyCore
       /// <summary>
        /// Game timer tick handler
        /// </summary>
        private void GameTimer_Tick(object State)
        {
            gameUniverse.StepUniverse();

            // If next level then update timer speed and update statsPanel
            if (gameUniverse.Statistics.NextLevel)
            {
                gameTimer.Change(dueTime, new TimeSpan(0, 0, 0, 0, gameUniverse.Statistics.Interval));
                gameUniverse.Statistics.NextLevel = false;
            }

            // If game is over then stop the counter
            if (gameUniverse.Statistics.GameOver)
            {
                gameTimer.Dispose();
            }

            //universeView.Invalidate();
            universeView.Render();
        }    
#endif
#if TinyCore
        /// <summary>
        /// Game timer tick handler
        /// </summary>
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            gameUniverse.StepUniverse();

            // If next level then update timer speed and update statsPanel
            if (gameUniverse.Statistics.NextLevel)
            {
                gameTimer.Interval = new TimeSpan(0, 0, 0, 0, gameUniverse.Statistics.Interval);
                gameUniverse.Statistics.NextLevel = false;
            }

            // If game is over then stop the counter
            if (gameUniverse.Statistics.GameOver)
            {
                gameTimer.Stop();
            }

            //universeView.Invalidate();
        }
#endif

        



    }
}
