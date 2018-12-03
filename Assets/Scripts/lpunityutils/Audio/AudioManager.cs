using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LPUnityUtils
{

    public class AudioManager : Singleton<AudioManager>
    {
        public List<Sound> Sounds;

        void Awake()
        {
            if (!EnforceSingleton(false))
            {
                return;
            }

            foreach (Sound sound in Sounds)
            {
                sound.Source = gameObject.AddComponent<AudioSource>();
            }
        }

        public void Play(string soundName)
        {
            Sound s = Sounds.Find(sound => sound.Name == soundName);
            if (s == null)
            {
                Debug.Log("Sound not found! " + soundName);
                return;
            }
            s.Source.Play();
        }
    }

}