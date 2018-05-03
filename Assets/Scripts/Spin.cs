using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{

    public Vector3 axis;
    public float rotRpm;

    private void Start()
    {
        rotRpm = Random.Range(1, 10);
        axis = new Vector3(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
    }
    void Update()
    {
        float angle = rotRpm * Time.time / 60 * 360;
        gameObject.transform.localRotation = Quaternion.AngleAxis(angle, axis);


    }
}
