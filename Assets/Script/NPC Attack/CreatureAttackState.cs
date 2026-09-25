using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAttackState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;

    public float jarakBerhentiSerang = 2.5f;

    public float rateSerangan = 1f;
    private float waktuSerang;
    public int kerusakanYangTerjadi;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lookatPlayer();

        if (waktuSerang <= 0)
        {
            Serangan();
            waktuSerang = 1f / rateSerangan;
        }
        else { 
            waktuSerang -= Time.deltaTime;
        }

        float DekatDenganPemain = Vector3.Distance(player.position, animator.transform.position);
        if (DekatDenganPemain > jarakBerhentiSerang)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void Serangan()
    {
        agent.gameObject.GetComponent<animal>().playAttackShound();
        PlayerState.Instance.menerimaDMG(kerusakanYangTerjadi);
    }

    private void lookatPlayer()
    {
        Vector3 arah = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(arah);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
