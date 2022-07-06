using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class StateController : MonoBehaviour {

	public State currentState;
	public EnemyStats enemyStats;
	public Transform eyes;
	public State remainState;
	//To raise the event
	public UnityEvent onTanksDetected;

	[HideInInspector] public NavMeshAgent navMeshAgent;
	[HideInInspector] public TankShooting tankShooting;
	[HideInInspector] public List<Transform> wayPointList;
	[HideInInspector] public int nextWayPoint;
	[HideInInspector] public Transform chaseTarget;
	[HideInInspector] public float stateTimeElapsed;

	private bool aiActive;
	//A checker to prevent spamming of alerts
	private bool alerted;


	void Awake () 
	{
		tankShooting = GetComponent<TankShooting> ();
		navMeshAgent = GetComponent<NavMeshAgent> ();
	}

	public void SetupAI(bool aiActivationFromTankManager, List<Transform> wayPointsFromTankManager)
	{
		wayPointList = wayPointsFromTankManager;
		aiActive = aiActivationFromTankManager;
		if (aiActive) 
		{
			navMeshAgent.enabled = true;
		} else 
		{
			navMeshAgent.enabled = false;
		}
	}

	public void TransitionToState(State nextState)
	{
		if (nextState == remainState) return;
		currentState = nextState;
		OnExitState();
	}

	public bool CheckIfCountDownElapsed(float duration)
	{
		stateTimeElapsed += Time.deltaTime;
		return stateTimeElapsed >= duration;
	}

	void Update()
	{
		if (!aiActive) return;

		currentState.UpdateState(this);
		//Check if the ai are currently chasing another tank
		if(currentState.name == "ChaseScanner")
		{
			Alert();
		}
	}
	
	//Helper function to raise the event
	void Alert()
	{
		if (!alerted)
		{
			alerted = true;
			onTanksDetected.Invoke();
			StartCoroutine(nextAlertCycle());
		}
	}
	
	//To provide an interval wait time to the alert function
	IEnumerator nextAlertCycle()
	{
		yield return new WaitForSeconds(4);
		alerted = false;
	}

	void OnExitState()
	{
		stateTimeElapsed = 0;
	}

	void OnDrawGizmos()
	{
		if (currentState != null && eyes != null)
		{
			Gizmos.color = currentState.sceneGizmoColor;
			Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
		}
	}

}