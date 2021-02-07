using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float Damage = 0;
    public float Speed = 1;
    public float LifeTime = 10f;
    public List<string> ignoredTags;
    public List<int> ignoredLayers;
    void Start()
    {
        if (Speed <= 0) Speed = 1;
    }

    void Update()
    {
        if (LifeTime <= 0) Destroy(gameObject);
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        LifeTime -= Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        if (!ignoredTags.Contains(other.tag) && !ignoredLayers.Contains(other.gameObject.layer) && other.GetType() != typeof(SphereCollider))
        {
            HealthManager health = other.gameObject.GetComponent<HealthManager>();
            if (health != null) health.ChangeHealth(-Damage);

            //TODO: spawn projectile hit prefab on current transform.
            Destroy(gameObject);
        }
    }
}
