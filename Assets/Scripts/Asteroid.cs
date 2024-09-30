using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateStep = 6f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private Spawn_Manager spawnManager;

    void Start()
    {
        this.transform.position = new Vector3(0f, 3.5f, 0);
        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.forward * _rotateStep * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.2f);
        }
    }
}
