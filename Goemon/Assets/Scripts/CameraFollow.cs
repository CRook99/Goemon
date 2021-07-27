using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    private Vector3 desired;
    private Vector3 smoothed;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(-2f, 2f, -2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        desired = player.transform.position;
        smoothed = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        transform.localPosition = smoothed;
        transform.LookAt(enemy.transform);
    }
}
