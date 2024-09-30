using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private float x_min = -9.4f;
    private float x_max = 9.4f;
    private float y_max = 6.5f;
    private Player player;
    private Animator animator;

    private const int VALUE = 10;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Enemy::player is null");
        }
        animator = this.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Enemy::animator is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePosition();
    }

    void CalculatePosition()
    {
        if (transform.position.y < -7f)
        {
            transform.position = new Vector3(Random.Range(x_min, x_max), y_max, 0);
        }

        transform.Translate((-_speed) * Time.deltaTime * Vector3.up);  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            player?.Damage();
            Die();

        }
        if (other.CompareTag("Laser"))
        {
            player.AddScore(VALUE);
            Die();
        }
    }

    private void Die()
    {
        _speed = 0;
        animator.SetTrigger("onEnemyDeath");
        this.GetComponent<AudioSource>().Play();
        Destroy(this.gameObject, 2.6f);
    }
}
