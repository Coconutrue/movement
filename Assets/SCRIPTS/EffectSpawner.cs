using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    [Header("Список префабов эффектов (ПОРЯДОК КАК В МЕНЮ)")]
    [SerializeField] private List<GameObject> effectPrefabs;

    [Header("Куда вставить модель эффекта")]
    [SerializeField] private Transform playerRootTransform;

    [Header("Настройки сохранения")]
    [SerializeField] private string _savePrefix = "Effect";

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
        if (selectedIndex >= 0 && selectedIndex < effectPrefabs.Count)
        {
            GameObject prefabToSpawn = effectPrefabs[selectedIndex];

            if (prefabToSpawn != null)
            {
                GameObject spawnedShip = Instantiate(prefabToSpawn, playerRootTransform);
                spawnedShip.transform.localPosition = Vector3.zero;
                spawnedShip.transform.localRotation = Quaternion.identity;
                spawnedShip.transform.localScale = Vector3.one;
            }
            else
            {
                Debug.LogError("Префаб ефекта пуст");
            }
        }
        else
        {
            Debug.LogError("Выбранный индекс ефекта вне диапазона списка префабов");
        }
    }
}
