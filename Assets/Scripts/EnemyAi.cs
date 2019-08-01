using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public GameObject player;
    public float range;
    public float speed;
    public float hp;
    public Animator animator;
    public AudioSource audioSource;

    bool ifAlert,ifDead;
    // Start is called before the first frame update
    void Start()
    {
        ifAlert = false;
        ifDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector3.Distance(player.transform.position, transform.position) < range || ifAlert) && !ifDead)
            AttackPlayer();
    }

    void AttackPlayer()
    {
        if (!ifAlert)
        {
            ifAlert = true;
            audioSource.Play();
        }
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        transform.LookAt(player.transform);
        animator.SetBool("alerted", true);
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<InputController>().Damage(1f);
            animator.SetBool("attack",true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile"))
        {
            Damage();
        }
    }

    void Damage()
    {
        hp -= 100f;
        ifAlert = true;
        if (hp < 0f)
            Die();
        animator.SetTrigger("hit");
    }

    void Die()
    {
        ifDead = true;
        animator.SetBool("dead",true);
        Destroy(this.gameObject,3f);
    }
}
