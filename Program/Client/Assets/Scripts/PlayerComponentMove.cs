using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class PlayerComponentMove : NetworkBehaviour
{
    public int id;

    public float velocity;

    public GameObject bulletObject;


    [SyncVar]
    public Vector3 currentPosition;

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
        CheckUpdateMove();
        CheckUpdateFire();
    }

    private void CheckUpdateMove()
    {
        if(isServer)
        {
            //if(!IsInvoking("RpcUpdatePosition"))
            //    Invoke("RpcUpdatePosition", 0.1f);
        }

        if(isClient)
        {
            if(isLocalPlayer)
            {
                float axisX = Input.GetAxis("Horizontal");
                float axisZ = Input.GetAxis("Vertical");

                //if((axisX != 0.0f) || (axisZ != 0.0f))
                //    Debug.Log(string.Format("axisX = {0}    axisZ = {1}", axisX, axisZ));

                float moveDeltaHorizontal = axisX * velocity * Time.deltaTime;
                float moveDeltaVertical = axisZ * velocity * Time.deltaTime;

                if((moveDeltaHorizontal != 0.0f) || (moveDeltaVertical != 0.0f))
                {
                    //Debug.Log(string.Format("moveSpeedX = {0}    moveSpeedZ = {1}", moveDeltaHorizontal, moveDeltaVertical));
                    Move(moveDeltaHorizontal, moveDeltaVertical);
                }

                //if(!IsInvoking("CmdUpdatePosition"))
                //    Invoke("CmdUpdatePosition", 0.1f);
            }
        }
    }

    private void CheckUpdateFire()
    {
        if(!isLocalPlayer)
            return;

        if(Input.GetKeyDown(KeyCode.Space))
            CmdFire();
    }

    private void Move(float moveDeltaHorizontal, float moveDeltaVertical)
    {
        transform.Translate(moveDeltaHorizontal, transform.position.y, moveDeltaVertical);
    }

    [Command]
    private void CmdUpdatePosition()
    {
        if(currentPosition != transform.position)
            Debug.Log("Update position : " + transform.position);

        currentPosition = transform.position;
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

    [ClientRpc]
    private void RpcUpdatePosition()
    {
        transform.position = currentPosition;
    }
}
