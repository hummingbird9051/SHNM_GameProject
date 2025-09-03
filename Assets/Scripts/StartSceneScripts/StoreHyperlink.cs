using UnityEngine;
using UnityEngine.UI;

public class StoreHyperlink : MonoBehaviour
{
    private string StoreUrl = "https://www.vangma1897.shop/";
    private string InstaURL = "https://www.instagram.com/p/CYlEdK7rtKb/?igsh=MWFnNHNpbDR6aG5wOA==";
    private string FormURL = "https://forms.gle/RChmcjy5Xx51uE7p6";

    public void OpenStoreLink()
    {
        Application.OpenURL(StoreUrl);
    }

    public void OpenInstagramLink()
    {
        Application.OpenURL(InstaURL);
    }

    public void OpenGoogleForm()
    {
        Application.OpenURL(FormURL);
    }
}
