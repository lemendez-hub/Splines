using UnityEngine;
using UnityEngine.InputSystem;

public class StretchBall : MonoBehaviour
{
    public Transform ball_transform;

    public FloatSpring x;
    public FloatSpring z;

    public float stretch_strength = 0.1f;
    public float max_stretch = 2f;

    private void Awake()
    {
        x.dampingRatio = 0.7f;
        z.dampingRatio = 0.7f;
    }

    private void Start()
    {
        x.SetValue(transform.position.x);
        z.SetValue(transform.position.z);

        x.equilibrium = transform.position.x;
        z.equilibrium = transform.position.z;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                x.equilibrium = hit.point.x;
                z.equilibrium = hit.point.z;
            }
        }
        
        x.Advance(Time.deltaTime);
        z.Advance(Time.deltaTime);

        transform.position = new Vector3(x.Value, transform.position.y, z.Value);

        Vector3 velocity = new Vector3(x.Velocity, 0f, z.Velocity);

        float speed = velocity.magnitude;

        float stretch = Mathf.Clamp(1f + speed * stretch_strength, 1f, max_stretch);

        ball_transform.localScale = new Vector3(1f / Mathf.Sqrt(stretch), 1f / Mathf.Sqrt(stretch), stretch);

        if (speed > 0.01f)
        {
            ball_transform.rotation = Quaternion.FromToRotation(Vector3.forward, velocity.normalized);
        }
    }
}