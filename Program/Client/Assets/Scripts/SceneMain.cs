using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class SceneMain : MonoBehaviour
{
    public bool flag;

    public int id;

    public string sceneName;

    public EntityManager entityManager;

    // Use this for initialization
    private void Start()
    {
        Debug.Log(string.Format("Scene [{0}] was started.", sceneName));

        if(entityManager == null)
        {
            Transform findTransform = transform.Find("EntityManager");
            if(findTransform != null)
                entityManager = findTransform.GetComponent<EntityManager>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
            SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Player");
        Debug.Assert(playerPrefab != null);

        GameObject myPlayer = Instantiate<GameObject>(playerPrefab);
        myPlayer.name = playerPrefab.name;
        myPlayer.transform.position = playerPrefab.transform.position;
        myPlayer.transform.parent = entityManager.transform;

        NetworkServer.Spawn(myPlayer);
    }
}
