using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaPlantBehavior : MonoBehaviour {
public float detectionRange = 5.0f;
public Transform launchPoint;
private Animator animator;
private Transform playerTransform;
public GameObject slimeBallPrefab;
public float throwForce = 10f;
private bool isOnCooldown = false;
private float nextAttackTime = 0f;
public float attackCooldown = 1f;

private void Start()
{
    //Store the Animator object and player position
    animator = GetComponent<Animator>();
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
}

private void Update()
{
    //Calculates distance between piranha and player
    float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
    bool isPlayerToTheRight = playerTransform.position.x < transform.position.x;

    if (distanceToPlayer <= detectionRange && Time.time >= nextAttackTime && !isOnCooldown && isPlayerToTheRight)
    {
        SetAttackAnimationState(true);

        if (Time.time >= nextAttackTime)
        {
            ThrowSlimeBall();
            nextAttackTime = Time.time + attackCooldown;
            isOnCooldown = true;
            StartCoroutine(CooldownRoutine());
        }
    }
    else
    {
        SetAttackAnimationState(false);
    }
}

private void SetAttackAnimationState(bool isAttacking)
{
    //Takes a boolean and sets the piranha animation based on it
    animator.SetBool("IsAttacking", isAttacking);
}

private void ThrowSlimeBall()
{
    //Handles logic for throwing slime ball
    GameObject slimeBall = Instantiate(slimeBallPrefab, launchPoint.position, Quaternion.identity);
    Rigidbody2D rb = slimeBall.GetComponent<Rigidbody2D>();
    rb.AddForce(Vector2.left * throwForce, ForceMode2D.Impulse);

    Destroy(slimeBall, 5.0f);
}

private IEnumerator CooldownRoutine()
{
    // Cooldown Routine
    yield return new WaitForSeconds(attackCooldown);
    isOnCooldown = false;
}

}

