using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    private IEnumerator coroutine;
    private bool stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        coroutine = SpawnEnemyRoutine(5f);
        StartCoroutine(coroutine);
        //si può anche fare StartCoroutine("SpawnRoutine");
        StartCoroutine(SpawnPowerupRoutine());
    }

    //spawn game obj every 5 seconds

    IEnumerator SpawnEnemyRoutine(float waitTime)
    {
        yield return new WaitForSeconds(3.0f);
        while (stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-9.4f, 9.4f), 6.4f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            Instantiate(_powerups[Random.Range(0, 3)], new Vector3(Random.Range(-7f, 7f), 7f, 0), Quaternion.identity);
        }
    }

    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }
}
