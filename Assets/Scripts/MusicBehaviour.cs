using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class MusicBehaviour
    {
        public MusicBehaviour()
        {
            AudioClip clip = Resources.Load<AudioClip>("Sounds/parsec-soundtrack.mp3");
            AudioSource.PlayClipAtPoint(clip, GameObject.Find("Rocket").transform.position);
        }
    }
}
