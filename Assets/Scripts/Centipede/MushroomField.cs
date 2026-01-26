using UnityEngine;

public class MushroomField : MonoBehaviour
{
    private BoxCollider fieldArea;
    [SerializeField] 
    private Mushroom mushroomPrefab;
    [SerializeField]
    private int numberOfMushrooms = 30;
    [SerializeField]
    private Quaternion rotation;

    private void Awake()
    {
        fieldArea = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        for (int i = 0; i < numberOfMushrooms; i++)
        {
            Vector3 position = new Vector3(
                Mathf.Round(Random.Range(fieldArea.bounds.min.x, fieldArea.bounds.max.x)),
                0.5f,
                Mathf.Round(Random.Range(fieldArea.bounds.min.z, fieldArea.bounds.max.z))
            );
            Instantiate(mushroomPrefab, position, Quaternion.Euler(0f, 0f, 0f), transform);
        }
    }
}
