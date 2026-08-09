using System.Collections.Generic;
using UnityEngine;

public class GameplayShipSpawner : MonoBehaviour
{
    [Header("Список префабов кораблей (ПОРЯДОК КАК В МЕНЮ!)")]
    [SerializeField] private List<GameObject> shipPrefabs;

    [Header("Куда вставить модель корабля")]
    [SerializeField] private Transform playerRootTransform;

    [Header("Настройки сохранения")]
    [SerializeField] private string _savePrefix = "Ship";

    void Awake()
    {
        foreach (Transform child in playerRootTransform)
        {
            if (child.gameObject.name != "Main Camera" && !child.CompareTag("MainCamera"))
            {
                Destroy(child.gameObject);
            }
        }

        int selectedIndex = PlayerPrefs.GetInt($"Selected_{_savePrefix}_Index", 0);
        if (selectedIndex >= 0 && selectedIndex < shipPrefabs.Count)
        {
            GameObject prefabToSpawn = shipPrefabs[selectedIndex];

            if (prefabToSpawn != null)
            {
                GameObject spawnedShip = Instantiate(prefabToSpawn, playerRootTransform);
                spawnedShip.transform.localPosition = Vector3.zero;
                spawnedShip.transform.localRotation = Quaternion.identity;
                spawnedShip.transform.localScale = Vector3.one;
            }
            else
            {
                Debug.LogError("Префаб корабля пустой!");
            }
        }
        else
        {
            Debug.LogError("Выбранный индекс корабля вне диапазона списка префабов!");
        }
    }
}
