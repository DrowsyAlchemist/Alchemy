using Agava.YandexGames;
using UnityEngine;

public class ElementForYanOpener : IElementClickHandler
{
    private const string PurchaseId = "OpenElement";

    public void OnElementClick(BookElementRenderer elementRenderer)
    {
#if UNITY_EDITOR
        OnPurchased(elementRenderer);
        return;
#endif
        Billing.PurchaseProduct(PurchaseId, onSuccessCallback: (response) => OnPurchased(elementRenderer), developerPayload: elementRenderer.Element.Id);
    }

    private void OnPurchased(BookElementRenderer elementRenderer)
    {
        elementRenderer.Element.Open();
        Debug.Log("Element purchased");
        elementRenderer.RenderOpened(elementRenderer.Element);
    }
}
