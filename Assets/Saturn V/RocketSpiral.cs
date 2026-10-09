using UnityEngine;

public class RocketSpiral : MonoBehaviour
{
    public SplinePath path;

    public float radius = 25f;
    public float height = 110f;
    public int turns = 2;

    void Awake()
    {
        if (path == null)
        {
            return;
        }

        int segments = turns * 4;
        int count = segments * 3 + 1;

        Transform [] points = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            float t = i / (float)(count - 1);
            float angle = t * turns * Mathf.PI * 2f;

            int part = i % 3;
            float offset = 0f;

            if (part == 1) {
                offset = 0.55228475f / 3f;
            }
            else if (part == 2)
            {
                offset = -0.55228475f / 3f;
            }

            float a = angle + offset;

            GameObject point = new GameObject("Point " + i);
            point.transform.SetParent(transform);

            point.transform.localPosition = new Vector3(Mathf.Cos(a) * radius,t * height,Mathf.Sin(a) * radius);

            points[i] = point.transform;
        }

        path.points = points;
        path.BuildDistanceTable();
    }
}