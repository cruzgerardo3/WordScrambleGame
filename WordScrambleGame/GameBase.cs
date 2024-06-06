using System.Media;

namespace WordScrambleGame
{
    public class GameBase
    {

        public void PlaySound(string soundFile)
        {
            SoundPlayer soundPlayer = new SoundPlayer(soundFile);
            soundPlayer.Play();
        }
    }
}