using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace SpaceMobaClient.Systems.Sound
{
    public static class SoundManager
    {
        private static Song CurrentSong;

        /// <summary>
        /// Plays a given song.
        /// </summary>
        /// <param name="song">Name of song to play.</param>
        public static void PlaySong(string song)
        {
            try
            {
                CurrentSong = GameClient.GetGameClient().Content
                    .Load<Song>(song);

                MediaPlayer.Play(CurrentSong);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.25f;
            }
            catch(Exception e)
            {
                Trace.WriteLine("Exception in SoundManager.PlaySong():");
                Trace.WriteLine(e.ToString());
            }
        }
    }
}
