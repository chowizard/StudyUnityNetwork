using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class EntityManager : MonoBehaviour
{
    private Dictionary<int, PlayerComponentMove> players = new Dictionary<int, PlayerComponentMove>();

    public PlayerComponentMove CreatePlayer(GameObject prefab)
    {
        GameObject myPlayerObject = Object.Instantiate<GameObject>(prefab);
        myPlayerObject.name = prefab.name;
        myPlayerObject.transform.position = prefab.transform.position;
        myPlayerObject.transform.parent = transform;

        PlayerComponentMove player = myPlayerObject.transform.GetComponent<PlayerComponentMove>();

        AddPlayer(player.id, player);

        return player;
    }

    public void AddPlayer(int id, PlayerComponentMove player)
    {
        if(player == null)
            return;

        players.Add(id, player);
    }

    public void RemovePlayer(int id)
    {
        if(!ExistPlayer)
            return;

        PlayerComponentMove player = GetPlayer(id);
        if(player == null)
            return;

        Destroy(player);
        player = null;

        players.Remove(id);
    }

    public PlayerComponentMove GetPlayer(int id)
    {
        PlayerComponentMove data;
        return players.TryGetValue(id, out data) ? data : null;
    }

    public PlayerComponentMove[] Players
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
