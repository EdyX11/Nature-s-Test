using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class VulcanoRock : MonoBehaviour
{


    [SerializeField] private Rock _shapePrefab;
    [SerializeField] private int  _spawnAmount = 20;
    [SerializeField] private int _defaultCap = 100;
    [SerializeField] private int _maxCap = 200;
    [SerializeField] private bool _usePool;
    

    public AudioClip impactSound; // Optional: Add an impact sound
    private ObjectPool<Rock> _pool;
    private void Start()
    {
        _pool = new ObjectPool<Rock>(() =>
        {
            return Instantiate(_shapePrefab);
        }, rock =>
        {
            rock.gameObject.SetActive(true);
        }, rock =>
        {
            rock.gameObject.SetActive(false);
        }, rock =>
        {

            Destroy(rock.gameObject);
        }, false, _defaultCap, _maxCap);





        InvokeRepeating(nameof(Spawn), 1.0f, 1.0f);


        // Start the coroutine to deactivate the GameObject after 5 seconds
        // StartCoroutine(DeactivateAfterDelay(7f));
    }
    private void Spawn()
    {
        for ( var i = 0; i < _spawnAmount; i ++ )
        {

            var rock = _usePool ?  _pool.Get() : Instantiate(_shapePrefab);
            rock.transform.position = transform.position + Random.insideUnitSphere * 30; // spawn range
            rock.Init(KillShape);
        
        }


    }
    private void KillShape(Rock rock)
    {
        if (_usePool) _pool.Release(rock);        
       else Destroy(rock.gameObject);


    }
   

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 30f); // Visual aid for spawn range
    }

    // Coroutine to deactivate the GameObject after a specified delay
    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}


