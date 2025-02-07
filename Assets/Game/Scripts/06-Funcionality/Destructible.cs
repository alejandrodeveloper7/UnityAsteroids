using System;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public Action OnDestroyed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out BulletController detectedBullet);
        if (detectedBullet != null)
        {
            OnDestroyed?.Invoke();
        }
    }
}
