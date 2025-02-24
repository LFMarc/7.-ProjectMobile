using UnityEngine;
using System;
using TMPro;
using DG.Tweening;

public class GameController : MonoBehaviour
{
	#region Properties
	[field:SerializeField] public float ClickRatio { get; set; }
	[field:SerializeField] public PoolSystem Pool { get; set; }
    #endregion

    #region Fields
    [SerializeField] private TextMeshProUGUI _rewardText;
	[SerializeField] private TextMeshProUGUI _clicksText;

	[SerializeField] private ParticleSystem _particlesRain;

    [SerializeField] private Transform objectPool; //Generar automáticamente la progresión de clicks iniciales

    [SerializeField] private Mario _marioPrefab;
    private Mario _marioInstance;

    [SerializeField] private DonkeyKong _dkPrefab;
    private DonkeyKong _dkInstance; 


    #endregion

    #region Unity Callbacks
    // Start is called before the first frame update
    void Start()
    {
        SlotButtonUI.OnSlotReward += GetReward;
    }
	private void OnDestroy()
	{
		SlotButtonUI.OnSlotReward -= GetReward;
	}

	#endregion

	#region Public Methods
	public void RainParticles()
	{
		_particlesRain.Emit(Mathf.Clamp((int)ClickRatio, 0, 13));
	}
    #endregion

    #region Private Methods
    /***
 *       ____    U _____ u                 _       ____     ____    _    
 *    U |  _"\ u \| ___"|/__        __ U  /"\  uU |  _"\ u |  _"\ U|"|u  
 *     \| |_) |/  |  _|"  \"\      /"/  \/ _ \/  \| |_) |//| | | |\| |/  
 *      |  _ <    | |___  /\ \ /\ / /\  / ___ \   |  _ <  U| |_| |\|_|   
 *      |_| \_\   |_____|U  \ V  V /  U/_/   \_\  |_| \_\  |____/ u(_)   
 *      //   \\_  <<   >>.-,_\ /\ /_,-. \\    >>  //   \\_  |||_   |||_  
 *     (__)  (__)(__) (__)\_)-'  '-(_/ (__)  (__)(__)  (__)(__)_) (__)_) 
 */
    private void GetReward(Reward reward)
    {
        ShowReward(reward);

        if (reward.RewardType == RewardType.Plus)
        {
            ClickRatio += reward.Value;
            _clicksText.text = "x" + ClickRatio;
            return;
        }

        if (reward.RewardType == RewardType.Multi)
        {
            ClickRatio *= reward.Value;
            _clicksText.text = "x" + ClickRatio;
            return;
        }

        if (reward.RewardType == RewardType.Agent)
        {
            if (reward.Value == 0) // Mario
            {
                if (_marioInstance == null)
                {
                    _marioInstance = Instantiate(_marioPrefab, transform.position, Quaternion.identity);
                    _marioInstance.ActivateMario();
                }
                else
                {
                    _marioInstance.ImproveMario(); //Mejora a Mario si ya está en la escena
                }
            }
            if (reward.Value == 1) // Donkey Kong
            {
                if (_dkInstance == null)
                {
                    _dkInstance = Instantiate(_dkPrefab, transform.position, Quaternion.identity);
                    _dkInstance.Initialize(this); 
                    _dkInstance.ActivateDK();
                    Debug.Log("Donkey Kong inicializado y activado.");
                }
                else
                {
                    _dkInstance.ImproveDK();
                }
            }

            return;
        }
    }

    private void ShowReward(Reward reward)
	{
		//Initialziation
		if (!_rewardText.gameObject.activeSelf)
		{
			_rewardText.gameObject.SetActive(true);
			_rewardText.transform.localScale = Vector3.zero;
		}

		//Update text
		_rewardText.text = "REWARD\n " + reward.RewardType + reward.Value + " Clicks";

		// Crear una secuencia
		Sequence mySequence = DOTween.Sequence();

		// Añadir el primer efecto de escala
		mySequence.Append(_rewardText.transform.DOScale(1, 1));

		// Añadir el efecto de sacudida en la rotación
		mySequence.Append(_rewardText.transform.DOShakeRotation(1, new Vector3(0, 0, 30)));

		// Añadir el segundo efecto de escala
		mySequence.Append(_rewardText.transform.DOScale(0, 1));

		// Iniciar la secuencia
		mySequence.Play();

	}

    //Generar automáticamente la progresión de clicks iniciales
    public int GetInitialClicks(int index, int baseClicks = 20)
    {
        return Mathf.RoundToInt(baseClicks * Mathf.Pow(2f, index));
    }

    public int GetSlotIndex(SlotButtonUI slot)
    {
        for (int i = 0; i < objectPool.childCount; i++)
        {
            if (objectPool.GetChild(i) == slot.transform)
            {
                return i;
            }
        }
        return 0; // Fallback por si algo sale mal
    }

    #endregion
}
