using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingElement : MonoBehaviour
{
    public Vector3 windDirection = Vector2.right;
    public float windSpeed = 0.1f;
    public float minSpeed = 0.01f;
    public float resetRadius = 5;
    Transform[] clouds;
    float[] speeds;


    void Start()
    {
        clouds = new Transform[transform.childCount];
        speeds = new float[transform.childCount];
        for (var i = 0; i < transform.childCount; i++)
        {
            clouds[i] = transform.GetChild(i);
            speeds[i] = Random.value;
        }
        resetRadius = 100f;
    }

    void Update()
    {
        var r2 = resetRadius * resetRadius;
        for (var i = 0; i < speeds.Length; i++)
        {
            var cloud = clouds[i];
            var speed = Mathf.Lerp(minSpeed, windSpeed, speeds[i]);
            cloud.localPosition += 0.1f * windDirection * speed;
            if ((cloud.localPosition.sqrMagnitude > r2))
            {
                cloud.position = -cloud.position;
            }
        }
    }
}
