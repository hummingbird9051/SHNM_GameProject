using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class CostShower : MonoBehaviour
{
    [SerializeField] private BuyTickets tickets;
    [SerializeField] private GameObject notEnoughMinerals;
    [SerializeField] private GameObject tooManyTikets;
    [SerializeField] private GameObject purchaseSucceeded;


    private int costs = 1000;

    private int totalCost;

    
    void Update()
    {
        totalCost = tickets.ticketAmount * costs;
        TextMeshProUGUI txt = gameObject.GetComponent<TextMeshProUGUI>();
        if (txt != null)
        {
            txt.text = totalCost.ToString();
        }
    }

    public void Purchase()
    {
        if (GameDataManager.Instance.GetLivesData() + tickets.ticketAmount > 5)
        {
            tooManyTikets.SetActive(true);
            return;
        }
        else if (!GameDataManager.Instance.MinusMoney(totalCost))
        {
            notEnoughMinerals.SetActive(true);
            return;
        }
        else
        {
            purchaseSucceeded.SetActive(true);
            GameDataManager.Instance.AddLife(tickets.ticketAmount);
        }
    }
}
