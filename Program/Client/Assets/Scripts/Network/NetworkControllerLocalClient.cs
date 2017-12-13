using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using Chowizard.UnityNetwork.Client.Core;

namespace Chowizard.UnityNetwork.Client.Network
{
    public sealed class NetworkControllerLocalClient
    {
        private NetworkManager networkManager;
        private GameManager mainScene;

        private NetworkClient netClient;

        public NetworkControllerLocalClient(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
            mainScene = networkManager.transform.parent.GetComponent<GameManager>();
        }

        public void Setup()
        {
            if(netClient == null)
            {
                netClient = ClientScene.ConnectLocalServer();
                netClient.RegisterHandler(MsgType.Error, OnError);
                netClient.RegisterHandler(MsgType.Connect, OnConnected);
                netClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
                netClient.RegisterHandler(MsgType.Ready, OnReady);
                netClient.RegisterHandler(MsgType.NotReady, OnNotReady);
            }
        }

        public void Terminate()
        {
            if((netClient != null) && netClient.isConnected)
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
            string message = "Error occured. : ";
            message += "\nMessage Type : " + networkMessage.msgType;
            Debug.Log(message);

            networkManager.message = message;
        }

        public void OnConnected(NetworkMessage networkMessage)
        {
            if(!ClientScene.ready)
                ClientScene.Ready(netClient.connection);

            string message = "Connected to server.";
            message += "\nMessage Type : " + networkMessage.msgType;
            Debug.Log(message);

            networkManager.message = message;
        }

        public void OnDisconnected(NetworkMessage networkMessage)
        {
            string message = "Disconnected from server.";
            message += "\nMessage Type : " + networkMessage.msgType;
            Debug.Log(message);

            networkManager.message = message;
        }

        public void OnReady(NetworkMessage networkMessage)
        {
            string message = "ready to send message to server.";
            message += "\nMessage Type : " + networkMessage.msgType;
            Debug.Log(message);

            networkManager.message = message;
        }

        public void OnNotReady(NetworkMessage networkMessage)
        {
            string message = "Not ready to send message to server.";
            message += "\nMessage Type : " + networkMessage.msgType;
            Debug.LogError(message);

            networkManager.message = message;
        }
        #endregion

        #region Custom Events
        #endregion
    }
}

