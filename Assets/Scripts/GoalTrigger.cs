using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalTrigger : MonoBehaviour
{
    public UnityEvent OnBallEnter = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            OnBallEnter.Invoke();
        }

    }
}
