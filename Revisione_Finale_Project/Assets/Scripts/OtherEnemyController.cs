using System;
using System.Collections;
using AI_Perception.Interfaces;
using AI_Perception.Senses;
using AI_Perception.Stimuli.Sources;
using UnityEngine;
using UnityEngine.AI;

public class OtherEnemyController : MonoBehaviour, IPerceptionReceiver
{
	public NavMeshAgent agent;
	public Animator animator;
	public bool notified;

	public Transform point;

	public bool isGoing;

	private void Awake()
	{
		animator.SetBool("isSeated", true);
	}

	public void OnSenseTriggered(IStimulusSource stimulusSource, Sense sense)
	{
		if (!notified)
		{
			if (sense.GetType() == typeof(FactionNotifier))
			{
				OnFactionNotification();
				notified = true;
				return;
			}
		}
		
		GameManager.Instance.Lose(sense);
	}

	private void Update()
	{
		if (isGoing)
		{
			if (!agent.pathPending)
			{
				if (agent.remainingDistance <= agent.stoppingDistance)
				{
					if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
					{
						animator.SetBool("isRunning", false);
						isGoing = false;
					}
				}
			}
		}
	}

	private void OnFactionNotification()
	{
		animator.SetBool("isSeated", false);
		StartCoroutine(GoToDestinationCoroutine());
	}

	private IEnumerator GoToDestinationCoroutine()
	{
		yield return new WaitForSeconds(.25f);
		animator.SetBool("isRunning", true);
		agent.SetDestination(point.position);
		isGoing = true;
	}
}
