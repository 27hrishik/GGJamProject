using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpScare : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float hp;
    public Animator animator;
    public AudioSource audioSource;

    bool ifAlert, ifDead,triggerJump;
    // Start is called before the first frame update
    void Start()
    {
        ifAlert = false;
        ifDead = false;
        triggerJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((triggerJump || ifAlert) && !ifDead)
            AttackPlayer();
    }

    void AttackPlayer()
    {
        if (!ifAlert)
            ifAlert = true;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        transform.LookAt(player.transform);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<InputController>().Damage(1f);
            animator.SetBool("attack", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Damage();
        }
        if (other.CompareTag("Player"))
        {
            triggerJump = true;
            animator.SetBool("scare", true);
            audioSource.Play();
        }
    }

    void Damage()
    {
        hp -= 100f;
        ifAlert = true;
        if (hp < 0f)
            Die();
    }

    void Die()
    {
        ifDead = true;
        animator.SetBool("dead", true);
        Destroy(this.gameObject, 3f);
    }

}
