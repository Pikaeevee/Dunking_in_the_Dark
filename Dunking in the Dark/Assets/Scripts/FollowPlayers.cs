using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayers : MonoBehaviour
{
    [SerializeField] private float zOffset = -2;
    [SerializeField] private float sizeOffset = 4;
    [SerializeField] private float sizeScale = .5f;
    [SerializeField] private float lerpAmount = 1f;
    private GameObject p1;
    private GameObject p2;
    private Vector3 midpoint = Vector3.zero;
    private float dist = 5;
    private Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        p1 = GameObject.FindGameObjectWithTag("Player1");
        p2 = GameObject.FindGameObjectWithTag("Player2");
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Caclulate where we need to be, and how big
        midpoint = ((p1.transform.position.normalized + p2.transform.position) / 2) + new Vector3(0, 0, zOffset);
        dist = sizeScale * Vector3.Distance(p1.transform.position, p2.transform.position) + sizeOffset;

        //Set the those sizes and positions
        cam.orthographicSize = dist;
        transform.position = Vector3.Lerp(transform.position, midpoint, lerpAmount);
    }
}
