using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    bool collisionHappened;
    int collisionCheckAmmount;

    //[SerializeField] BallScript[] balls;
    [SerializeField] GameObject[] pendulumBobs;
    [SerializeField] int maxTimesToCheckCollision;
    float bounciness = 1f;

    [SerializeField] Slider dropAngleSlider;
    [SerializeField] Text dropAngleText;
    [SerializeField] Slider pendulumsToDropSlider;
    [SerializeField] Text pendulumsToDropText;

    float debugTotalVelocity;
    float debugTimer;
    bool debugAnyColHappen;

    void FixedUpdate()
    {
        //debugTimer += Time.fixedDeltaTime;
        //debugAnyColHappen = false;

        collisionCheckAmmount = 0;
        do
        {
            CheckCollision();

        } while (collisionHappened && collisionCheckAmmount < maxTimesToCheckCollision);

        //totala vinkelhastigheten ökar när simuleringn körs
        //if (debugAnyColHappen)
        //{
        //    debugTotalVelocity = 0;
        //    for (int i = 0; i < pendulumBobs.Length; i++)
        //    {
        //        debugTotalVelocity += Mathf.Abs(pendulumBobs[i].GetComponentInParent<PendulumScript>().angularVelocity);
        //    }
        //    print("total Velocity is: " + debugTotalVelocity + " debug timer: " + debugTimer);
        //    debugTimer = 0f;
        //}
    }
    public void UppdateDropAngleSliderText()
    {
        dropAngleText.text = "" + dropAngleSlider.value;
    }

    public void UppdatePendulumsToDropSliderText()
    {
        pendulumsToDropText.text = "" + pendulumsToDropSlider.value;
    }

    public void ReloadScene()
    {
        for (int i = 0; i < pendulumBobs.Length; i++)
        {
            pendulumBobs[i].GetComponentInParent<PendulumScript>().ResetPendulum();
            
        }
        if (dropAngleSlider.value > 0)
        {
            for (int i = 0; i < pendulumsToDropSlider.value; i++)
            {
                pendulumBobs[i].GetComponentInParent<PendulumScript>().lineToBobAngle = dropAngleSlider.value;
            }
        }
        else
        {
            for (int i = 0; i < pendulumsToDropSlider.value; i++)
            {
                pendulumBobs[pendulumBobs.Length - i - 1].GetComponentInParent<PendulumScript>().lineToBobAngle = dropAngleSlider.value;
            }
        }
    }

    //gå igenom alla kullor och kolla ifall de är mycket nära varandra 
    void CheckCollision()
    {
        collisionHappened = false;
        collisionCheckAmmount++;

        //kolla alla kullor (pendulums)
        for (int i = 0; i < pendulumBobs.Length; i++)
        {
            //kolla alla kullor som inte redan har kollat alla kullor
            for (int j = i + 1; j < pendulumBobs.Length; j++)
            {
                if (Vector3.Distance(pendulumBobs[i].transform.position, pendulumBobs[j].transform.position) < pendulumBobs[i].GetComponentInParent<PendulumScript>().bobRadius + pendulumBobs[j].GetComponentInParent<PendulumScript>().bobRadius)
                {
                    debugAnyColHappen = true;
                    collisionHappened = true;
                    HandleCollision(pendulumBobs[i], pendulumBobs[j]);
                }
            }
        }
    }


    //flytta isär dem
    void HandleCollision(GameObject ball1, GameObject ball2)
    {
        PendulumScript ball1PendulumScript = ball1.GetComponentInParent<PendulumScript>();
        PendulumScript ball2PendulumScript = ball2.GetComponentInParent<PendulumScript>();

        //sepparerar kulorna (pendulums)
        //Vector3 collisionVector = ball1.transform.position - ball2.transform.position;
        //float penetrationDepth = ball1PendulumScript.bobRadius + ball2PendulumScript.bobRadius - collisionVector.magnitude;
        //collisionVector = collisionVector.normalized * penetrationDepth;

        //how do dis med pendel?
        //uppdatera vinkeln baserat på bobens nya possision och sen fixa bobens possition och hoppas de inte kolliderar (lol)
        //konstigt nog är det delen jag halfassar som ger mig buggs :)
        //hårdkoda att så att vinkeln sätts till 0 vid kollision (:
        //ball1.transform.Translate(collisionVector / 2);
        //ball2.transform.Translate(-collisionVector / 2);

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

        //kollar om vinkelhastigheten i systemet är densamma före och efter kollisionen
        debugTotalVelocity = 0;
        for (int i = 0; i < pendulumBobs.Length; i++)
        {
            debugTotalVelocity += Mathf.Abs(pendulumBobs[i].GetComponentInParent<PendulumScript>().angularVelocity);
        }
        print("Befor col total Velocity is: " + debugTotalVelocity);



        float velocity1old = Mathf.PI * 2 * ball1.angularVelocity;
        float velocity2old = Mathf.PI * 2 * ball2.angularVelocity;

        ball1.angularVelocity = ((1 - bounciness) * velocity1old + (1 + bounciness) * velocity2old) / 2;
        ball2.angularVelocity = ball1.angularVelocity + bounciness * (velocity1old - velocity2old);

        ball1.angularVelocity /= Mathf.PI * 2;
        ball2.angularVelocity /= Mathf.PI * 2;



        //kollar om vinkelhastigheten i systemet är densamma före och efter kollisionen
        debugTotalVelocity = 0;
        for (int i = 0; i < pendulumBobs.Length; i++)
        {
            debugTotalVelocity += Mathf.Abs(pendulumBobs[i].GetComponentInParent<PendulumScript>().angularVelocity);
        }
        print("After col total Velocity is: " + debugTotalVelocity);

    }
}
