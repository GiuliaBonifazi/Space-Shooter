using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const int MAX_HP = 3;

    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject[] _engineDamage;
    [SerializeField]
    private AudioClip laserSound;
    [SerializeField]
    private AudioClip powerUpClip;

    private float negxLimit = -10.6f;
    private float posxLimit = 10.6f;
    private float negyLimit = -4.7f;
    private float posyLimit = 0;

    private bool hasTripleShot = false;
    private bool isShieldActive = false;
    private float speedBoostMultiplier = 2f;

    private float _fireRate = 0.15f;
    private float nextFire = -1f;

    private Spawn_Manager _spawnManager;
    private UI_Manager UImanager;
    private IEnumerator tripleShotCoroutine;
    private AudioSource audioComp;

    private int _hp;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        _hp = MAX_HP;
        transform.position = new Vector3(0, -2.4f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();
        UImanager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        UImanager.UpdateLives(_hp);
        audioComp = this.GetComponent<AudioSource>();
        if(_spawnManager == null)
        {
            Debug.Log("spawn manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && nextFire < Time.time)
        {
            ShootLaser();
        }
    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        //transform.Translate(_speed * Time.deltaTime * Vector3.right * horizontalInput);
        //transform.Translate( _speed * Time.deltaTime * verticalInput * Vector3.up);

        //transform.Translate(_speed * Time.deltaTime * new Vector3(horizontalInput, verticalInput, 0));

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
            transform.Translate(_speed * Time.deltaTime * direction);
        

        //blocco verticale
        /*if (transform.position.y >= _posyLimit)
        {
            transform.position = new Vector3(transform.position.x, _posyLimit, 0);
        }
        else if (transform.position.y <= _negyLimit)
        {
            transform.position = new Vector3(transform.position.x, _negyLimit, 0);
        }*/

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, negyLimit, posyLimit), 0);

        //warp laterale
        if (transform.position.x > posxLimit)
        {
            transform.position = new Vector3(negxLimit, transform.position.y, 0);
        }
        else if (transform.position.x < negxLimit)
        {
            transform.position = new Vector3(posxLimit, transform.position.y, 0);
        }

    }

    void ShootLaser()
    {
        //Debug.Log("Space Key Pressed!");
        
        nextFire = Time.time + _fireRate;
        if(hasTripleShot == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1.05f, 0);
            Instantiate(_laserPrefab, pos, Quaternion.identity); //default rotation
        }
        audioComp.clip = laserSound;
        audioComp?.Play();
        
        //potevo anche fare transform.position + new Vector3(0,0.8f,0)
    }

    public void AddScore(int toAdd)
    {
        score += toAdd;
        UImanager.UpdateScore(score);
    }

    public void ActivateTripleShot()
    {
        audioComp.clip = powerUpClip;
        audioComp.Play();
        hasTripleShot = true;
        tripleShotCoroutine = TripleShotPowerDownRoutine(5f);
        StartCoroutine(tripleShotCoroutine);
    }

    private IEnumerator TripleShotPowerDownRoutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        hasTripleShot = false;
    }

    public void ActivateSpeedBoost()
    {
        audioComp.clip = powerUpClip;
        audioComp.Play();
        _speed *= speedBoostMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    private IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _speed /= speedBoostMultiplier;
    }

    public void ActivateShield()
    {
        audioComp.clip = powerUpClip;
        audioComp.Play();
        this.transform.GetChild(0).gameObject.SetActive(true);
        isShieldActive = true;
    }

    public void Damage()
    {
        if (isShieldActive)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            isShieldActive = false;
            return;
        }
        _hp--;
        UImanager.UpdateLives(_hp);
        if(_hp == 0)
        {
            _spawnManager.OnPlayerDeath();
            UImanager.GameOver();
            Destroy(this.gameObject);
        } else
        {
            _engineDamage[MAX_HP - _hp - 1].SetActive(true);
        }
    }

}
