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
        // Удаляем только старые эффекты, не трогая сам корабль
        foreach (Transform child in playerRootTransform)
        {
            if (child.gameObject.name.StartsWith("Effect_"))
            {
                Destroy(child.gameObject);
            }
        }

        int selectedIndex = YG.YG2.saves.selectedEffectIndex;
        if (selectedIndex >= 0 && selectedIndex < effectPrefabs.Count)
        {
            GameObject prefabToSpawn = effectPrefabs[selectedIndex];

            if (prefabToSpawn != null)
            {
                GameObject spawnedEffect = Instantiate(prefabToSpawn, playerRootTransform);
                spawnedEffect.name = "Effect_" + selectedIndex; // Пометка для удаления
                spawnedEffect.transform.localPosition = Vector3.zero;
                spawnedEffect.transform.localRotation = Quaternion.identity;
                spawnedEffect.transform.localScale = Vector3.one;
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
