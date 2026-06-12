using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        Instantiate(playerPrefab, transform.position, transform.rotation);
    }
}