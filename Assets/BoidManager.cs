using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [SerializeField]
    bool separation = true;
    [SerializeField]
    bool alignment = true;
    [SerializeField]
    bool cohesion = true;

    [SerializeField]
    BoidSettings boidSettings;

    [SerializeField]
    int randomBoidAmount = 20;
    [SerializeField]
    float randomSpawnRadiusX = 20;
    [SerializeField]
    float randomSpawnRadiusZ = 10;
    [SerializeField]
    GameObject boidPrefab;



    UnityBoid[] cachedBoids;

    private void Start()
    {
        for (int i = 0; i < randomBoidAmount; ++i)
        {
            var spawnPos = Vector3.zero;
            spawnPos.x = Random.Range(-randomSpawnRadiusX, randomSpawnRadiusX);
            spawnPos.z = Random.Range(-randomSpawnRadiusZ, randomSpawnRadiusZ);

            SpawnBoid(spawnPos);
        }

        cachedBoids = FindObjectsOfType<UnityBoid>();
        UpdateBoidRules();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var point = ray.origin + (ray.direction * 50);
            point.y = 0;

            SpawnBoid(point);
        }

        cachedBoids = FindObjectsOfType<UnityBoid>();
        var otherBoidsData = new List<BoidData>();

        foreach (var boid in cachedBoids)
        {
            var boidTransform = boid.gameObject.transform;

            otherBoidsData.Add(
                new BoidData()
                {
                    position = ConvertToSystemVector3(boidTransform.position),
                    rotation = ConvertToSystemVector3(boidTransform.forward)
                }
            );
        }

        foreach (var boid in cachedBoids)
        {
            boid.UpdateBoid(otherBoidsData);
        }
    }

    void OnValidate()
    {
        UpdateBoidRules();
    }

    void SpawnBoid(Vector3 spawnPos)
    {
        var randomRot = Random.Range(0, 360);
        var spawnRot = Quaternion.Euler(0, randomRot, 0);

        Instantiate(boidPrefab, spawnPos, spawnRot, transform);
    }

    void UpdateBoidRules()
    {
        if (cachedBoids == null)
            return;

        foreach (var boid in cachedBoids)
        {
            if (boid == null)
                continue;

            boid.UpdateRules(separation, alignment, cohesion, boidSettings);
        }
    }

    System.Numerics.Vector3 ConvertToSystemVector3(Vector3 vector)
    {
        return new System.Numerics.Vector3(vector.x, vector.y, vector.z);
    }

    Vector3 ConvertToUnityVector3(System.Numerics.Vector3 vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }
}
