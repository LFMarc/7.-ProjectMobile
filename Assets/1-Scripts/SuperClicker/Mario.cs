using UnityEngine;
using System;
using DG.Tweening;

public class Mario : MonoBehaviour
{
    #region Properties
    public SlotButtonUI destiny { get; set; }
    [field: SerializeField] public float RepeatRate { get; private set; } = 1.0f; //Inicialmente 1 click por segundo
    #endregion

    #region Fields
    private bool isActive = false;

    public static event Action<int> OnMarioLevelUp; //Evento para actualizar la UI
    private int level = 0;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        SlotButtonUI.OnSlotClicked += SetDestiny;
    }

    private void OnDestroy()
    {
        SlotButtonUI.OnSlotClicked -= SetDestiny;
    }
    #endregion

    #region Public Methods
    public void ActivateMario()
    {
        if (!isActive)
        {
            isActive = true;
            level++;
            OnMarioLevelUp?.Invoke(level);
            StartClicking();
        }
    }

    public void ImproveMario()
    {
        if (isActive)
        {
            RepeatRate *= 0.8f; // Reduce el tiempo entre clics (más rápido)
            level++;
            StartClicking(); //Reinicia los clics con la nueva velocidad

            OnMarioLevelUp?.Invoke(level);
        }
    }
    #endregion

    #region Private Methods
    private void SetDestiny(SlotButtonUI newDestiny)
    {
        if (newDestiny != null && newDestiny.ClicksLeft > 0) //Solo cambia si el nuevo destino tiene stock
        {
            destiny = newDestiny;
            Movement();
            StartClicking(); //Reinicia los clics si encuentra un nuevo destino válido
        }
    }

    private void StartClicking()
    {
        CancelInvoke(nameof(Click)); // Cancela cualquier llamada previa
        Invoke(nameof(Click), RepeatRate); // Llama a Click() después de RepeatRate segundos
    }

    private void Click()
    {
        if (destiny != null && destiny.ClicksLeft > 0) //Solo clickea si hay stock
        {
            destiny.Click(1, true);
            Invoke(nameof(Click), RepeatRate); //Sigue clickeando con el nuevo RepeatRate
        }
        else
        {
            CancelInvoke(nameof(Click)); //Para de hacer clics si ya no hay stock
            destiny = null; //Mario se queda sin destino
        }
    }


    private void Movement()
    {
        if (destiny != null)
            transform.DOMove(destiny.transform.position, 1);
    }
    #endregion
}
