using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

using Chowizard.UnityNetwork.Client.Core;

namespace Chowizard.UnityNetwork.Client.Character
{
    [DisallowMultipleComponent]
    public class CharacterEntity : NetworkBehaviour
    {
        public int ownerNetConnectionId;
        public short playerControllId;

        public CharacterEntityProperty property;

        public GameObject model;
        public GameObject attachPointsRoot;

        public float moveSpeed = 10.0f;
        public float rotationSpeed;

        /* 전체 컴포넌트 목록 */
        private Dictionary<System.Type, CharacterComponent> components = new Dictionary<System.Type, CharacterComponent>();

        public ClassType AddCharacterComponent<ClassType>() where ClassType : CharacterComponent
        {
            ClassType component = gameObject.AddComponent<ClassType>();
            if(component == null)
                return null;

            component.owner = this;

            Debug.Assert(components.ContainsKey(typeof(ClassType)) == false);
            AddCharacterComponent(component);

            return component;
        }

        public void AddCharacterComponent<ClassType>(ClassType component) where ClassType : CharacterComponent
        {
            System.Type type = GetTypeForRegister<ClassType>();
            Debug.Assert(type != null);
            if(type == null)
                return;

            components.Add(type, component);
        }

        public bool RemoveCharacterComponent<ClassType>() where ClassType : CharacterComponent
        {
            ClassType component = gameObject.GetComponent<ClassType>();
            if(component == null)
                return true;

            Destroy(component);

            return components.Remove(typeof(ClassType));
        }

        public ClassType GetCharacterComponent<ClassType>() where ClassType : CharacterComponent
        {
            System.Type type = GetTypeForRegister<ClassType>();
            Debug.Assert(type != null);
            if(type == null)
                return null;

            CharacterComponent data;
            return components.TryGetValue(type, out data) ? data as ClassType : null;
        }

        public bool ActivateCharacterComponent<ClassType>() where ClassType : CharacterComponent
        {
            return false;
        }

        public bool DeactivateCharacterComponent<ClassType>() where ClassType : CharacterComponent
        {
            return false;
        }

        public bool ActivateAllCharacterComponent<ClassType>() where ClassType : CharacterComponent
        {
            return false;
        }

        public bool DeactivateAllCharacterComponent<ClassType>() where ClassType : CharacterComponent
        {
            return false;
        }

        public CharacterComponent[] Components
        {
            get
            {
                return (components.Count > 0) ? components.Values.ToArray() : null;
            }
        }

        public override void OnNetworkDestroy()
        {
            //Debug.Log("OnNetworkDestroy : NetID = " + netId);

            base.OnNetworkDestroy();
        }

        public override void OnStartLocalPlayer()
        {
            //Debug.Log("OnStartLocalPlayer : NetID = " + netId);

            base.OnStartLocalPlayer();

            EntityManager.Instance.MyCharacter = this;

            GameManager.Singleton.mainCamera.followTarget = gameObject;
            GameManager.Singleton.mainCamera.isFollowTarget = true;

            Transform bodyTransform = model.transform.Find("Body");
            Debug.Assert(bodyTransform != null);
            if(bodyTransform != null)
                bodyTransform.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        public override void OnStartServer()
        {
            //Debug.Log("OnStartServer : NetID = " + netId);

            EntityManager.Instance.AddEntity(netId.Value, this);

            base.OnStartServer();
        }

        public override void OnStartClient()
        {
            //Debug.Log("OnStartClient : NetID = " + netId);

            EntityManager.Instance.AddEntity(netId.Value, this);

            base.OnStartClient();
        }

        public override void PreStartClient()
        {
            //Debug.Log("PreStartClient : NetID = " + netId);

            base.PreStartClient();
        }

        private void Awake()
        {
            //property = new CharacterEntityProperty();
        }

        // Use this for initialization
        private void Start()
        {
            Debug.Assert(model != null);
            Debug.Assert(attachPointsRoot != null);
        }

        private void OnDrawGizmosSelected()
        {
            if(property.isPlayer == true)
            {
                Gizmos.color = new Color(0.0f, 8.0f, 0.1f);
                Gizmos.DrawWireSphere(transform.position, Network.NetworkManager.Instance.enableToSyncDistance);
            }
        }

        private void OnDestroy()
        {
            EntityManager.Instance.RemoveEntity(netId.Value);
        }

        private System.Type GetTypeForRegister<ClassType>() where ClassType : CharacterComponent
        {
            System.Type type = typeof(ClassType);
            while((type.BaseType != null) && (type.BaseType != typeof(CharacterComponent)))
                type = type.BaseType;

            //Debug.Log("Register Type : " + type.ToString());
            return type;
        }
    }
}
