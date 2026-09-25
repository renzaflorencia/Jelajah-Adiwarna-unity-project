using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreaturewalkState : StateMachineBehaviour
{
    float timer;
    public float waktuBerjalan = 10f;

    Transform player;
    NavMeshAgent agent;

    public float deteksiRadiusArea = 18f;
    public float kecepatanBerjalan = 2f;

    List<Transform> wayPointsList = new List<Transform>();

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //inisial
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = kecepatanBerjalan;
        timer = 0;

        //mendapatkan semua titik jalan dan bergerak ke titik jalan pertama
        GameObject waypointsCluster = animator.GetComponent<npcWaypoints>().npcWaypointsCluster;
        foreach (Transform t in waypointsCluster.transform) {
            wayPointsList.Add(t);
        }

        Vector3 posisiPertama = wayPointsList[Random.Range(0, wayPointsList.Count)].position;
        agent.SetDestination(posisiPertama);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //jika agent datang di titik jalan, bergerak ke titik jalan berikutnya
        if (agent.remainingDistance <= agent.stoppingDistance) {
            agent.SetDestination(wayPointsList[Random.Range(0, wayPointsList.Count)].position);
        }

        //tranform ke state jalan
        timer += Time.deltaTime;
        if (timer > waktuBerjalan)
        {
            animator.SetBool("isWalking", true);
        }

        //transisi ke chase state 
        float DekatDenganPemain = Vector3.Distance(player.position, animator.transform.position);
        if (DekatDenganPemain < deteksiRadiusArea)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

}
