using System;
using TMPro;
using UnityEngine;

public class BuyTickets : MonoBehaviour
{
    public int ticketAmount = 0;

    public void CountUp()
    {
        ticketAmount += 1;
        refreshText();
    }

    public void CountDown()
    {
        ticketAmount -= 1;
        refreshText();
    }

    private void refreshText()
    {
        TextMeshProUGUI txt = gameObject.GetComponent<TextMeshProUGUI>();
        if (txt == null) return;
        txt.text = ticketAmount.ToString();
    }
}
