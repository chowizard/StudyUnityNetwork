using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Chowizard.UnityNetwork.Client.Scene
{
    public abstract class GameScene : MonoBehaviour
    {
        public enum eSceneType
        {
            None = 0,

            Intro,
            Start,
            Lobby,
            GamePlay
        }

        public eSceneType sceneType = eSceneType.None;
    }
}
