using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugawaraCamera : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        Vector3 playerPos = player.transform.position;
        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            transform.position.z
        );
    }
}
