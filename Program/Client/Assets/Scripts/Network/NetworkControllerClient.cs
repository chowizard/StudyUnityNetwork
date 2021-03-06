﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

using Chowizard.UnityNetwork.Client.Character;
using Chowizard.UnityNetwork.Client.Core;
using Chowizard.UnityNetwork.Client.Network;
using Chowizard.UnityNetwork.Client.Network.Message;
using Chowizard.UnityNetwork.Client.Scene;

namespace Chowizard.UnityNetwork.Client.Network
{
    public sealed class NetworkControllerClient
    {
        private NetworkManager networkManager;
        private GameManager sceneMain;

        private NetworkClient netClient;

        public NetworkControllerClient(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
            sceneMain = networkManager.transform.parent.GetComponent<GameManager>();
        }

        public bool Setup()
        {
            return SetupClient();
        }

        public void Terminate()
        {
            if(netClient == null)
                return;

            if(netClient.connection != null)
            {
                /* 플레이어가 있다면 플레이어를 제거한다. */
                CharacterEntity myCharacter = EntityManager.Instance.MyCharacter;
                if(myCharacter != null)
                {
                    //EntityManager.Instance.RemoveEntity(myCharacter.netId.Value);
                    EntityManager.Instance.DestroyEntity(myCharacter);
                }
            }

            if(netClient.isConnected)
                netClient.Disconnect();

            netClient.Shutdown();
        }

        public NetworkClient NetClient
        {
            get
            {
                return netClient;
            }
        }

        #region HLAPI Events
        public void OnError(NetworkMessage networkMessage)
        {
            string logText = "Error occured. : ";
            logText += "\nMessage Type : " + networkMessage.msgType;
            Debug.Log(logText);

            networkManager.message = logText;
        }

        public void OnConnected(NetworkMessage networkMessage)
        {
            if(!ClientScene.ready)
                ClientScene.Ready(netClient.connection);

            if(!ClientScene.ready)
                return;

            GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.GamePlay);

            #region Log
            string logText = string.Format("Connected to server. Address = {0})", networkMessage.conn.address);
            logText += "\n[Connection] : " + networkMessage.conn;
            logText += "\n[Message Type] : " + networkMessage.msgType;
            Debug.Log(logText);

            networkManager.message = logText;
            #endregion
        }

        public void OnDisconnected(NetworkMessage networkMessage)
        {
            NetworkManager.Instance.Terminate();
            GameSceneManager.Instance.ChangeScene(GameScene.eSceneType.Intro);

            #region Log
            string logText = string.Format("Disconnected from server. Address = {0})", networkMessage.conn.address);
            logText += "\n[Connection] : " + networkMessage.conn;
            logText += "\n[Message Type] : " + networkMessage.msgType;
            Debug.Log(logText);

            networkManager.message = logText;
            #endregion
        }

        public void OnReady(NetworkMessage networkMessage)
        {
            #region Log
            string logText = "ready to send message to server.";
            logText += "\n[Connection] : " + networkMessage.conn;
            logText += "\n[Message Type] : " + networkMessage.msgType;
            Debug.Log(logText);

            networkManager.message = logText;
            #endregion
        }

        public void OnNotReady(NetworkMessage networkMessage)
        {
            #region Log
            string logText = "Not ready to send message to server.";
            logText += "\n[Connection] : " + networkMessage.conn;
            logText += "\n[Message Type] : " + networkMessage.msgType;
            Debug.LogError(logText);

            networkManager.message = logText;
            #endregion
        }
        #endregion

        #region Custom Events
        private void SendCharacterMoveTo(uint characterId, Vector3 position)
        {
            NetworkMessageCharacterMoveTo networkMessage = new NetworkMessageCharacterMoveTo(characterId, position);
            netClient.SendByChannel(NetworkMessageCharacterMoveTo.Code, networkMessage, Channels.DefaultUnreliable);
        }

        private void OnCharacterMoveTo(NetworkMessage networkMessage)
        {
            Debug.Assert(networkMessage != null);
            if(networkMessage == null)
                return;

            NetworkMessageCharacterMoveTo detailMessage = networkMessage.ReadMessage<NetworkMessageCharacterMoveTo>();
            Debug.Assert(detailMessage != null);
            if(detailMessage == null)
                return;

            //EntityManager.detailMessage.characterId
        }
        #endregion

        private bool SetupClient()
        {
            if(netClient == null)
            {
                netClient = new NetworkClient();
                
                // UNET 내장 네트워크 이벤트 메시지 등록
                netClient.RegisterHandler(MsgType.Error, OnError);
                netClient.RegisterHandler(MsgType.Connect, OnConnected);
                netClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
                netClient.RegisterHandler(MsgType.Ready, OnReady);
                netClient.RegisterHandler(MsgType.NotReady, OnNotReady);

                // 사용자 정의 네트워크 이벤트 메시지 등록
                netClient.RegisterHandler(NetworkMessageCharacterMoveTo.Code, OnCharacterMoveTo);
            }

            try
            {
                netClient.Connect(networkManager.address, networkManager.port);
            }
            catch(System.Exception exception)
            {
                Debug.LogException(exception);
                return false;
            }

            return true;
        }
    }
}

