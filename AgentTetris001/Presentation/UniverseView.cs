using System;
using Microsoft.SPOT;
using AgentTetris001.GameLogic;
using Microsoft.SPOT.Presentation.Media;


namespace AgentTetris001.Presentation
{
    public class UniverseView
    {
        Font fontNinaB;

        const int NextBlockCOLUMNS = 27;
        const int NextBlockROWS = 6;
        const int COLUMN_WIDTH = 7;
        const int ROW_HEIGHT = 7;

        static Bitmap _display;
        //static Bitmap _gamedisplay;
        GameUniverse gameUniverse;

        /// <summary>
        /// Creates new UniverseView for specified GameUniverse
        /// </summary>
        /// <param name="gameUniverse">GameUniverse to visualize</param>
        public UniverseView(GameUniverse gameUniverse)
        {
            this.gameUniverse = gameUniverse;

            // initialize display buffer
            _display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);
            fontNinaB = Resources.GetFont(Resources.FontResources.small);
        }

        public void Render()
        {
            // Performance tunning
            // storing properties to variables for faster reuse
            int fieldCols = gameUniverse.Field.Columns;
            int fieldRows = gameUniverse.Field.Rows;
            
            _display.Clear();
            
            
            #region Draw the border

            _display.DrawLine(Color.White, 2, 73, 0, 73, 127);
            
            #endregion



            #region Draw the game field
            for (int row = 0; row < fieldRows; row++)
                for (int col = 0; col < fieldCols; col++)
                {
                    int brushType = gameUniverse.Field.GetCell(row, col) - 1;
                    if (brushType >= 0)
                        _display.DrawRectangle(Color.White,
                                         1,
                                         col * COLUMN_WIDTH + 1,
                                         row * ROW_HEIGHT + 1,
                                         COLUMN_WIDTH - 1,
                                         ROW_HEIGHT - 1, 0, 0, Color.White, 0, 0, Color.White, 0, 0, 0);
                }
            #endregion
            #region Draw the current object
            // Performance tunning
            fieldCols = gameUniverse.CurrentBlock.Columns;
            fieldRows = gameUniverse.CurrentBlock.Rows;

            for (int row = 0; row < fieldRows; row++)
                for (int col = 0; col < fieldCols; col++)
                {
                    int brushType = gameUniverse.CurrentBlock.GetCell(row, col) - 1;
                    if (brushType >= 0)
                        _display.DrawRectangle(Color.White,
                                         1,
                                         (col + gameUniverse.BlockX) * COLUMN_WIDTH + 1,
                                         (row + gameUniverse.BlockY) * ROW_HEIGHT + 1,
                                         COLUMN_WIDTH - 1,
                                         ROW_HEIGHT - 1, 0, 0, Color.White, 0, 0, Color.White, 0, 0, 0);
                }
            #endregion
            #region Draw the next picece
            // Performance tunning
            fieldCols = gameUniverse.NextBlock.Columns;
            fieldRows = gameUniverse.NextBlock.Rows;

            int offsetX = ((COLUMN_WIDTH * NextBlockCOLUMNS) - (fieldCols * COLUMN_WIDTH) + 2) / 2;
            int offsetY = ((ROW_HEIGHT * NextBlockROWS) - (fieldRows * ROW_HEIGHT) + 2) / 2;

            for (int row = 0; row < fieldRows; row++)
                for (int col = 0; col < fieldCols; col++)
                {
                    int brushType = gameUniverse.NextBlock.GetCell(row, col) - 1;
                    if (brushType >= 0)
                        _display.DrawRectangle(Color.White,
                                         1,
                                         (col * COLUMN_WIDTH) + offsetX,
                                         (row * ROW_HEIGHT) + offsetY,
                                         COLUMN_WIDTH - 1,
                                         ROW_HEIGHT - 1, 0, 0, Color.White, 0, 0, Color.White, 0, 0, 0);
                }
            #endregion
            #region Draw Stat
            _display.DrawText("Score", fontNinaB, Color.White, 80, 50);
            _display.DrawText(gameUniverse.Statistics.Score.ToString(), fontNinaB,Color.White,80,60);

            _display.DrawText("Level", fontNinaB, Color.White, 80, 75);
            _display.DrawText(gameUniverse.Statistics.Level.ToString(), fontNinaB, Color.White, 80, 85);

            _display.DrawText("Lines", fontNinaB, Color.White, 80, 100);
            _display.DrawText(gameUniverse.Statistics.LinesCompleted.ToString(), fontNinaB, Color.White, 80, 110);


            #endregion
            #region Draw Game Over banner
            if (gameUniverse.Statistics.GameOver)
            {
                _display.DrawLine(Color.White, 10, 0, 60, 128, 60);
                _display.DrawText("Try Again?", fontNinaB, Color.Black, 40, 55);

            }
            #endregion

            _display.Flush();
        }

    }
}
