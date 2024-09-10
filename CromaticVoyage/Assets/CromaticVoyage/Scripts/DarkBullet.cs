using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBullet : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rig;

    public float force;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rig.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerControllerV2 playerController = other.gameObject.GetComponent<PlayerControllerV2>();
            
            // Reduz a vida do player
            playerController.currentHealth -= 20;

            // Verifica se a vida do player chegou a zero ou menos
            if (playerController.currentHealth <= 0)
            {
                playerController.Die(); // Chama a função de morte do PlayerControllerV2
            }

            Destroy(gameObject); // Destrói o projétil após acertar o player
        }
    }
}