using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MusicBehaviour : MonoBehaviour
    {
        private void Start()
        {
            PlayMusicMainMenu();
        }
        private void Update()
        {

        }

        #region TO SEPARATE CLASS
        void PlayMusicMainMenu()
        {
            string playerName = "SoundPlayer";
            GameObject player = GameObject.Find(playerName);
            if (player != null)
            {
                AudioSource src = player.GetComponent<AudioSource>();
                if (src != null)
                    src.Play();
                else
                    Debug.Log($"Could not get source of \"{playerName}\"");
            }
            else
                Debug.Log($"Could not get object \"{playerName}\"");
        }

        void PlayOnceMainMenu()
        {
            string resPathNoExt = "parsec-v20251202T21-main-theme";
            AudioClip clip = Resources.Load<AudioClip>(resPathNoExt);
            if (clip != null)
            {
                string playObjName = "PlayBtn";
                GameObject playObj = GameObject.Find(playObjName);
                if (playObj != null)
                    AudioSource.PlayClipAtPoint(clip, playObj.transform.position, 1.0f);
                else
                    Debug.Log($"Scene does not contain object \"{playObjName}\"");
            }
            else
                Debug.Log($"Could not load {resPathNoExt}");
        }
        #endregion
    }
}
