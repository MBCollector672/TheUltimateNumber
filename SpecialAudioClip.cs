using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TheUltimateNumber
{
    [CreateAssetMenu]
    public class SpecialAudioClip : ScriptableObject
    {
        public AudioClip clip = new AudioClip();
        public int value;
        public bool explodeAfter = false;
    }
}
