// Copyright Olli Etuaho 2018

using System.Collections.Generic;
using UnityEngine;

namespace LPUnityUtils
{

    [CreateAssetMenu]
    public class SoundVariants : ScriptableObject
    {
        [SerializeField] private List<Sound> sounds;

        public void PlayOn(AudioSource source)
        {
            if (sounds.Count == 0)
            {
                Debug.Log("No sounds set as variants");
                return;
            }
            int soundIndex = Mathf.FloorToInt(Random.Range(0.0f, sounds.Count - Mathf.Epsilon));
            sounds[soundIndex].SetToSource(source);
            source.Play();
        }
    }
}