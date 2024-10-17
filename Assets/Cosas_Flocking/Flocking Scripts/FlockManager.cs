using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public static FlockManager FM;
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos = Vector3.zero;

    [Header("Fish Settings")]
    [Range(0.0f, 5.0f)] public float minSpeed;
    [Range(0.0f, 5.0f)] public float maxSpeed;
    [Range(1.0f, 10.0f)] public float neighbourDistance;
    [Range(1.0f, 5.0f)] public float rotationSpeed;

    private GameObject currentLeader;
    private Color originalColor;


    private GameObject newLeader;
    private int framesAsLeader = 0; // Contador de frames
    private const int leaderHoldFrames = 300; // N�mero de frames que el l�der se mantiene

    void Start()
    {
        allFish = new GameObject[numFish];

        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
            allFish[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
        }

        FM = this;
        goalPos = this.transform.position;
    }

    void Update()
    {
        if (Random.Range(0, 100) < 10)
        {
            goalPos = this.transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
        }

        // Detectar el pez l�der
        UpdateLeader();
    }

    void UpdateLeader()
    {
        float closestDistance = Mathf.Infinity;
        newLeader = null;

        foreach (GameObject fish in allFish)
        {
            float distanceToGoal = Vector3.Distance(fish.transform.position, goalPos);
            if (distanceToGoal < closestDistance)
            {
                closestDistance = distanceToGoal;
                newLeader = fish;
            }
        }

        if (newLeader != currentLeader)
        {
            // Si hay un nuevo l�der
            if (newLeader != null)
            {
                // Si el nuevo l�der ha estado m�s cerca durante el tiempo suficiente
                framesAsLeader++;
                if (framesAsLeader >= leaderHoldFrames)
                {
                    // Restaurar color original del l�der anterior
                    if (currentLeader != null)
                    {
                        Renderer rend = currentLeader.GetComponentInChildren<Renderer>();
                        if (rend != null)
                        {
                            rend.material.color = originalColor;
                        }
                    }

                    // Cambiar al nuevo l�der
                    currentLeader = newLeader;
                    Renderer leaderRenderer = currentLeader.GetComponentInChildren<Renderer>();
                    if (leaderRenderer != null)
                    {
                        originalColor = leaderRenderer.material.color;
                        leaderRenderer.material.color = Color.red; // Cambia el color a rojo, por ejemplo
                    }

                    // Reiniciar el contador de frames
                    framesAsLeader = 0;
                }
            }
        }
        else
        {
            // Si el l�der sigue siendo el mismo, resetea el contador de frames
            framesAsLeader = 0;
        }
    }

}
