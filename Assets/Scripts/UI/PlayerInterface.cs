using System.Collections.Generic;
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
    private CrystalBall _crystalBall;

    [SerializeField]
    private ContestedHealth _playerContestedHealth;
    
    [SerializeField]
    private Crystal _crystal;

    [SerializeField]
    private CaligneaBoss _boss;
    
    private VisualElement _playerHealthBar;
    
    private VisualElement _playerContestedHealthBar;

    private VisualElement _crystalHealthBar;
    
    private VisualElement _crystalHealthBarBackground;

    private Label _waveLabel;

    private VisualElement _deathOverlay;

    private VisualElement _winOverlay;
    
    private Label _enemiesLeftLabel;

    private Image _crosshair;

    private GameManager _gameManager;
    private WaveManager _waveManager;

    private Image _burstIcon;
    private Image _beamIcon;

    private VisualElement _bossHealthBar;
    private VisualElement _bossPatienceBar;

    private Image _letter;
    
    private List<Image> _modeImages = new List<Image>();

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
        _crystalHealthBarBackground = _document.rootVisualElement.Q<VisualElement>("CrystalHealthBackground");
        _waveLabel = _document.rootVisualElement.Q<VisualElement>("WaveDisplayBackground")
            .Q<Label>("WaveDisplay");
        _deathOverlay = _document.rootVisualElement.Q<VisualElement>("DeathLabelBackground");
        _winOverlay = _document.rootVisualElement.Q<VisualElement>("WinLabelBackground");
        _enemiesLeftLabel = _document.rootVisualElement.Q<VisualElement>("EnemiesLeftBackground")
            .Q<Label>("EnemiesLeftLabel");
        _crosshair = _document.rootVisualElement.Q<VisualElement>("Crosshair")
            .Q<Image>("Crosshair");;
        _burstIcon = _document.rootVisualElement.Q<VisualElement>("ModeBackground")
            .Q<Image>("BurstIcon");
        _beamIcon = _document.rootVisualElement.Q<VisualElement>("ModeBackground")
            .Q<Image>("BeamIcon");
        _bossHealthBar = _document.rootVisualElement.Q<VisualElement>("CaligneaHealthBackground");
        _bossPatienceBar = _document.rootVisualElement.Q<VisualElement>("CaligneaPatienceBackground");
        _letter = _document.rootVisualElement.Q<Image>("FakeLetter");
        
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
        
        if(_crosshair == null)
            Debug.LogError("Crosshair not found");
        
        if(_burstIcon == null)
            Debug.LogError("Burst Icon not found");
        
        if(_beamIcon == null)
            Debug.LogError("Beam Icon not found");
        
        if(_bossHealthBar == null)
            Debug.LogError("Boss Health Bar not found");
        
        if(_bossPatienceBar == null)
            Debug.LogError("Boss Patience Bar not found");
        
        if(_letter == null)
            Debug.LogError("Letter image not found");
        
        _modeImages.Add(_burstIcon);
        _modeImages.Add(_beamIcon);
        
        _deathOverlay.style.display = DisplayStyle.None;
        _winOverlay.style.display = DisplayStyle.None;
        
        _letter.style.display = DisplayStyle.None;
        
        _enemiesLeftLabel.text = "Enemies Left:";
        _waveLabel.text = "Wave:";
    }

    void Start()
    {
        // I have no clue why this needs to be here and not in Awake()
        _playerContestedHealth = _player.ContestedHealth;
        _crystalBall = _player.CrystalBall;
        
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
        _player.OnAimAssistToggle += UpdateCrosshair;
        _crystalBall.OnModeChange += UpdateCrystalBallUI;
        CaligneaBoss.OnBossSpawned += ShowBossMeters;
        CaligneaBoss.OnPatienceExpires += ShowFakeLetter;
        
        UpdateWave(_waveManager.CurrentWave);
        UpdateCrystalBallUI(_crystalBall.CurrentModeIndex);
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
        _player.OnAimAssistToggle -= UpdateCrosshair;
        _crystalBall.OnModeChange -= UpdateCrystalBallUI;
        CaligneaBoss.OnBossSpawned -= ShowBossMeters;
        CaligneaBoss.OnPatienceExpires -= ShowFakeLetter;

        if (_boss != null)
        {
            _boss.OnHealthChanged -= UpdateBossHealth;
            _boss.OnPatienceChange -= UpdateBossPatience;
        }

        _enemiesLeftLabel.text = "Enemies Left:";
        _waveLabel.text = "Wave:";
    }

    void UpdateCrosshair(bool aimAssist)
    {
        if (aimAssist)
            _crosshair.tintColor = Color.red;
        else
            _crosshair.tintColor = Color.white;
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
        
        _bossHealthBar.style.display = DisplayStyle.None;
        _bossPatienceBar.style.display = DisplayStyle.None;
    }

    void ShowFakeLetter()
    {
        _letter.style.display = DisplayStyle.Flex;

        _enemiesLeftLabel.style.display = DisplayStyle.None;
        _waveLabel.style.display = DisplayStyle.None;
        _bossHealthBar.style.display = DisplayStyle.None;
        _bossPatienceBar.style.display = DisplayStyle.None;
        _playerHealthBar.style.display = DisplayStyle.None;
        _crystalHealthBar.style.display = DisplayStyle.None;
        _crystalHealthBarBackground.style.display = DisplayStyle.None;
        _burstIcon.style.display = DisplayStyle.None;
        _beamIcon.style.display = DisplayStyle.None;
        _crosshair.style.display = DisplayStyle.None;
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

    void UpdateCrystalBallUI(int mode)
    {
        foreach (Image image in _modeImages)
            image.style.display = DisplayStyle.None;
        
        _modeImages[mode].style.display = DisplayStyle.Flex;
    }

    void ShowBossMeters(CaligneaBoss boss)
    {
        _boss = boss;
        _bossHealthBar.style.display = DisplayStyle.Flex;
        _bossPatienceBar.style.display = DisplayStyle.Flex;
        
        _boss.OnHealthChanged += UpdateBossHealth;
        _boss.OnPatienceChange += UpdateBossPatience;
    }

    void UpdateBossHealth()
    {
        _bossHealthBar.Q<VisualElement>("BossbarFill").style.width = 
            Length.Percent((_boss.CurrentHealth / _boss.MaxHealth) * 100);
    }

    void UpdateBossPatience(float amount)
    {
        _bossPatienceBar.Q<VisualElement>("PatiencebarFill").style.width =
            Length.Percent((_boss.CurrentPatience / _boss.MaxPatience) * 100);
    }
}
