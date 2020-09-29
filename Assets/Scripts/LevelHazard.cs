using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHazard : MonoBehaviour
{
    [SerializeField] Health _player = null;
    [SerializeField] AudioSource death = null;
    //[SerializeField] ParticleSystem particles = null;

    public Rigidbody enemy;

    public float speed = 5f;
    public float rotateSpeed = 200f;

    public GameObject player;

    void Update()
    {
        transform.LookAt(player.transform);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _player.TakeDamage(20);
            //particles.Play();
            death.Play();
            Invoke("Die", .5f);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
