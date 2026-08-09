using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

public enum PurchaseType { SoftCurrency, YandexMoney }

[System.Serializable]
public class WebShipData
{
    public string nameOfShip;
    public GameObject fbxPrefab; 
    public PurchaseType purchaseType;
    public int price;
    public string yandexProductId;
}

public class VerticalShipMenu : MonoBehaviour
{
    [Header("UI элементы")]
    [SerializeField] private RectTransform contentContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Button actionButton; 
    [SerializeField] private TextMeshProUGUI actionButtonText; 

    [Header("Настройки категории")]
    [SerializeField] private string _savePrefix = "Ship";

    [Header("3D настройки")]
    [SerializeField] private Transform spawnPoint; 
    [SerializeField] private float rotationSpeed = 40f;

    [Header("Список кораблей")]
    [SerializeField] private List<WebShipData> shipList;

    private GameObject spawnedShip;
    private int currentSelectedIndex = 0;

    private void OnEnable()
    {
#if yandexInApp
        YG2.iap.onBuy += OnYandexPurchaseSuccess;
#elif YG_PAYMENTS
        YG2.iaps.onPurchaseSuccess += OnYandexPurchaseSuccess;
#endif
    }

    private void OnDisable()
    {
#if yandexInApp
        YG2.iap.onBuy -= OnYandexPurchaseSuccess;
#elif YG_PAYMENTS
        YG2.iaps.onPurchaseSuccess -= OnYandexPurchaseSuccess;
#endif
    }

    void Start()
    {
        PlayerPrefs.SetInt($"{_savePrefix}_Owned_0", 1);

        if (!PlayerPrefs.HasKey($"Selected_{_savePrefix}_Index"))
        {
            PlayerPrefs.SetInt($"Selected_{_savePrefix}_Index", 0);
            PlayerPrefs.Save();
        }

        for (int i = 0; i < shipList.Count; i++)
        {
            GameObject newBtn = Instantiate(buttonPrefab, contentContainer);
            MenuButtonID btnScript = newBtn.GetComponent<MenuButtonID>();

            if (btnScript != null)
            {
                btnScript.SetupButton(i, shipList[i].nameOfShip, OnShipButtonClicked);
            }
        }

        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnActionButtonClicked);
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
        currentSelectedIndex = index;

        if (spawnedShip != null) 
        {
            Destroy(spawnedShip);
        }

        if (shipList[index].fbxPrefab != null)
        {
            spawnedShip = Instantiate(shipList[index].fbxPrefab, spawnPoint);
            spawnedShip.transform.localPosition = Vector3.zero;
            spawnedShip.transform.localRotation = Quaternion.identity; 
        }

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (actionButtonText == null) return;

        bool isOwned = IsShipOwned(currentSelectedIndex);
        int activeShipIndex = PlayerPrefs.GetInt($"Selected_{_savePrefix}_Index", 0);

        if (isOwned)
        {
            if (activeShipIndex == currentSelectedIndex)
            {
                actionButtonText.text = "Выбран";
                actionButton.interactable = false;
            }
            else
            {
                actionButtonText.text = "Выбрать";
                actionButton.interactable = true;
            }
        }
        else
        {
            WebShipData ship = shipList[currentSelectedIndex];
            if (ship.purchaseType == PurchaseType.SoftCurrency)
            {
                actionButtonText.text = $"Купить за {ship.price} монет";
            }
            else
            {
                actionButtonText.text = "Купить за Яны";
            }
            actionButton.interactable = true;
        }
    }

    private bool IsShipOwned(int index)
    {
        if (PlayerPrefs.GetInt($"{_savePrefix}_Owned_" + index, 0) == 1)
            return true;

        WebShipData ship = shipList[index];
        if (ship.purchaseType == PurchaseType.YandexMoney && !string.IsNullOrEmpty(ship.yandexProductId))
        {
#if yandexInApp
            if (YG2.iap.IsPurchased(ship.yandexProductId))
            {
                PlayerPrefs.SetInt($"{_savePrefix}_Owned_" + index, 1);
                PlayerPrefs.Save();
                return true;
            }
#elif YG_PAYMENTS
            if (YG2.iaps.IsPurchased(ship.yandexProductId))
            {
                PlayerPrefs.SetInt($"{_savePrefix}_Owned_" + index, 1);
                PlayerPrefs.Save();
                return true;
            }
#endif
        }

        return false;
    }

    private void OnActionButtonClicked()
    {
        if (IsShipOwned(currentSelectedIndex))
        {
            PlayerPrefs.SetInt($"Selected_{_savePrefix}_Index", currentSelectedIndex);
            PlayerPrefs.Save();
            UpdateButtonState();
        }
        else
        {
            TryPurchaseShip(currentSelectedIndex);
        }
    }

    private void TryPurchaseShip(int index)
    {
        WebShipData ship = shipList[index];

        if (ship.purchaseType == PurchaseType.SoftCurrency)
        {
            int currentCoins = YG2.saves.money; 

            if (currentCoins >= ship.price) 
            {
                YG2.saves.money -= ship.price;
                YG2.SaveProgress();
                ConfirmPurchase(index);
            }
            else
            {
                Debug.Log("Недостаточно внутриигровых монет!");
            }
        }
        else if (ship.purchaseType == PurchaseType.YandexMoney)
        {
            if (!string.IsNullOrEmpty(ship.yandexProductId))
            {
#if yandexInApp
                YG2.iap.Buy(ship.yandexProductId);
#elif YG_PAYMENTS
                YG2.iaps.Buy(ship.yandexProductId);
#else
                Debug.LogWarning("Модуль покупок отключен. Симулируем покупку.");
                ConfirmPurchase(index); 
#endif
            }
            else
            {
                Debug.LogError($"У корабля {ship.nameOfShip} (индекс {index}) отсутствует Yandex Product ID!");
            }
        }
    }

    private void OnYandexPurchaseSuccess(string id)
    {
        for (int i = 0; i < shipList.Count; i++)
        {
            if (shipList[i].purchaseType == PurchaseType.YandexMoney && shipList[i].yandexProductId == id)
            {
                ConfirmPurchase(i);
                break;
            }
        }
    }

    private void ConfirmPurchase(int index)
    {
        PlayerPrefs.SetInt($"{_savePrefix}_Owned_" + index, 1); 
        PlayerPrefs.SetInt($"Selected_{_savePrefix}_Index", index); 
        PlayerPrefs.Save();
        
        UpdateButtonState();

        if (MainMenuDisplay.Instance != null)
        {
            MainMenuDisplay.Instance.DisplayStats();
        }

        Debug.Log($"Успешная покупка: {shipList[index].nameOfShip}");
    }
}
