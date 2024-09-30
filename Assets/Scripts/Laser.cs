using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _laserSpeed = 8f;
    private float laserDeletePos = 8f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_laserSpeed * Time.deltaTime * Vector3.up);

        if (transform.position.y > laserDeletePos)
        {
            if( transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject); //Si può anche usare this.gameObject
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
        if (other.CompareTag("Asteroid"))
        {
            Destroy(this.gameObject);
        }
    }
}
