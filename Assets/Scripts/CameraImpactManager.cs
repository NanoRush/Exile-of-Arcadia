using UnityEngine;
using Cinemachine;

public class CameraImpactManager : MonoBehaviour
{
    public static CameraImpactManager Instance;

    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Shake Strength")]
    [SerializeField] private float hitShake = 0.6f;
    [SerializeField] private float deathShake = 1.2f;

    private void Awake()
    {
        Instance = this;
    }

    public void HitImpact()
    {
        impulseSource.GenerateImpulse(hitShake);
    }

    public void DeathImpact()
    {
        impulseSource.GenerateImpulse(deathShake);
    }
}