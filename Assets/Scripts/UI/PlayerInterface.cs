using UnityEngine;
using UnityEngine.UIElements;

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

    private void Awake()
    {
        _player = FindAnyObjectByType<Player>();
        _crystal = FindAnyObjectByType<Crystal>();
        
        _playerHealthBar = _document.rootVisualElement.Q<VisualElement>("PlayerHealthBackground")
            .Q<VisualElement>("HealthbarFill");
        _crystalHealthBar = _document.rootVisualElement.Q<VisualElement>("CrystalHealthBackground")
            .Q<VisualElement>("HealthbarFill");
        
        if(_playerHealthBar == null)
            Debug.LogError("No Player Health Bar found");
        
        if(_crystalHealthBar == null)
            Debug.LogError("No Crystal Health Bar found");
    }

    void Start()
    {
        _player.Health.OnDamageTaken += UpdatePlayerHealth;
        _crystal.Health.OnDamageTaken += UpdateCrystalHealth;
    }

    private void OnDestroy()
    {
        _player.Health.OnDamageTaken -= UpdatePlayerHealth;
        _crystal.Health.OnDamageTaken -= UpdateCrystalHealth;
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
