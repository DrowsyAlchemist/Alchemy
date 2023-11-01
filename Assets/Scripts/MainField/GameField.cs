using Agava.YandexGames;
using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField] private RectTransform _elementsContainer;
    [SerializeField] private UIButton _clearButton;
    [SerializeField] private UIButton _hideAdForYanButton;

    private const string OffAdProductId = "OffAd";
    private Saver _saver;

    public RectTransform ElementsContainer => _elementsContainer;

    public void Init(Saver saver)
    {
        _saver = saver;
        _clearButton.AssignOnClickAction(Clear);
        _hideAdForYanButton.AssignOnClickAction(HideStickyAd);
    }

    public void Clear()
    {
        for (int i = 0; i < _elementsContainer.childCount; i++)
            Destroy(_elementsContainer.GetChild(i).gameObject);
    }

    private void HideStickyAd()
    {
        Billing.PurchaseProduct(OffAdProductId, onSuccessCallback: (response) =>
        {
            StickyAd.Hide();
            _saver.OffAd();
        });
    }
}
