using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    public class BloodSplat
    {
        public int X { get; }
        public int Y { get; }
        private List<Image> frames = new List<Image>();
        private int currentFrame = 0;
        private int frameDelay = 5;
        private int frameCounter = 0;

        public bool IsFinished => currentFrame >= frames.Count;

        public BloodSplat(int x, int y)
        {
            X = x;
            Y = y-16;

            for (int i = 0; i <= 14; i++) 
            {
                frames.Add(Image.FromFile($"Images/BloodSplat/bloodsplats_{i}.png"));
            }
        }

        public void Update()
        {
            frameCounter++;
            if (frameCounter >= frameDelay)
            {
                currentFrame++;
                frameCounter = 0;
            }
        }

        public void Draw(Graphics g)
        {
            if (!IsFinished)
            {
                g.DrawImage(frames[Math.Min(currentFrame, frames.Count - 1)], X, Y, 64, 64);
            }
        }
    }
}
