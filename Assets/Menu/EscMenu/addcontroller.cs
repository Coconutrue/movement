using UnityEngine;
using YG; // Обязательно подключаем пространство имен Яндекс Игр

public class addcontroller : MonoBehaviour
{
    public void ShowFullscreenAd()
    {
        YG2.InterstitialAdvShow();
    }

    public void ShowRewardedAd()
    {
        YG2.RewardedAdvShow("ContinueAdd");
    }
}
