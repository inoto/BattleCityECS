using System;
using System.Collections;
using Morpeh.Globals;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleBattleCity
{
    public class Projectile : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D other)
        {
            var entity = GetComponent<ProjectileProvider>().Entity;
            ref var projectile = ref entity.GetComponent<ProjectileComponent>();
            

            // DestroyImmediate(gameObject);
        }
    }
}