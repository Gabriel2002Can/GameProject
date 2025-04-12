using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    public class Zombie
    {
        public int X { get; set; }
        public int Y { get; set; }

        //Zombie speed
        public float speed = 3.5f;

        public List<Image> zombieSprites = new List<Image>();

        public List<Image> attackSprites = new List<Image>();
        public int AttackFrameCounter { get; set; } = 0;

        public int currentFrame = 0;
        public int frameCounter = 0;
        public int frameDelay = 5;

        public bool isAttacking = false; // Flag to indicate if the zombie is attacking

        public int attackCooldown = 60; // Frames between attacks (1 sec at 60fps)
        public int currentCooldown = 0;

        public int health = 3; // Zombies have 3 HP
        public bool IsAlive = true;
        public float RotationAngle { get; private set; }


        public Zombie(int x, int y)
        {
            X = x;
            Y = y;

            // Load all zombie sprite frames
            for (int i = 0; i <= 16; i++) 
            {
                zombieSprites.Add(Image.FromFile($"Images/ZombieMoving/skeleton-move_{i}.png"));
            }

            for(int i = 0; i <= 8; i++)
            {
                attackSprites.Add(Image.FromFile($"Images/ZombieAttack/skeleton-attack_{i}.png"));
            }
        }

        public void TakeDamage()
        {
            health--;

            if(health <= 0)
            {
                IsAlive = false;
            }
        }

        public bool TryAttack(Player player)
        {
            float dist = DistanceTo(player.X, player.Y);

            if (dist < 40 && currentCooldown <= 0)
            {
                currentCooldown = attackCooldown;
                isAttacking = true;
                return true; // Hit successful!
            }
            
            currentCooldown--;
            return false;
        }

        private float DistanceTo(int targetX, int targetY)
        {
            float dx = targetX - X;
            float dy = targetY - Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public void MoveToward(int targetX, int targetY)
        {
            int dx = targetX - X;
            int dy = targetY - Y;

            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            if (distance > 1)
            {
            
                X += (int)(dx / distance * speed);
                Y += (int)(dy / distance * speed);
            }

            RotationAngle = (float)(Math.Atan2(dy, dx) * 180 / Math.PI); // in degrees
        }

        public void Draw(Graphics g)
        {

            g.TranslateTransform(X + 32, Y + 32);
            g.RotateTransform(RotationAngle);


            if (isAttacking)
            {
                g.DrawImage(attackSprites[AttackFrameCounter], -32, -32, 64, 64);
                AttackFrameCounter++;

                if (AttackFrameCounter >= attackSprites.Count)
                {
                    AttackFrameCounter = 0;
                    isAttacking = false;
                    attackCooldown = 30; // delay before next attack (adjust as needed)
                }
            }
            else
            {
                g.DrawImage(zombieSprites[currentFrame], -32, -32, 64, 64);
            }

            g.ResetTransform();
        
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(X, Y, 50, 50); // For collision detection later
        }
    }
}
