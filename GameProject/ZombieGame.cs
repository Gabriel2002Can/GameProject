//Hello! This is the Top Down Zombie Shooter Game!
//There are some basic concepts to play this game!
//You can control your character's movement using the wasd keys from your keyboard.
//You can control your character's aim pointing with your mose to where you want to aim.
//And you can shoot with the spaceBar key.
//The game has 5 waves, each wave will have more zombies than the previous one.
//The game will end when you pass all five waves.
//To clear a wave , you need to kill all the zombies.
//Your character have 4 points of Health, and you will lose one if you get hit by a zombie.
//
//If you complete the game, or if you lose all of your Health, you will be asked if you want to restart the game.
//
//If you want to pause the game or you wish to review to game commands just press the "P" key.
//
//Have fun!
//
// GDI+ is used here for drawing graphics (System.Drawing namespace).
// Double-buffering is enabled to avoid flickering while rendering animations.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using GameProject;
using System.Media;

public class ZombieGame : Form
{
    Timer gameTimer = new Timer();
    List<Bullet> bullets = new List<Bullet>();
    List<Zombie> zombies = new List<Zombie>();
    List<BloodSplat> bloodSplats = new List<BloodSplat>();

    public SoundPlayer shootSound;

    private WaveManager waveManager;

    int speed = 5;

    bool up, down, left, right;

    Player player;

    public ZombieGame()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.WindowState = FormWindowState.Maximized;
        this.TopMost = true;
        shootSound = new SoundPlayer("Sounds/shoot-5-102360.wav");

        player = new Player(400, 300);

        waveManager = new WaveManager();


        this.DoubleBuffered = true;
        this.Width = 800;
        this.Height = 600;
        this.Text = "Top-Down Zombie Shooter";

        //FRAMERATE
        //gameTimer.Interval = 60;
        gameTimer.Interval = 20; // (+-60 frames per second)

        gameTimer.Tick += GameLoop;
        gameTimer.Start();

        this.KeyDown += OnKeyDown;
        this.KeyUp += OnKeyUp;
        this.MouseMove += OnMouseMove;

    }

    private void openMenu()
    {
        gameTimer.Stop();
        MessageBox.Show("Luis Gabriel Stedile Portella\nW0490083\n\n\nHello! This is the Top Down Zombie Shooter Game!\r\n\nThere are some basic concepts to play this game!\r\nYou can control your character's movement using the wasd keys from your keyboard.\r\nYou can control your character's aim pointing with your mose to where you want to aim.\r\nAnd you can shoot with the spaceBar key.\r\n\nThe game has 5 waves, each wave will have more zombies than the previous one.\r\nThe game will end when you pass all five waves.\r\nTo clear a wave , you need to kill all the zombies.\r\nYour character have 4 points of Health, and you will lose one if you get hit by a zombie.\r\n\r\nIf you complete the game, or if you lose all of your Health, you will be asked if you want to restart the game.\r\n\r\nHave fun!");
        gameTimer.Start();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Escape)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Normal;
            this.TopMost = false;
            return true;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void GameLoop(object sender, EventArgs e)
    {
        // Handle player movement
        if (up) player.Y -= speed;
        if (down) player.Y += speed;
        if (left) player.X -= speed;
        if (right) player.X += speed;

        int playerSize = 64;
        player.X = Math.Max(0, Math.Min(this.ClientSize.Width - playerSize, player.X));
        player.Y = Math.Max(0, Math.Min(this.ClientSize.Height - playerSize, player.Y));

        // If player is moving, change the frame (this could be improved with more conditions for different directions)
        if (up || down || left || right)
        {
            player.isMoving = true;
        }
        else
        {
            player.isMoving = false;
        }

        if (player.ShootCooldown > 0)
            player.ShootCooldown--;

        if (player.isMoving)
        {
            // Cycle through the frames to animate
            player.CurrentFrame++;
            if (player.CurrentFrame >= player.playerSprites.Count) player.CurrentFrame = 0;
        }

        foreach(var zombie in zombies)
        {
            zombie.currentFrame++;
            if (zombie.currentFrame >= zombie.zombieSprites.Count) zombie.currentFrame = 0;
        }

        foreach (var bullet in bullets)
        {
            bullet.Move();
        }

        foreach (var zombie in zombies)
        {
            zombie.MoveToward(player.X, player.Y); // Move toward the player
        }

        foreach (var zombie in zombies)
        {
            if (zombie.TryAttack(player) && !player.isPlayerInvulnerable)
            {
                player.Health--;
                player.isPlayerInvulnerable = true;
                player.invulnTimer = 90; // 1 second of invulnerability

                if (player.Health <= 0)
                {
                    ResetMovementFlags();

                    // Game Over
                    gameTimer.Stop();
                    MessageBox.Show("You died!");
                    waveManager.restart(zombies, player, gameTimer);
                    break;
                }
            }
        }

        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            var bullet = bullets[i];
            for (int j = zombies.Count - 1; j >= 0; j--)
            {
                var zombie = zombies[j];
                if (bullet.GetBounds().IntersectsWith(zombie.GetBounds()))
                {
                    zombie.TakeDamage();
                    bloodSplats.Add(new BloodSplat(zombie.X, zombie.Y)); // Add blood splat

                    bullets.RemoveAt(i);

                    if (!zombie.IsAlive)
                        zombies.RemoveAt(j);

                    break;
                }
            }
        }

        waveManager.Update(zombies,player,gameTimer);

        if (player.isPlayerInvulnerable)
        {
            player.invulnTimer--;
            if (player.invulnTimer <= 0)
                player.isPlayerInvulnerable = false;
        }

        foreach (var zombie in zombies)
        {
            // Distance to player
            int dx = zombie.X - player.X;
            int dy = zombie.Y - player.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance < 50 && zombie.attackCooldown <= 0)
            {
                zombie.isAttacking = true;

            }
            else if (!zombie.isAttacking)
            {
                zombie.MoveToward(player.X, player.Y); // only move if not attacking
            }

            if (zombie.attackCooldown > 0)
                zombie.attackCooldown--;
        }

        foreach (var splat in bloodSplats)
        {
            splat.Update();
        }

        player.Update();

        // Remove bullets that are off the screen
        bullets.RemoveAll(b => b.Y < 0);

        Invalidate(); // Trigger a redraw
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        g.Clear(Color.Black);

        player.Draw(g);

        //foreach (var bullet in bullets)
        //{
        //    bullet.Draw(g);
        //}

        foreach (var zombie in zombies)
        {
            zombie.Draw(g);
        }

        foreach (var splat in bloodSplats)
        {
            splat.Draw(g);
        }

        g.DrawString($"Wave: {waveManager.CurrentWave}", new Font("Arial", 16), Brushes.White, 10, 10);

        string positionText = $"Health: {player.Health}";
        Font font = new Font("Arial", 16);
        SizeF textSize = g.MeasureString(positionText, font);

        float xPosition = this.ClientSize.Width - textSize.Width - 10; // 10px from the right edge
        float yPosition = 10; // 10px from the top

        g.DrawString(positionText, font, Brushes.White, xPosition, yPosition);

        //bloodSplats.RemoveAll(s => s.IsFinished);
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {           
        if(e.KeyCode == Keys.P)
        {
            openMenu();
        }

        // Handle movement keys
        if (e.KeyCode == Keys.W) up = true;
        if (e.KeyCode == Keys.S) down = true;
        if (e.KeyCode == Keys.A) left = true;
        if (e.KeyCode == Keys.D) right = true;

        // Shoot bullet on spacebar press
        if (e.KeyCode == Keys.Space && player.ShootCooldown == 0)
        {
            bullets.Add(new Bullet(player.X + 28, player.Y+16, player.Angle)); // Spawn a bullet at player's position
            shootSound.Stop();
            shootSound.Play();
            player.ShootCooldown = player.MaxShootCooldown;
            player.TriggerMuzzleFlash();
        }
    }
    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.W) up = false;
        if (e.KeyCode == Keys.S) down = false;
        if (e.KeyCode == Keys.A) left = false;
        if (e.KeyCode == Keys.D) right = false;
    }

    private void ResetMovementFlags()
    {
        up = false;
        down = false;
        left = false;
        right = false;
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        // Calculate the angle between the player and the mouse cursor
        float dx = e.X - (player.X + 32); // Mouse X - Player X (centered)
        float dy = e.Y - (player.Y + 32); // Mouse Y - Player Y (centered)

        // Calculate the angle in radians and convert to degrees
        player.Angle = (float)(Math.Atan2(dy, dx) * (180 / Math.PI)); // Convert to degrees
    }

    [STAThread]
    static void Main()
    {
        Application.Run(new ZombieGame());
    }
}
