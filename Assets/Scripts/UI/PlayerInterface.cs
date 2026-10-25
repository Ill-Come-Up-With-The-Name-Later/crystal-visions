using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class PlayerInterface : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private UIDocument _document;
    
    [Header("Debug (Do not edit)")]
    [SerializeField]
    private Player _player;
    
    [SerializeField]
    private Crystal _crystal;
    
    private VisualElement _playerHealthBar;

    private VisualElement _crystalHealthBar;

    private Label _waveLabel;

    private VisualElement _deathOverlay;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _crystal = FindAnyObjectByType<Crystal>();
        
        _playerHealthBar = _document.rootVisualElement.Q<VisualElement>("PlayerHealthBackground")
            .Q<VisualElement>("HealthbarFill");
        _crystalHealthBar = _document.rootVisualElement.Q<VisualElement>("CrystalHealthBackground")
            .Q<VisualElement>("HealthbarFill");
        _waveLabel = _document.rootVisualElement.Q<VisualElement>("WaveDisplayBackground")
            .Q<Label>("WaveDisplay");
        _deathOverlay = _document.rootVisualElement.Q<VisualElement>("DeathLabelBackground");
        
        if(_playerHealthBar == null)
            Debug.LogError("No Player Health Bar found");
        
        if(_crystalHealthBar == null)
            Debug.LogError("No Crystal Health Bar found");
        
        if(_waveLabel == null)
            Debug.LogError("Wave Label not found");
        
        if(_deathOverlay == null)
            Debug.LogError("Death Overlay not found");
    }

    void Start()
    {
        _player.Health.OnDamageTaken += UpdatePlayerHealth;
        _crystal.Health.OnDamageTaken += UpdateCrystalHealth;
        GameManager.Instance.OnLoss += ShowDeathOverlay;
        WaveManager.Manager.OnWaveStarted += UpdateWave;
    }

    private void OnDestroy()
    {
        _player.Health.OnDamageTaken -= UpdatePlayerHealth;
        _crystal.Health.OnDamageTaken -= UpdateCrystalHealth;
        GameManager.Instance.OnLoss -= ShowDeathOverlay;
        WaveManager.Manager.OnWaveStarted -= UpdateWave;
    }

    void ShowDeathOverlay()
    {
        Debug.Log("Show death overlay");
        _deathOverlay.style.display = DisplayStyle.Flex;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void UpdateWave(int wave)
    {
        _waveLabel.text = $"Wave: {(wave + 1).ToString()}";
    }

    void UpdatePlayerHealth(float damage)
    {
        _playerHealthBar.style.width = 
            Length.Percent((_player.Health.CurrentHealth / _player.Health.MaxHealth) * 100);
    }

    void UpdateCrystalHealth(float damage)
    {
        _crystalHealthBar.style.width =
            Length.Percent((_crystal.Health.CurrentHealth / _crystal.Health.MaxHealth) * 100);
    }
}
