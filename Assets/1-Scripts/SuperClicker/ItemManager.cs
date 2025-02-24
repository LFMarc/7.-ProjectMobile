using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject blueShellButton;
    [SerializeField] private GameObject bulletBillButton;
    [SerializeField] private GameObject shockButton;

    [SerializeField] private GameObject blueShellPrefab;
    [SerializeField] private GameObject bulletBillPrefab;
    [SerializeField] private GameObject shockPrefab;

    [SerializeField] private GameController _game;

    // Info Popups
    [SerializeField] private GameObject blueShellInfo;
    [SerializeField] private GameObject bulletBillInfo;
    [SerializeField] private GameObject shockInfo;

    public void Awake()
    {
        //Ocultar botones
        blueShellButton.SetActive(false);
        bulletBillButton.SetActive(false);
        shockButton.SetActive(false);

        blueShellInfo.SetActive(false);
        bulletBillInfo.SetActive(false);
        shockInfo.SetActive(false);
    }

    public void UseBlueShell()
    {
        blueShellButton.SetActive(false);

        SlotButtonUI firstSlot = GetFirstAvailableSlot();
        if (firstSlot != null)
        {
            GameObject shell = Instantiate(blueShellPrefab, transform.position, Quaternion.identity);
            shell.transform.DOMove(firstSlot.transform.position, 1f).OnComplete(() => {
                firstSlot.Click(Mathf.RoundToInt(_game.ClickRatio * 10), true);
                Destroy(shell);
            });
        }
    }

    public void UseBulletBill()
    {
        bulletBillButton.SetActive(false);

        int targets = Random.Range(1, 6);
        List<SlotButtonUI> availableSlots = GetRandomSlots(targets);
        int totalClicks = Mathf.RoundToInt(_game.ClickRatio * targets);
        int clicksPerSlot = totalClicks / targets;

        foreach (SlotButtonUI slot in availableSlots)
        {
            GameObject bullet = Instantiate(bulletBillPrefab, transform.position, Quaternion.identity);
            bullet.transform.DOMove(slot.transform.position, 0.5f).OnComplete(() => {
                slot.Click(clicksPerSlot, true);
                Destroy(bullet);
            });
        }
    }

    public void UseShock()
    {
        GameObject shock = Instantiate(shockPrefab, transform.position, Quaternion.identity);
        foreach (SlotButtonUI slot in FindObjectsOfType<SlotButtonUI>())
        {
            slot.Click(Mathf.RoundToInt(_game.ClickRatio), true);
        }
        shockButton.SetActive(false);
        Destroy(shock, 2f);
        shockButton.SetActive(false);
    }

    private SlotButtonUI GetFirstAvailableSlot()
    {
        foreach (SlotButtonUI slot in FindObjectsOfType<SlotButtonUI>())
        {
            if (slot.ClicksLeft > 0)
                return slot;
        }
        return null;
    }

    private List<SlotButtonUI> GetRandomSlots(int count)
    {
        List<SlotButtonUI> allSlots = new List<SlotButtonUI>(FindObjectsOfType<SlotButtonUI>());
        List<SlotButtonUI> selectedSlots = new List<SlotButtonUI>();

        while (selectedSlots.Count < count && allSlots.Count > 0)
        {
            int index = Random.Range(0, allSlots.Count);
            selectedSlots.Add(allSlots[index]);
            allSlots.RemoveAt(index);
        }
        return selectedSlots;
    }

    public void EnableItemUseButtons()
    {
        blueShellButton.SetActive(true);
        bulletBillButton.SetActive(true);
        shockButton.SetActive(true);
    }

    // Info Popups
    public void OnClickBlueShellInfo()
    {
        blueShellInfo.SetActive(!blueShellInfo.activeSelf);
    }

    public void OnClickBulletBillInfo()
    {
        bulletBillInfo.SetActive(!bulletBillInfo.activeSelf);
    }

    public void OnClickShockInfo()
    {
        shockInfo.SetActive(!shockInfo.activeSelf);
    }
}
