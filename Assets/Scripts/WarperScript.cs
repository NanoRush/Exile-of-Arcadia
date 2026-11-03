
using UnityEngine;

public class WarperScript : MonoBehaviour
{
    [System.Serializable]
    public class WarpPosition
    {
        public float x;
        public float y;
    }

    [Header("Warp Settings")]
    [SerializeField] private WarpPosition[] warpPositions;

    [Header("Visual Effects")]
    public ParticleSystem teleportTrail;

    private EnemyHealth enemyHealth;
    private int previousHealth;
    private int currentWarpIndex = 0;

    public AudioClip audioClip;
    private AudioSource AudioSource;

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        previousHealth = enemyHealth.health;
        AudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (enemyHealth.health < previousHealth && enemyHealth.health > 0)
        {
            Warp();
        }

        previousHealth = enemyHealth.health;
    }

    private void Warp()
    {
        if (warpPositions.Length == 0)
            return;

        Vector2 oldPosition = transform.position;

        Vector2 newPosition = new Vector2(
            warpPositions[currentWarpIndex].x,
            warpPositions[currentWarpIndex].y
        );

        // Calculate distance between positions
        float distance = Vector2.Distance(oldPosition, newPosition);

        // Calculate direction
        Vector2 direction = (newPosition - oldPosition).normalized;

        // Number of particles based on distance
        int particleCount = Mathf.Max(1, Mathf.CeilToInt(distance * 5f));

        // Move Warper
        transform.position = newPosition;
        AudioSource.PlayOneShot(audioClip);
        CameraImpactManager.Instance.HitImpact();

        // Emit particles evenly between old and new positions
        for (int i = 0; i <= particleCount; i++)
        {
            Vector2 particlePosition = Vector2.Lerp(
                oldPosition,
                newPosition,
                (float)i / particleCount
            );

            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
            emitParams.position = particlePosition;
            emitParams.velocity = Vector3.zero;

            teleportTrail.Emit(emitParams, 1);
        }

        currentWarpIndex++;

        if (currentWarpIndex >= warpPositions.Length)
            currentWarpIndex = 0;
    }

    private void StopTeleportTrail()
    {
        teleportTrail.Stop();
    }

    public WarpPosition[] GetWarpPositions()
    {
        return warpPositions;
    }

    public void SetWarpPositions(WarpPosition[] positions)
    {
        warpPositions = positions;
    }

}
