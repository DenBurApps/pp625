using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreenView : MonoBehaviour
{
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _addPlantButton;
    [SerializeField] private Button _filterButton;
    [SerializeField] private TMP_InputField _searchInput;
    [SerializeField] private GameObject _emptyPlane;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action SettingsClicked;
    public event Action AddPlantClicked;
    public event Action FilterClicked;
    public event Action<string> SearchInputed;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _searchInput.onValueChanged.AddListener(OnSearchInputed);
        _settingsButton.onClick.AddListener(OnSettingsClicked);
        _addPlantButton.onClick.AddListener(OnAddPlantClicked);
        _filterButton.onClick.AddListener(OnFilterClicked);
    }

    private void OnDisable()
    {
        _searchInput.onValueChanged.RemoveListener(OnSearchInputed);
        _settingsButton.onClick.RemoveListener(OnSettingsClicked);
        _addPlantButton.onClick.RemoveListener(OnAddPlantClicked);
        _filterButton.onClick.RemoveListener(OnFilterClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void SetTransparent()
    {
        _screenVisabilityHandler.SetTransperent();
    }
    
    public void ToggleEmptyPlane(bool status)
    {
        _emptyPlane.gameObject.SetActive(status);
    }

    private void OnSettingsClicked() => SettingsClicked?.Invoke();
    private void OnAddPlantClicked() => AddPlantClicked?.Invoke();
    private void OnFilterClicked() => FilterClicked?.Invoke();
    private void OnSearchInputed(string search) => SearchInputed?.Invoke(search);
}
