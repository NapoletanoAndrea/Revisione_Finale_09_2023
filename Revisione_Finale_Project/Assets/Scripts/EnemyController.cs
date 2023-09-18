using System;
using System.Collections;
using AI_Perception.Interfaces;
using AI_Perception.Senses;
using AI_Perception.Stimuli.Sources;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IPerceptionReceiver
{
    public Transform enemyTransform;

    public Animator animator;
    public NavMeshAgent agent;

    public Transform rotatePoint;
    public float rotateSeconds;

    public Sight sight;
    public Hearing hearing;

    private bool _triggered;
    private bool isSleeping = true;

    private void Start()
    {
        animator.SetTrigger("Sleep");
    }

    public void OnSenseTriggered(IStimulusSource stimulusSource, Sense sense)
    {
        if (!_triggered)
        {
            animator.SetTrigger("Stand Up");
            _triggered = true;
        }
        
        if (!isSleeping)
        {
            GameManager.Instance.Lose(sense);
        }

    }

    public void ChangePresets()
    {
        sight.settingsPresets.Switch("Alert");
        hearing.settingsPresets.Switch("Alert");
        isSleeping = false;
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        Vector3 direction = (rotatePoint.position - enemyTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        float count = 0;
        while (count < rotateSeconds)
        {
            count += Time.deltaTime;
            enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, lookRotation, count / rotateSeconds);
            yield return null;
        }
        enemyTransform.rotation = Quaternion.Slerp(enemyTransform.rotation, lookRotation, 1);
        animator.SetBool("isYelling", true);
    }
}
