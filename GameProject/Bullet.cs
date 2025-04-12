using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    public class Bullet
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public bool IsActive { get; set; }
        public float Angle { get; set; } // Angle in degrees

        public Bullet(int startX, int startY, float angle)
        {
            X = startX;
            Y = startY;
            Speed = 45;
            IsActive = true;
            Angle = angle;
        }

        public void Move()
        { 
            X += (int)(Speed * Math.Cos(Angle * Math.PI / 180));
            Y += (int)(Speed * Math.Sin(Angle * Math.PI / 180));
        }

        //public void Draw(Graphics g)
        //{
        //    // Draw a small black circle for the bullet
        //    g.FillEllipse(Brushes.Red, X, Y, 6, 6); // Size can be adjusted
        //}

        public Rectangle GetBounds()
        {
            return new Rectangle((int)X, (int)Y, 4, 4); // Same size as the bullet drawing
        }


    }
}