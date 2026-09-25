using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureIdleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 4f;

    Transform player;

    public float deteksiRadiusArea = 18f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //tranform ke state jalan
        timer += Time.deltaTime;
        if (timer > idleTime) {
            animator.SetBool("isWalking", true);
        }
        //tranform ke state mengejar 

        float DekatDenganPemain = Vector3.Distance(player.position, animator.transform.position);
        if (DekatDenganPemain < deteksiRadiusArea) {
            animator.SetBool("isChasing", true);
        }
    }

}
