using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    //all this shit is old and bad
    const float g = 9.82f;
    [SerializeField] float debugG = 9.82f;
    [SerializeField] float debugInitSpeed;
    [SerializeField] float debugCenterForce;
    [SerializeField] float debugCenterAmp;

    public Vector3 velocity;
    [HideInInspector]
    public Vector3 Momentum
    {
        get
        { return velocity * mass; }
    }

    [SerializeField] float mass = 1;

    [SerializeField] float Radius = 0.5f;
    [HideInInspector] public float radius;
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius + 0.05f);
    }
    private void OnValidate()
    {
        radius = Radius * (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3;

    }
    private void Awake()
    {

    }

    void Start()
    {
        AddForce(Vector3.right * debugInitSpeed);
    }

    void FixedUpdate()
    {
        //GravityFall();

        CenterBall();

        UpdatePosition();
    }


    void UpdatePosition()
    {
        this.transform.position += velocity;
    }

    Vector3 GravityFall()
    {
        //Vector3 gravityAcelleration = Vector3.down * mass * g;
        //velocity += gravityAcelleration * Time.fixedDeltaTime;

        //AddForce(Vector3.down * mass * g * Time.fixedDeltaTime);
        //AddForce(Vector3.down * mass * debugG * Time.fixedDeltaTime);
        //print(velocity.magnitude);'

        return Vector3.down * mass * debugG * Time.fixedDeltaTime;
    }

    void CenterBall()
    {
        Vector3 centerPosV = new Vector3(0, transform.position.y, transform.position.z) - transform.position;
        AddForce(centerPosV.normalized * debugCenterForce * Mathf.Pow(centerPosV.magnitude, debugCenterAmp));
    }

    public void AddForce(Vector3 force)
    {
        velocity += force / mass;
    }
    
    public void SetVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
    }
}
