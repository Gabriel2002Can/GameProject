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
//Have fun!

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameProject
{
    public class WaveManager
    {
        public int CurrentWave { get; set; } = 0;
        public bool IsWaveCompleted { get; set; } = false;
        public int ZombiesRemaining { get; set; } = 0;
        private Random rand = new Random();

        private bool IsWaitingForNextWave = false;




        public void StartWave(Player player, List<Zombie> zombies)
        {
                CurrentWave++;
                ZombiesRemaining = CurrentWave; // Number of zombies based on wave
                SpawnZombies(player, zombies);
           
        }

        public void restart(List<Zombie> zombies, Player player, Timer gameTimer)
        {

            DialogResult result = MessageBox.Show(
                "Do you want to restart?",
                "Restart?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                zombies.Clear();
                player.Health = 4;

                CurrentWave = 0;
                IsWaitingForNextWave = true;
                gameTimer.Start();

                TimerHelper.StartTimer(1500, () =>
                {
                StartWave(player, zombies);
                IsWaitingForNextWave = false;
                });
            }
            else
            {
                // Exit the game
                Application.Exit();
            }
        }

        private void SpawnZombies(Player player, List<Zombie> zombies)
        {
            for (int i = 0; i < ZombiesRemaining; i++)
            {
                // Random spawn position (around player)
                float spawnX = player.X + rand.Next(-400, 400); // Example spawn around player
                float spawnY = player.Y + rand.Next(-400, 400); // Example spawn around player

                // Create and add a new zombie to the list
                Zombie newZombie = new Zombie((int)spawnX, (int)spawnY);
                zombies.Add(newZombie);
            }
        }

        public void Update(List<Zombie> zombies, Player player, Timer gameTimer)
        {
            if (IsWaitingForNextWave) return;

            // If no zombies remain, mark wave as completed
            foreach (var zombie in zombies)
            {
                if (zombie.IsAlive)
                {
                    return;
                }
            }

            if(CurrentWave >= 5)
            {
                gameTimer.Stop();
                MessageBox.Show("You have completed the game!");
                restart(zombies,player,gameTimer);
                return;
            }

            IsWaitingForNextWave = true;

            TimerHelper.StartTimer(1000, () =>
            {
                StartWave(player, zombies);
                Console.WriteLine($"Wave {CurrentWave} completed!");
                IsWaitingForNextWave = false;

            });

                //CurrentWave++;
                //StartWave(player, zombies);
                //Console.WriteLine($"Wave {CurrentWave} completed!");
            
        }
    }
}
