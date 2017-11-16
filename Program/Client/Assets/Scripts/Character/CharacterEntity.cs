﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

using UnityNet.Client.Character;

[DisallowMultipleComponent]
public class CharacterEntity : NetworkBehaviour
{
    public int ownerNetConnectionId;
    public short playerControllId;

    public CharacterEntityProperty property;

    public GameObject model;

    /* 최종 목료가 될 위치 */
    public Vector3 destinationPosition;
    public float moveSpeed = 10.0f;

    /* 최종 목료가 될 회전각 */
    public Quaternion destinationRotation;
    public float rotationSpeed;

    /* 전체 컴포넌트 목록 */
    private Dictionary<System.Type, CharacterComponent> components = new Dictionary<System.Type, CharacterComponent>();

    private float elapsedTimeToCheckSyncDiatance;

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
        components.Add(typeof(ClassType), component);
    }

    public bool RemoveCharacterComponent<ClassType>() where ClassType : CharacterComponent
    {
        ClassType component = gameObject.GetComponent<ClassType>();
        Destroy(component);

        return components.Remove(typeof(ClassType));
    }

    public ClassType GetCharacterComponent<ClassType>() where ClassType : CharacterComponent
    {
        CharacterComponent data;
        return components.TryGetValue(typeof(ClassType), out data) ? data as ClassType : null;
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

        model.GetComponent<MeshRenderer>().material.color = Color.red;
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

        destinationPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if(isServer == true)
        {
            if(elapsedTimeToCheckSyncDiatance >= NetworkManager.Instance.checkSyncDiatanceInterval)
            {
                UpdateSyncDistance();
                elapsedTimeToCheckSyncDiatance = 0.0f;
            }
            else
            {
                elapsedTimeToCheckSyncDiatance += Time.fixedDeltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(property.isPlayer == true)
        {
            Gizmos.color = new Color(0.0f, 8.0f, 0.1f);
            Gizmos.DrawWireSphere(transform.position, NetworkManager.Instance.enableToSyncDistance);
        }
    }

    private void OnDestroy()
    {
        EntityManager.Instance.RemoveEntity(netId.Value);
    }

    [Server]
    private void UpdateSyncDistance()
    {
        NetworkTransformSynchronizer synchronizar = GetComponent<NetworkTransformSynchronizer>();
        if(synchronizar == null)
            return;

        if(EntityManager.Instance.PlayerCount <= 0)
            return;

        bool enableToSync = false;

        CharacterEntity[] players = EntityManager.Instance.Players;
        foreach(CharacterEntity player in players)
        {
            if(player == null)
                continue;

            Vector3 myPosition = transform.position;
            Vector3 playerPosition = player.transform.position;
            float distanceSqr2 = Vector3.SqrMagnitude(playerPosition - myPosition);
            float enableToSyncDistanceSqr2 = NetworkManager.Instance.enableToSyncDistance * NetworkManager.Instance.enableToSyncDistance;

            if(distanceSqr2 <= enableToSyncDistanceSqr2)
            {
                enableToSync = true;
                break;
            }
        }

        if(synchronizar.enabled != enableToSync)
            synchronizar.enabled = enableToSync;

    }
}
