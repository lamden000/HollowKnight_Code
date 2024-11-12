using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    [Serializable]
    public class Sound
    {
        [SerializeField] private AudioClip audioClip;
        private float timePlayed;

        public void ResetTime()
        { timePlayed = 0; }

        public void Play(AudioSource audioSource)
        {
            audioSource.clip = audioClip;
            audioSource.time = timePlayed;
            audioSource.Play();
        }

        public void Stop(AudioSource audioSource)
        {
            timePlayed=audioSource.time;
            audioSource.Stop();
        }

    }

}
