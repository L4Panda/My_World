using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    public float gravity = -12;

    public void Attract(Transform player)
    {
        Vector3 gravityUp = (player.position - transform.position).normalized;
        Vector3 localUp = player.up;

        player.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * player.rotation;
        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, 50 * Time.deltaTime);
    }

}
