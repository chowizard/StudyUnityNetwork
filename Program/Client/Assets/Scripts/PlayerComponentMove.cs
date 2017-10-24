using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class PlayerComponentMove : NetworkBehaviour
{
    public int id;

    public float velocity;

    public GameObject bulletObject;

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    // Use this for initialization
    private void Start()
    {
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    private void Update()
    {
        if(!isLocalPlayer)
            return;

        CheckUpdateMove();
        CheckUpdateFire();
    }

    private void CheckUpdateMove()
    {
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");

        float moveDeltaHorizontal = axisX * velocity * Time.deltaTime;
        float moveDeltaVertical = axisZ * velocity * Time.deltaTime;

        if((moveDeltaHorizontal != 0.0f) || (moveDeltaVertical != 0.0f))
            CmdMove(moveDeltaHorizontal, moveDeltaVertical);
    }

    private void CheckUpdateFire()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            CmdFire();
    }

    [Command]
    private void CmdMove(float moveDeltaHorizontal, float moveDeltaVertical)
    {
        transform.Translate(moveDeltaHorizontal, transform.position.y, moveDeltaVertical);
    }

    [Command]
    private void CmdFire()
    {
        Vector3 spawnPosition = transform.position - transform.forward;
        GameObject bullet = Instantiate<GameObject>(bulletObject, spawnPosition, Quaternion.identity);

        BulletComponent bulletComponent = bullet.GetComponent<BulletComponent>();
        bulletComponent.velocity = -transform.forward;
        bulletComponent.speed = 4.0f;

        Object.Destroy(bullet, 2.0f);
    }
}
