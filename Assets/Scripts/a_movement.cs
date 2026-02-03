using UnityEngine;

public class a_movement : MonoBehaviour
{

    Rigidbody body;
    public float speed = 1;
    public float forward_force = 20;
    public float backward_force = -15;
    public Vector3 rotation_velocity = new Vector3(0, 75, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.linearDamping = 5;
        body.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            body.AddForce(transform.forward * forward_force * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            body.AddForce(transform.forward * backward_force * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Quaternion deltaRotation = Quaternion.Euler(rotation_velocity * Time.fixedDeltaTime);
            body.MoveRotation(body.rotation * deltaRotation);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Quaternion deltaRotation = Quaternion.Euler((-1) * rotation_velocity * Time.fixedDeltaTime);
            body.MoveRotation(body.rotation * deltaRotation);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 2;
        }
        else
        {
            speed = 1;
        }
    }
}
