using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgentUIManager : MonoBehaviour
{
    [System.Serializable]
    public class AgentData
    {
        public string Name;
        public Button InfoButton;
        public Button LevelButton;
        public GameObject AgentPrefab;
        public bool IsUnlocked = false;
        public int Level = 0;
        public float BaseRepeatRate = 2.0f;
    }

    [SerializeField] private Mario _agent;
    [SerializeField] private AgentData[] agents;
    [SerializeField] private GameController _gameController;

    // Info Buttons
    [SerializeField] private GameObject toadInfo;
    [SerializeField] private TextMeshProUGUI toadLevelText;

    [SerializeField] private GameObject marioInfo;
    [SerializeField] private TextMeshProUGUI marioLevelText;

    [SerializeField] private GameObject dkInfo;
    [SerializeField] private TextMeshProUGUI dkLevelText;

    private void Start()
    {
        Mario.OnMarioLevelUp += UpdateMarioLevelUI;
        DonkeyKong.OnDKLevelUp += UpdateDKLevelUI;
    }

    private void OnDestroy()
    {
        Mario.OnMarioLevelUp -= UpdateMarioLevelUI;
        DonkeyKong.OnDKLevelUp -= UpdateDKLevelUI;
    }

    private void UpdateMarioLevelUI(int level)
    {
        marioLevelText.text = level.ToString();
    }

    private void UpdateDKLevelUI(int level)
    {
        dkLevelText.text = level.ToString();
    }

    public void OnClickToadInfo()
    {
        toadInfo.SetActive(!toadInfo.activeSelf);
    }

    public void OnClickMarioInfo()
    {
        marioInfo.SetActive(!marioInfo.activeSelf);
    }

    public void OnClickDkInfo()
    {
        dkInfo.SetActive(!dkInfo.activeSelf);
    }

    private void Update()
    {
        toadLevelText.text = _gameController.ClickRatio.ToString("F0");
    }
}
