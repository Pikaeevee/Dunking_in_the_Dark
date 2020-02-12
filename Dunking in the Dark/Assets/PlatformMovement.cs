using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private Vector2[] points = new Vector2[2];
    private Rigidbody2D rig;
    [SerializeField] private float speed;
    private bool movingToFirst = true;
    
    
    void OnValidate()
    {
        if (points.Length != 2)
        {
            Debug.LogWarning("Don't change my array size!");
            Array.Resize(ref points, 2);
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        StartCoroutine(movePositions(rig.position, points[0]));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator movePositions(Vector2 pos1, Vector2 pos2)
    {
        float distance = Vector2.Distance(pos1, pos2);
        float time = distance / speed;
        float counter = 0;

        //Pause 1 frame, so timer works correctly
        yield return null;
        while (counter < time)
        {
            print("Taking one frame step! Time is " + counter);
            counter += Time.deltaTime;
            if (counter > time)
            {
                counter = time;
            }

            rig.MovePosition(Vector2.Lerp(pos1, pos2, counter/time));
            yield return null;
        }

        if (movingToFirst)
        {
            StartCoroutine(movePositions(points[1], points[0]));
        }
        else
        {
            StartCoroutine(movePositions(points[0], points[1]));
        }

        movingToFirst = !movingToFirst;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(new Vector3(points[0].x, points[0].y, 0), .2f);
        Gizmos.DrawWireSphere(new Vector3(points[1].x, points[1].y, 0), .2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(points[0].x, points[0].y, 0), new Vector3(points[1].x, points[1].y, 0));
    }
}
