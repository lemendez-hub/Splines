using UnityEngine;

/*
 * SplineFollow rides a SplinePath. Each frame it moves along the path,
 * finds u, places itself on the curve, and faces the target or the tangent.
 */

public class SplineFollow : MonoBehaviour
{
    public SplinePath path;
    public Transform target;
    public float speed = 2.5f; // Positive world units per second in the completed exercise.
    public bool easeInOut = true;
    public float easeDistance = 15f;
    public bool travelByDistance = true;
    public bool faceTarget = true;

    float _distance;
    float _u;

    void Update()
    {
        // if (travelByDistance)
        // {
        //     // TODO: Advance distance by speed over the frame and look up u for that distance.
        //     // Stop at TotalLength.
        //     _distance += speed * Time.deltaTime;
        //     _distance = Mathf.Clamp(_distance, 0f, path.TotalLength);
        //     _u = path.ParameterAtDistance(_distance);
        // }
        if (travelByDistance)
        {
            float currentSpeed = speed;
            
            if (easeInOut)
            {
                float start = Mathf.Clamp01(_distance / (path.TotalLength * 0.2f));
                float end = Mathf.Clamp01((path.TotalLength - _distance) / (path.TotalLength * 0.2f));
                float ease = Mathf.Min(start, end);
                ease = ease * ease * (3f - 2f * ease);
                currentSpeed = speed * Mathf.Lerp(0.2f, 1f, ease);
            }
            _distance += currentSpeed * Time.deltaTime;
            Debug.Log("Camera Speed: " + currentSpeed);
            _distance = Mathf.Clamp(_distance, 0f, path.TotalLength);
            _u = path.ParameterAtDistance(_distance);
        }

        // TODO: Place this object at the path point for u. Replay should return it to the start.
        transform.position = path.SamplePoint(_u);
        Vector3 dir;

        // TODO: Look at the target if faceTarget is on, otherwise along the path tangent.
        // Use world up so the horizon stays level.
        if(faceTarget && target != null)
        {
            dir = target.position - transform.position;
        }
        else
        {
            dir = path.SampleTangent(_u);
        }
        
        if(dir.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    // public void Restart()
    // {
    //     _distance = 0f;
    //     _u = 0f;
    // }

    public void Restart()
    {
        _distance = 0f;
        _u = 0f;
        
        if (path != null)
        {
            transform.position = path.SamplePoint(0f);
        }
    }
}