using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private Dictionary<int, PlayerCharacter> players = new Dictionary<int, PlayerCharacter>();

    public PlayerCharacter CreatePlayer(GameObject prefab)
    {
        GameObject myPlayerObject = Object.Instantiate<GameObject>(prefab);
        myPlayerObject.name = prefab.name;
        myPlayerObject.transform.position = prefab.transform.position;
        myPlayerObject.transform.parent = transform;

        return myPlayerObject.transform.GetComponent<PlayerCharacter>();
    }

    public void AddPlayer(int id, PlayerCharacter player)
    {
        if(player == null)
            return;

        players.Add(id, player);
    }

    public void RemovePlayer(int id)
    {
        if(!ExistPlayer)
            return;

        PlayerCharacter player = GetPlayer(id);
        if(player == null)
            return;

        Destroy(player);
        player = null;

        players.Remove(id);
    }

    public PlayerCharacter GetPlayer(int id)
    {
        PlayerCharacter data;
        return players.TryGetValue(id, out data) ? data : null;
    }

    public PlayerCharacter[] Players
    {
        get
        {
            return (PlayerCount > 0) ? players.Values.ToArray() : null;
        }
    }

    public int PlayerCount
    {
        get
        {
            return players.Count;
        }
    }

    public bool ExistPlayer
    {
        get
        {
            return (PlayerCount > 0) ? true : false;
        }
    }

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }
}
