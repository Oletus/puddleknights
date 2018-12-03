using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LPUnityUtils
{

    [System.Serializable]
    public class Sound
    {
        public string Name;

        [SerializeField] private AudioClip Clip;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float Volume = 0.5f;
        [Range(0.1f, 3.0f)]
        [SerializeField] private float Pitch = 1.0f;
        [SerializeField] private bool Loop = false;

        [System.NonSerialized] private AudioSource _Source;

        public AudioSource Source
        {
            get {
                return _Source;
            }
            set {
                _Source = value;
                _Source.clip = Clip;
                _Source.volume = Volume;
                _Source.pitch = Pitch;
                _Source.loop = Loop;
            }
        }
    }

}