using System.Collections;
using System.Collections.Generic;

using UnityEngine;
//using UnityEngine.Networking;
using UnityEngine.UI;

using Chowizard.UnityNetwork.Client.Character;
using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;
using Chowizard.UnityNetwork.Client.Scene;

namespace Chowizard.UnityNetwork.Client.Ui
{
    public class UiSetControlPanelServer : UiSet
    {
        public Button uiButtonTerminate;


        public Button uiButtonAddNpc;
        public Slider uiSliderAddNpcUnitCount;
        public Text uiTextAddNpcUnitCount;
        public int addNpcUnitMinimum = 1;
        public int addNpcUnitMaximum = 10;
        private int currentAddNpcUnitCount;


        public Slider uiSliderNpcSendRate;
        public Text uiTextNpcSendRate;
        private int npcSendRate;

        public Slider uiSliderNpcMovementThreshold;
        public Text uiTextNpcMovementThreshold;
        private float npcMovementThreshold;

        public Slider uiSliderNpcSnapThreshold;
        public Text uiTextNpcSnapThreshold;
        private float npcSnapThreshold;

        public Slider uiSliderNpcInterpolateMovementFactor;
        public Text uiTextNpcInterpolateMovementFactor;
        private float npcInterpolateMovementFactor;

        public void OnClickTerminate()
        {
            NetworkManager.Instance.Terminate();
            GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.Intro);
        }

        public void OnClickAddNpc()
        {
            if(GameSceneManager.Instance.currentScene == null)
                return;

            if(GameSceneManager.Instance.currentScene.sceneType != GameScene.eSceneType.GamePlay)
                return;

            if(NetworkManager.Instance.IsReadyByServer == false)
                return;

            SceneGamePlay scene = GameSceneManager.Instance.currentScene as SceneGamePlay;
            Debug.Assert(scene != null);

            scene.SpawnNonPlayerCharacters(currentAddNpcUnitCount);
        }

        // Use this for initialization
        private void Start()
        {
            Debug.Assert(uiButtonTerminate != null);
            Debug.Assert(uiButtonAddNpc != null);
            Debug.Assert(uiSliderAddNpcUnitCount != null);
            Debug.Assert(uiTextAddNpcUnitCount != null);
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateAddNpcUnitCount();
            UpdateNetworkConfiguration();
        }

        private void UpdateAddNpcUnitCount()
        {
            if(uiSliderAddNpcUnitCount.minValue != addNpcUnitMinimum)
                uiSliderAddNpcUnitCount.minValue = addNpcUnitMinimum;

            if(uiSliderAddNpcUnitCount.maxValue != addNpcUnitMaximum)
                uiSliderAddNpcUnitCount.maxValue = addNpcUnitMaximum;

            if(uiSliderAddNpcUnitCount.value != currentAddNpcUnitCount)
            {
                currentAddNpcUnitCount = (int)uiSliderAddNpcUnitCount.value;
                uiTextAddNpcUnitCount.text = string.Format("Add NPC Unit Count : {0}", currentAddNpcUnitCount);
            }
        }

        private void UpdateNetworkConfiguration()
        {
            UpdateNpcSendRate();
            UpdateNpcMovementThreshold();
            UpdateNpcSnapThreshold();
            UpdateNpcInterpolateMovementFactor();
        }

        private void UpdateNpcSendRate()
        {
            if(uiSliderNpcSendRate.value != npcSendRate)
            {
                npcSendRate = (int)uiSliderNpcSendRate.value;

                CharacterEntity[] entities = EntityManager.Instance.Entities;
                if(entities != null)
                {
                    foreach(CharacterEntity characterEntity in entities)
                    {
                        if(characterEntity == null)
                            continue;

                        if(characterEntity.property.isPlayer == true)
                            continue;

                        NetworkTransformSynchronizer networkTransformSync = characterEntity.GetComponent<NetworkTransformSynchronizer>();
                        if(networkTransformSync == null)
                            continue;

                        networkTransformSync.sendRate = npcSendRate;
                    }
                }

                uiTextNpcSendRate.text = string.Format("NPC Network Send Rate : {0} per seconds.", npcSendRate);
            }
        }

        private void UpdateNpcMovementThreshold()
        {
            if(uiSliderNpcMovementThreshold.value != npcMovementThreshold)
            {
                npcMovementThreshold = uiSliderNpcMovementThreshold.value;

                CharacterEntity[] entities = EntityManager.Instance.Entities;
                if(entities != null)
                {
                    foreach(CharacterEntity characterEntity in entities)
                    {
                        if(characterEntity == null)
                            continue;

                        if(characterEntity.property.isPlayer == true)
                            continue;

                        NetworkTransformSynchronizer networkTransformSync = characterEntity.GetComponent<NetworkTransformSynchronizer>();
                        if(networkTransformSync == null)
                            continue;

                        networkTransformSync.positionThreshold = npcMovementThreshold;
                    }
                }

                uiTextNpcMovementThreshold.text = string.Format("NPC Network Movement Threshold : {0}", npcMovementThreshold);
            }
        }

        private void UpdateNpcSnapThreshold()
        {
            if(uiSliderNpcSnapThreshold.value != npcSnapThreshold)
            {
                npcSnapThreshold = uiSliderNpcSnapThreshold.value;
                uiTextNpcSnapThreshold.text = string.Format("NPC Network Snap Threshold : {0}", npcSnapThreshold);
            }
        }

        private void UpdateNpcInterpolateMovementFactor()
        {
            if(uiSliderNpcInterpolateMovementFactor.value != npcInterpolateMovementFactor)
            {
                npcInterpolateMovementFactor = uiSliderNpcInterpolateMovementFactor.value;

                CharacterEntity[] entities = EntityManager.Instance.Entities;
                if(entities != null)
                {
                    foreach(CharacterEntity characterEntity in entities)
                    {
                        if(characterEntity == null)
                            continue;

                        if(characterEntity.property.isPlayer == true)
                            continue;

                        NetworkTransformSynchronizer networkTransformSync = characterEntity.GetComponent<NetworkTransformSynchronizer>();
                        if(networkTransformSync == null)
                            continue;

                        networkTransformSync.positionInterpolationFactor = npcInterpolateMovementFactor;
                    }
                }

                uiTextNpcInterpolateMovementFactor.text = string.Format("NPC Network Interpolate Movement Factor : {0}", npcInterpolateMovementFactor);
            }
        }
    }
}

