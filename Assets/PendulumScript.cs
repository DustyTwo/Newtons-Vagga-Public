using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PendulumScript : MonoBehaviour
{
    [SerializeField] GameObject origin;
    [SerializeField] GameObject line;
    [SerializeField] GameObject bob;
    [SerializeField] public float lineLength;

    [SerializeField] float gravityScale;
    //[SerializeField] float bobMas;
    [HideInInspector] public float bobRadius;

    [SerializeField] public float lineToBobAngle;
    [HideInInspector] public float angularVelocity;
    [HideInInspector] public float angularAcceleration;

    private void Awake()
    {
        origin = this.transform.Find("Origin").gameObject;
        line = this.transform.Find("Line").gameObject;
        bob = this.transform.Find("Bob").gameObject;
    }
    private void Start()
    {
        //fixar längden på linan
        lineLength = Vector3.Distance(origin.transform.position, bob.transform.position);
        line.transform.localScale = new Vector3(line.transform.localScale.x, lineLength * 0.5f, line.transform.localScale.z);
    }

    void FixedUpdate()
    {
        CalculateNewAngle();
        SetBobPosition();

        CorrectLine();
    }

    public void ResetPendulum()
    {
        angularVelocity = 0f;
        angularAcceleration = 0f;
        lineToBobAngle = 0f;
    }

    void CalculateNewAngle()
    {
        //plussar på pi för att annars blir den upp och ner
        angularAcceleration = gravityScale * Mathf.Sin(lineToBobAngle + Mathf.PI) * Time.fixedDeltaTime;
        angularVelocity += angularAcceleration;
        lineToBobAngle += angularVelocity;
    }

    void SetBobPosition()
    {
        //plussar på pi för att annars blir den upp och ner
        bob.transform.position = new Vector3(origin.transform.position.x + lineLength * Mathf.Sin(lineToBobAngle + Mathf.PI), origin.transform.position.y + lineLength * Mathf.Cos(lineToBobAngle + Mathf.PI), bob.transform.position.z);
    }

    public void FixAngleAfterCollision()
    {
        //är nog fel på något sätt >:(
        //tror det måste vara denna som gör att saker teleporteras
        //float temp = lineToBobAngle;
        //lineToBobAngle = Mathf.Deg2Rad * Vector3.Angle(bob.transform.position - origin.transform.position, Vector3.down);

        //print(Mathf.Abs(temp - lineToBobAngle));

        //"Har du sätt uppgiftsbeskrvningen? Hårdkodning är tillåtet :)"
        lineToBobAngle = 0f;

        SetBobPosition();
        CorrectLine();
    }

    void CorrectLine()
    {
        ////fixar längden på linan (ska bara göras i start men har här för debug)
        //lineLength = Vector3.Distance(origin.transform.position, bob.transform.position);
        //line.transform.localScale = new Vector3(line.transform.localScale.x, lineLength * 0.5f, line.transform.localScale.z);

        //fixar linans possision
        line.transform.position = origin.transform.position + (bob.transform.position - origin.transform.position) / 2;

        //fixar linans rottation (funkar bara i 2d)
        //line.transform.rotation = Quaternion.LookRotation(Vector3.Cross(bob.transform.position - origin.transform.position, Vector3.forward), Vector3.up);

        //fixar linans rottation (funkar i 3d) (jag är jesus)
        line.transform.rotation = Quaternion.LookRotation(Vector3.Cross(bob.transform.position - origin.transform.position, Vector3.Cross(bob.transform.position - origin.transform.position, Vector3.up)), Vector3.up);
    }
}
