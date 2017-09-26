using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class PlayerComponentMove : NetworkBehaviour
{
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

        float positionX = axisX * velocity * Time.deltaTime;
        float positionZ = axisZ * velocity * Time.deltaTime;

        transform.Translate(axisX, transform.position.y, axisZ);
    }

    private void CheckUpdateFire()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Fire();
    }

    private void Fire()
    {
        Vector3 spawnPosition = transform.position - transform.forward;
        GameObject bullet = Instantiate<GameObject>(bulletObject, spawnPosition, Quaternion.identity);

        BulletComponent bulletComponent = bullet.GetComponent<BulletComponent>();
        bulletComponent.velocity = -transform.forward;
        bulletComponent.speed = 4.0f;

        Object.Destroy(bullet, 2.0f);
    }
}
