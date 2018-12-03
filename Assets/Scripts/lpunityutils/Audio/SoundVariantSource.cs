// Copyright Olli Etuaho 2018

using UnityEngine;

namespace LPUnityUtils
{

    public class SoundVariantSource : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private SoundVariants variants;

        public void Play()
        {
            variants.PlayOn(source);
        }
    }

}