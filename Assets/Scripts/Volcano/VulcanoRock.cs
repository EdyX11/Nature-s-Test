using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class VulcanoRock : MonoBehaviour
{
    [SerializeField] private Rock _shapePrefab;
    [SerializeField] private int _spawnAmount = 20;
    [SerializeField] private int _defaultCap = 100;
    [SerializeField] private int _maxCap = 200;
    [SerializeField] private bool _usePool;

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

        // Start spawning rocks
        StartSpawning();
    }

    private void OnEnable()
    {
        StartSpawning(); // Ensure to restart spawning when the object becomes active
    }

    private void OnDisable()
    {
        StopSpawning(); // Stop spawning when the object becomes inactive
    }

    private void StartSpawning()
    {
        InvokeRepeating(nameof(Spawn), 1.0f, 1.0f); // Start or restart the repeating spawn
    }

    private void StopSpawning()
    {
        CancelInvoke(nameof(Spawn)); // Stop the repeating spawn
    }

    private void Spawn()
    {
        for (var i = 0; i < _spawnAmount; i++)
        {
            var rock = _usePool ? _pool.Get() : Instantiate(_shapePrefab);
            rock.transform.position = transform.position + Random.insideUnitSphere * 400; // Spawn range
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
        Gizmos.DrawWireSphere(transform.position, 400f); // Visual aid for spawn range
    }
}
