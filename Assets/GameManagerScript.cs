using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    bool collisionHappened;
    int collisionCheckAmmount;

    [SerializeField] BallScript[] balls;
    [SerializeField] GameObject[] pendulumBobs;
    [SerializeField] int maxTimesToCheckCollision;
    [SerializeField] float bounciness = 1f;

    float debugTotalVelocity;

    void FixedUpdate()
    {
        collisionCheckAmmount = 0;
        do
        {
            CheckCollision();

        } while (collisionHappened && collisionCheckAmmount < maxTimesToCheckCollision);


        debugTotalVelocity = 0;
        //for (int i = 0; i < balls.Length; i++)
        //{
        //    debugTotalVelocity += balls[i].velocity.magnitude;
        //}
        //print("total Velocity is: " + debugTotalVelocity);
    }

    //gå igenom alla balls och kolla ifall de är mycket nära varandra 
    void CheckCollision()
    {
        collisionHappened = false;
        collisionCheckAmmount++;

        ////kolla alla bollar (balls)
        //for (int i = 0; i < pendulumBalls.Length; i++)
        //{
        //    //kolla alla bollar som inte redan har kollat alla bollar
        //    for (int j = i + 1; j < pendulumBalls.Length; j++)
        //    {
        //        if (Vector3.Distance(balls[i].transform.position, balls[j].transform.position) < balls[i].radius + balls[j].radius)
        //        {
        //            collisionHappened = true;
        //            HandleCollision(balls[i].gameObject, balls[j].gameObject);
        //        }
        //    }
        //}
        
        //kolla alla bollar (pendulums)
        for (int i = 0; i < pendulumBobs.Length; i++)
        {
            //kolla alla bollar som inte redan har kollat alla bollar
            for (int j = i + 1; j < pendulumBobs.Length; j++)
            {
                if (Vector3.Distance(pendulumBobs[i].transform.position, pendulumBobs[j].transform.position) + 0.002f < pendulumBobs[i].GetComponentInParent<PendulumScript>().bobRadius + pendulumBobs[j].GetComponentInParent<PendulumScript>().bobRadius)
                {
                    collisionHappened = true;
                    HandleCollision(pendulumBobs[i], pendulumBobs[j]);
                }
            }
        }
    }


    //flytta isär dem och gör whatever
    void HandleCollision(GameObject ball1, GameObject ball2)
    {
        //print(ball1 + " and " + ball2 + " are colliding");

        ////sepparerar bollarna (balls)
        //Vector3 collisionVector = ball1.transform.position - ball2.transform.position;
        //float penetrationDepth = ball1.GetComponent<BallScript>().radius + ball2.GetComponent<BallScript>().radius - collisionVector.magnitude;
        //collisionVector = collisionVector.normalized * penetrationDepth;
        //ball1.transform.Translate(collisionVector / 2);
        //ball2.transform.Translate(-collisionVector / 2);

        //OnCollisionBall(ball1.GetComponent<BallScript>(), ball2.GetComponent<BallScript>());

        PendulumScript ball1PendulumScript = ball1.GetComponentInParent<PendulumScript>();
        PendulumScript ball2PendulumScript = ball2.GetComponentInParent<PendulumScript>();

        //sepparerar bollarna (pendulums)
        Vector3 collisionVector = ball1.transform.position - ball2.transform.position;
        float penetrationDepth = ball1PendulumScript.bobRadius + ball2PendulumScript.bobRadius - collisionVector.magnitude;
        collisionVector = collisionVector.normalized * penetrationDepth;

        //how do dis med pendel?
        //uppdatera vinkeln baserat på bobens nya possision och sen fixa bobens possition och hoppas de inte kolliderar (lol)
        //konstigt nog är det delen jag halfassar som ger mig buggs :)
        ball1.transform.Translate(collisionVector / 2);
        ball2.transform.Translate(-collisionVector / 2);

        ball1PendulumScript.FixAngleAfterCollision();
        ball2PendulumScript.FixAngleAfterCollision();

        OnCollisionPendulum(ball1PendulumScript, ball2PendulumScript);
    }

    private void OnCollisionBall(BallScript ball1, BallScript ball2)
    {
        float totalVelocity = ball1.velocity.magnitude + ball2.velocity.magnitude;

        Vector3 velocity1old = ball1.velocity;
        Vector3 velocity2old = ball2.velocity;

        ball1.SetVelocity(velocity2old);
        ball2.SetVelocity(velocity1old);
    }

    private void OnCollisionPendulum(PendulumScript ball1, PendulumScript ball2)
    {
        //rörelsemängd och skit
        //v1n + v2n = v1o + v2o
        //v1n + v2n = -(v1o - v2o)
        //v2n = v1n + (v1o - v2o)
        //v1n + v1n + (v1o - v2o) = 2v1n + (v1o - v2o) = v1o + v2o
        //2v1n = v1o + v2o - (v1o - v2o) = v1o + v2o - v1o + v2o = v2o + v2o = 2v2o
        //v1n = v2o
        //v2n = v1n + (v1o - v2o) = v2o + (v1o - v2o) = v1o
        //v2n = v1o
        //de byter hastighet?

        //är detta fel idk :)
        //antagligen (:
        //float velocity1old = ball1.angularVelocity;
        //float velocity2old = ball2.angularVelocity;

        //ball1.angularVelocity = velocity2old;
        //ball2.angularVelocity = velocity1old;

        //print(ball1.gameObject.name);

        float velocity1old = Mathf.PI * 2 * ball1.angularVelocity;
        float velocity2old = Mathf.PI * 2 * ball2.angularVelocity;

        ball1.angularVelocity = ((1 - bounciness) * velocity1old + (1 + bounciness) * velocity2old) / 2;
        ball2.angularVelocity = ball1.angularVelocity + bounciness * (velocity1old - velocity2old);

        ball1.angularVelocity /= Mathf.PI * 2;
        ball2.angularVelocity /= Mathf.PI * 2;
    }
}
