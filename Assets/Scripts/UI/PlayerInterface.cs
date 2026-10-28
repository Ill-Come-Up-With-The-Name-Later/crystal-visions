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
    private ContestedHealth _playerContestedHealth;
    
    [SerializeField]
    private Crystal _crystal;
    
    private VisualElement _playerHealthBar;
    
    private VisualElement _playerContestedHealthBar;

    private VisualElement _crystalHealthBar;

    private Label _waveLabel;

    private VisualElement _deathOverlay;

    private VisualElement _winOverlay;
    
    private Label _enemiesLeftLabel;

    private GameManager _gameManager;
    private WaveManager _waveManager;

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _crystal = FindAnyObjectByType<Crystal>();
        
        if(_player == null)
            Debug.LogError("No Player found");
        
        if(_crystal == null)
            Debug.LogError("No Crystal found");
        
        _playerHealthBar = _document.rootVisualElement.Q<VisualElement>("PlayerHealthBackground")
            .Q<VisualElement>("HealthbarFill");
        _playerContestedHealthBar = _document.rootVisualElement.Q<VisualElement>("PlayerHealthBackground")
            .Q<VisualElement>("ContestedHealthbarFill");
        _crystalHealthBar = _document.rootVisualElement.Q<VisualElement>("CrystalHealthBackground")
            .Q<VisualElement>("HealthbarFill");
        _waveLabel = _document.rootVisualElement.Q<VisualElement>("WaveDisplayBackground")
            .Q<Label>("WaveDisplay");
        _deathOverlay = _document.rootVisualElement.Q<VisualElement>("DeathLabelBackground");
        _winOverlay = _document.rootVisualElement.Q<VisualElement>("WinLabelBackground");
        _enemiesLeftLabel = _document.rootVisualElement.Q<VisualElement>("EnemiesLeftBackground")
            .Q<Label>("EnemiesLeftLabel");
        
        if(_playerHealthBar == null)
            Debug.LogError("No Player Health Bar found");
        
        if(_crystalHealthBar == null)
            Debug.LogError("No Crystal Health Bar found");
        
        if(_waveLabel == null)
            Debug.LogError("Wave Label not found");
        
        if(_deathOverlay == null)
            Debug.LogError("Death Overlay not found");
        
        if(_winOverlay == null)
            Debug.LogError("Win Overlay not found");
        
        if(_enemiesLeftLabel == null)
            Debug.LogError("Enemies left label not found");
        
        _deathOverlay.style.display = DisplayStyle.None;
        _winOverlay.style.display = DisplayStyle.None;
        
        _enemiesLeftLabel.text = "Enemies Left:";
        _waveLabel.text = "Wave:";
    }

    void Start()
    {
        // I have no clue why this needs to be here and not in Awake()
        _playerContestedHealth = _player.ContestedHealth;
        
        if(_playerContestedHealth == null) 
            Debug.LogError("No contested health found");
        
        _gameManager = GameManager.Instance;
        _waveManager = WaveManager.Instance;
        
        _player.Health.OnHealthChanges += UpdatePlayerHealth;
        _playerContestedHealth.OnContestedHealthChange += UpdatePlayerContestedHealth;
        _crystal.Health.OnHealthChanges += UpdateCrystalHealth;
        _gameManager.OnLoss += ShowDeathOverlay;
        _gameManager.OnWin += ShowWinOverlay;
        _waveManager.OnWaveStarted += UpdateWave;
        _waveManager.ChangeEnemiesLeft += UpdateRemainingEnemies;
        
        UpdateWave(0);
    }

    void OnDestroy()
    {
        _player.Health.OnHealthChanges -= UpdatePlayerHealth;
        _playerContestedHealth.OnContestedHealthChange -= UpdatePlayerContestedHealth;
        _crystal.Health.OnHealthChanges-= UpdateCrystalHealth;
        _gameManager.OnLoss -= ShowDeathOverlay;
        _gameManager.OnWin -= ShowWinOverlay;
        _waveManager.OnWaveStarted -= UpdateWave;
        _waveManager.ChangeEnemiesLeft -= UpdateRemainingEnemies;
        
        _enemiesLeftLabel.text = "Enemies Left:";
        _waveLabel.text = "Wave:";
    }
    
    void UpdateRemainingEnemies(int enemies)
    {
        _enemiesLeftLabel.text = $"Enemies Left: {enemies.ToString()}";
    }

    void ShowWinOverlay()
    {
        //Debug.Log("Show win overlay");
        _winOverlay.style.display = DisplayStyle.Flex;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ShowDeathOverlay()
    {
        //Debug.Log("Show death overlay");
        _deathOverlay.style.display = DisplayStyle.Flex;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void UpdateWave(int wave)
    {
        _waveLabel.text = $"Wave: {(wave + 1).ToString()}/{_waveManager.Waves}";
        UpdateRemainingEnemies(_waveManager.EnemiesLeftInWave);
    }

    void UpdatePlayerHealth()
    {
        _playerHealthBar.style.width = 
            Length.Percent((_player.Health.CurrentHealth / _player.Health.MaxHealth) * 100);
    }
    
    void UpdatePlayerContestedHealth(float amount)
    {
        _playerContestedHealthBar.style.width = 
            Length.Percent((_player.ContestedHealth.ContestedHealthAmount / _player.Health.MaxHealth) * 100);
    }

    void UpdateCrystalHealth()
    {
        _crystalHealthBar.style.width =
            Length.Percent((_crystal.Health.CurrentHealth / _crystal.Health.MaxHealth) * 100);
    }
}
