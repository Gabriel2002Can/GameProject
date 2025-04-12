using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Health { get; set; } = 4;
        public bool isPlayerInvulnerable { get; set; } = false;
        public int invulnTimer { get; set; } = 0;
        public bool isMoving { get; set; } = false;
        public int ShootCooldown { get; set; } = 0;
        public int MaxShootCooldown { get; set; } = 10;

        public int Width => 64;
        public int Height => 64;

        public int CurrentFrame { get; set; }

        public float Angle { get; set; } = 0f;

        public List<Image> playerSprites { get; set; }

        public bool ShowMuzzleFlash { get; set; } = false;
        private int muzzleFlashCounter = 0;
        private int muzzleFlashDuration = 5; // Number of frames to show it

        private Image muzzleFlashImage = Image.FromFile("Images/muzzle_flash_01.png");

        public Player(int x, int y)
        {
            X = x;
            Y = y;
            CurrentFrame = 0;

            playerSprites = new List<Image>();

            LoadPlayerSprites();

        }

        public void TriggerMuzzleFlash()
        {
            ShowMuzzleFlash = true;
            muzzleFlashCounter = muzzleFlashDuration;
        }

        public void Update()
        {
            if (ShowMuzzleFlash)
            {
                muzzleFlashCounter--;
                if (muzzleFlashCounter <= 0)
                {
                    ShowMuzzleFlash = false;
                }
            }
        }

        private void LoadPlayerSprites()
        {
            // Load all the 19 frames for the player animation
            for (int i = 0; i <= 19; i++)
            {
                
                playerSprites.Add(Image.FromFile($"Images/Moving/survivor-idle_handgun_{i}.png"));
            }
        }

        public void Draw(Graphics g)
        {
            g.TranslateTransform(X + 32, Y + 32);
            g.RotateTransform(Angle);
            g.DrawImage(playerSprites[CurrentFrame], -32,-32,64,64);

            if (ShowMuzzleFlash)
            {
                // Position flash at the tip of the gun
                float flashOffsetX = 28; // Distance from player center
                float flashOffsetY = 16;  // Straight ahead

                g.DrawImage(muzzleFlashImage, flashOffsetX - 16, flashOffsetY - 16, 32, 32);
            }

            g.ResetTransform(); 
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(X, Y, Width, Height);
        }
    }
}
