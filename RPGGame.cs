// RPGGame.cs
// --------------------
// Simple RPG Game using C# and Windows Forms
// Controls: Arrow Keys to move the player.
// Goal: Reach the treasure to win.
// Press "R" to restart the game.
// Includes basic animation and sound.

// GDI+ is used here for drawing graphics (System.Drawing namespace).
// Double-buffering is enabled to avoid flickering while rendering animations.

using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

public class RPGGame : Form
{
    Rectangle player = new Rectangle(50, 50, 40, 40);
    Rectangle goal = new Rectangle(300, 200, 40, 40);
    bool gameOver = false;
    Timer gameTimer = new Timer();
    SoundPlayer winSound;

    public RPGGame()
    {
        this.Text = "Simple RPG Game";
        this.Size = new Size(400, 300);
        this.DoubleBuffered = true; // Enables double-buffering
        this.KeyDown += new KeyEventHandler(OnKeyDown);
        this.Paint += new PaintEventHandler(OnPaint);

        gameTimer.Interval = 100;
        gameTimer.Tick += new EventHandler(UpdateGame);
        gameTimer.Start();

        // Load a simple win sound (you can replace the path with your own .wav file)
        //winSound = new SoundPlayer("win.wav"); // Make sure the file is in your project folder
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (gameOver)
        {
            if (e.KeyCode == Keys.R)
            {
                RestartGame();
            }
            return;
        }

        switch (e.KeyCode)
        {
            case Keys.Left: player.X -= 10; break;
            case Keys.Right: player.X += 10; break;
            case Keys.Up: player.Y -= 10; break;
            case Keys.Down: player.Y += 10; break;
        }

        if (player.IntersectsWith(goal))
        {
            gameOver = true;
            //winSound.Play(); // Play win sound
        }

        Invalidate(); // Redraw the screen
    }

    private void UpdateGame(object sender, EventArgs e)
    {
        Invalidate(); // Triggers Paint event
    }

    private void RestartGame()
    {
        player.X = 50;
        player.Y = 50;
        gameOver = false;
        Invalidate();
    }

    private void OnPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        // Draw player
        g.FillRectangle(Brushes.Blue, player);

        // Draw goal
        g.FillRectangle(Brushes.Gold, goal);

        if (gameOver)
        {
            string message = "You Win! Press 'R' to Restart.";
            Font font = new Font("Arial", 12);
            g.DrawString(message, font, Brushes.Black, new Point(80, 100));
        }
    }

    [STAThread]
    public static void Main()
    {
        Application.Run(new RPGGame());
    }
}
