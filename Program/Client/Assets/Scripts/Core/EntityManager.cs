using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityNet.Client.Core;

using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public const int NpcIdSeed = 1000;


    private Dictionary<uint, CharacterEntity> entities = new Dictionary<uint, CharacterEntity>();

    private IdGenerator npcIdGenerator = new IdGenerator(NpcIdSeed);

    private CharacterEntity myCharacter;

    public static EntityManager Instance
    {
        get
        {
            return GameManager.Singleton.entityManager;
        }
    }

    public void Clear()
    {
        foreach(var pair in entities)
        {
            CharacterEntity entity = pair.Value;
            if(entity == null)
                continue;

            DestroyEntity(entity);
        }
        entities.Clear();
    }

    public bool DestroyEntity(CharacterEntity entity)
    {
        if(entity == null)
            return false;

        Destroy(entity.gameObject);
        entity = null;

        return true;
    }

    public CharacterEntity CreatePlayerCharacter(GameObject prefab, int ownerNetConnectionId, short playerControllId)
    {
        return CreatePlayerCharacter(prefab,
                                     ownerNetConnectionId,
                                     playerControllId,
                                     Vector3.zero,
                                     Quaternion.identity);
    }

    public CharacterEntity CreatePlayerCharacter(GameObject prefab,
                                                 int ownerNetConnectionId,
                                                 short playerControllId,
                                                 Vector3 position,
                                                 Quaternion rotation)
    {
        GameObject myPlayerObject = Object.Instantiate<GameObject>(prefab);
        myPlayerObject.name = prefab.name;
        myPlayerObject.transform.position = prefab.transform.position;

        CharacterEntity entity = myPlayerObject.transform.GetComponent<CharacterEntity>();
        entity.playerControllId = playerControllId;
        entity.transform.position = position;
        entity.transform.rotation = rotation;

        return entity;
    }

    public uint GenerateNpcId()
    {
        uint id;
        return (npcIdGenerator.Generate(out id) == true) ? id : 0u;
    }

    public CharacterEntity CreateNonPlayerCharacter(GameObject prefab)
    {
        return CreateNonPlayerCharacter(prefab, Vector3.zero, Quaternion.identity);
    }

    public CharacterEntity CreateNonPlayerCharacter(GameObject prefab,
                                                    Vector3 position,
                                                    Quaternion rotation)
    {
        GameObject myPlayerObject = Object.Instantiate<GameObject>(prefab);
        myPlayerObject.name = prefab.name;
        myPlayerObject.transform.position = prefab.transform.position;

        CharacterEntity entity = myPlayerObject.transform.GetComponent<CharacterEntity>();
        entity.transform.position = position;
        entity.transform.rotation = rotation;

        return entity;
    }

    public void AddEntity(uint id, CharacterEntity entity)
    {
        if(entity == null)
            return;

        entities.Add(id, entity);

        entity.transform.parent = transform;
    }

    public CharacterEntity RemoveEntity(uint id)
    {
        if(!ExistEntity)
            return null;

        CharacterEntity player = GetEntity(id);
        if(player == null)
            return null;

        entities.Remove(id);

        return player;
    }

    public CharacterEntity GetEntity(uint id)
    {
        CharacterEntity data;
        return entities.TryGetValue(id, out data) ? data : null;
    }

    public CharacterEntity[] GetMyControlledEntities(int netConnectionId)
    {
        if(ExistEntity == false)
            return null;

        List<CharacterEntity> myControlledEntities = new List<CharacterEntity>(EntityCount);
        foreach(var pair in entities)
        {
            CharacterEntity entity = pair.Value;
            if(entity == null)
                continue;

            if(entity.ownerNetConnectionId == netConnectionId)
                myControlledEntities.Add(entity);
        }

        return (myControlledEntities.Count > 0) ? myControlledEntities.ToArray() : null;
    }

    public CharacterEntity[] Entities
    {
        get
        {
            return (EntityCount > 0) ? entities.Values.ToArray() : null;
        }
    }

    public int EntityCount
    {
        get
        {
            return entities.Count;
        }
    }

    public bool ExistEntity
    {
        get
        {
            return (EntityCount > 0) ? true : false;
        }
    }

    public CharacterEntity[] Players
    {
        get
        {
            List<CharacterEntity> players = new List<CharacterEntity>(EntityCount);
            foreach(var pair in entities)
            {
                CharacterEntity entity = pair.Value;
                if(entity == null)
                    continue;

                if(entity.property.isPlayer == true)
                    players.Add(entity);
            }

            return (players.Count > 0) ? players.ToArray() : null;
        }
    }

    public int PlayerCount
    {
        get
        {
            int playerCount = 0;
            foreach(var pair in entities)
            {
                CharacterEntity entity = pair.Value;
                if(entity == null)
                    continue;

                if(entity.property.isPlayer == true)
                    ++playerCount;
            }

            return playerCount;
        }
    }

    public CharacterEntity MyCharacter
    {
        get
        {
            return myCharacter;
        }
        set
        {
            myCharacter = value;
        }
    }
}
