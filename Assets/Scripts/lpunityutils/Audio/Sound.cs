// Copyright Olli Etuaho 2018

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
                SetToSource(_Source);
            }
        }

        public void SetToSource(AudioSource source)
        {
            source.clip = Clip;
            source.volume = Volume;
            source.pitch = Pitch;
            source.loop = Loop;
        }
    }

}  // namespace LPUnityUtils
