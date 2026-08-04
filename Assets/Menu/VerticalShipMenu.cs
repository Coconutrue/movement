using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WebShipData
{
    public string nameOfShip;
    public GameObject fbxPrefab; 
}

public class VerticalShipMenu : MonoBehaviour
{
    [Header("UI элементы")]
    [SerializeField] private RectTransform contentContainer;
    [SerializeField] private GameObject buttonPrefab;

    [Header("3D настройки")]
    [SerializeField] private Transform spawnPoint; 
    [SerializeField] private float rotationSpeed = 40f;

    [Header("Список кораблей")]
    [SerializeField] private List<WebShipData> shipList;

    private GameObject spawnedShip;

    void Start()
    {
        for (int i = 0; i < shipList.Count; i++)
        {
            GameObject newBtn = Instantiate(buttonPrefab, contentContainer);
            MenuButtonID btnScript = newBtn.GetComponent<MenuButtonID>();

            if (btnScript != null)
            {
                btnScript.SetupButton(i, shipList[i].nameOfShip, OnShipButtonClicked);
            }
        }

        if (shipList.Count > 0)
        {
            OnShipButtonClicked(0);
        }
    }

    void Update()
    {
        if (spawnedShip != null)
        {
            spawnedShip.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    private void OnShipButtonClicked(int index)
{
    if (spawnedShip != null) 
    {
        Destroy(spawnedShip);
    }

    if (shipList[index].fbxPrefab != null)
    {
        spawnedShip = Instantiate(shipList[index].fbxPrefab, spawnPoint);
        spawnedShip.transform.localPosition = Vector3.zero;
        spawnedShip.transform.localRotation = Quaternion.identity; 
        
        PlayerPrefs.SetInt("SelectedShipIndex", index);
        PlayerPrefs.Save();
    }
}
}
