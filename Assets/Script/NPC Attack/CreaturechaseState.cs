using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreaturechaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

    public float kecepatanMengejar = 6f;

    public float jarakBerhentiMengejar = 21;
    public float jarakSerang = 2.5f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = kecepatanMengejar;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        animator.transform.LookAt(player);

        float jarakDariPemain = Vector3.Distance(player.position, animator.transform.position);
        if (jarakDariPemain > jarakBerhentiMengejar) {
            animator.SetBool("isChasing", false);
        }
        if (jarakDariPemain < jarakSerang) {
            animator.SetBool("isAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}
