using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;

    public float stopAttackingDistance = 1.2f;

    public float attackRate = 2f;
    public float attackTimer;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        attackController.SetAttackMaterial();
        attackController.muzzleEffect.gameObject.SetActive(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            //It should be LookAtTarget
            LookAtTarget();

            //Keep moving towards the target
            //agent.SetDestination(attackController.targetToAttack.position);

            if ( attackTimer <= 0 )
            {
                Attack();
                attackTimer = 1f / attackRate;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }


            // Should unit still attack 
            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
            if (distanceFromTarget > stopAttackingDistance || attackController.targetToAttack == null)
            {
                agent.SetDestination(animator.transform.position);
                animator.SetBool("isAttacking", false); //Move to Follow State
            }

        }
        else
        {
            animator.SetBool("isAttacking", false); //Move to Follow State
        }

    }

    private void Attack()
    {
        var damageToInflict = attackController.unitDamage;

        SoundManager.Instance.PlayInfantryAttackSound();

        var damageable = attackController.targetToAttack.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damageToInflict);
        }
    }


    private void LookAtTarget()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);
        
        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);   
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController.muzzleEffect.gameObject.SetActive(false);
    }

}
