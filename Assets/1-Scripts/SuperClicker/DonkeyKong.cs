using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class DonkeyKong : MonoBehaviour
{
    [SerializeField] private GameObject dkFacePrefab;
    private GameController _game;
    private int level = 0;
    private SlotButtonUI destiny;

    private static float _lastExtraClickTime = 0f;
    private float _extraClickCooldown = 2f;

    public delegate void DKLevelUpEvent(int level);
    public static event DKLevelUpEvent OnDKLevelUp;

    private bool isActive = false;

    public void Initialize(GameController game)
    {
        _game = game;
    }

    private void Start()
    {
        SlotButtonUI.OnSlotClicked += ExtraClick;
    }

    private void OnDestroy()
    {
        SlotButtonUI.OnSlotClicked -= ExtraClick;
    }

    public void ActivateDK()
    {
        if (!isActive)
        {
            isActive = true;
            level = 1;
            Debug.Log($"DK Activado - Nivel: {level}");
            OnDKLevelUp?.Invoke(level);
        }
    }

    public void ImproveDK()
    {
        if (isActive)
        {
            level++;
            _extraClickCooldown = Mathf.Max(0.5f, 2f - (level * 0.2f));
            Debug.Log($"DK Mejorado - Nuevo Nivel: {level}");
            OnDKLevelUp?.Invoke(level);
        }
    }

    public int GetLevel()
    {
        return level;
    }

    private void ExtraClick(SlotButtonUI clickedSlot)
    {
        if (Time.time - _lastExtraClickTime < _extraClickCooldown) return;
        _lastExtraClickTime = Time.time;

        if (_game == null)
        {
            Debug.LogError("DonkeyKong: _game sigue siendo null en ExtraClick.");
            return;
        }

        SlotButtonUI extraSlot = GetRandomSlot(clickedSlot);
        if (extraSlot != null)
        {
            int clickAmount = Mathf.RoundToInt(_game.ClickRatio);
            extraSlot.Click(clickAmount, true);
            SetDestiny(extraSlot);
        }
    }

    private SlotButtonUI GetRandomSlot(SlotButtonUI excludeSlot)
    {
        List<SlotButtonUI> availableSlots = new List<SlotButtonUI>();

        foreach (SlotButtonUI slot in FindObjectsOfType<SlotButtonUI>())
        {
            if (slot != excludeSlot && slot.ClicksLeft > 0)
            {
                availableSlots.Add(slot);
            }
        }

        return availableSlots.Count > 0 ? availableSlots[Random.Range(0, availableSlots.Count)] : null;
    }

    private void SetDestiny(SlotButtonUI newDestiny)
    {
        if (newDestiny != null)
        {
            destiny = newDestiny;
            AppearWithAnimation();
        }
    }

    private void AppearWithAnimation()
    {
        if (destiny != null)
        {
            transform.position = destiny.transform.position;
            transform.localScale = Vector3.zero;

            Sequence anim = DOTween.Sequence();
            anim.Append(transform.DOScale(350, 0.2f).SetEase(Ease.OutBack))
                .Append(transform.DOShakeRotation(0.2f, new Vector3(0, 0, 15)))
                .Append(transform.DOScale(300, 0.1f).SetEase(Ease.InOutSine));

            destiny = null;
        }
    }
}
