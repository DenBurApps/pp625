using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Temperature : MonoBehaviour
{
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private TMP_InputField _temperatureInput;
    
    private string _temperature;
    public event Action SavedClicked;
    public event Action BackClicked;
    public event Action Deleted;
    
    public string TemperatureValue => _temperature;

    private void OnEnable()
    {
        _saveButton.onClick.AddListener(OnSaveClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _temperatureInput.onValueChanged.AddListener(OnTemperatureChanged);
        _deleteButton.onClick.AddListener(OnDeleted);
        
    }

    private void OnDisable()
    {
        _saveButton.onClick.RemoveListener(OnSaveClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _temperatureInput.onValueChanged.RemoveListener(OnTemperatureChanged);
        _deleteButton.onClick.RemoveListener(OnDeleted);
    }
    
    private void Start()
    {
        _deleteButton.gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        ValidateSaveButton();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    
    public void SetData(TemperatureData data)
    {
        _temperature = data.Temperature;
        _temperatureInput.text = data.Temperature;
        _deleteButton.gameObject.SetActive(true);
    }
    
    public void ReturnToDefault()
    {
        _temperatureInput.text = string.Empty;
        _temperature = string.Empty;
        _deleteButton.gameObject.SetActive(false);
    }
    
    private void OnSaveClicked()
    {
        SavedClicked?.Invoke();
    }

    private void OnBackButtonClicked()
    {
        ReturnToDefault();
        BackClicked?.Invoke();
    }
    
    private void ValidateSaveButton()
    {
        _saveButton.interactable = !string.IsNullOrEmpty(_temperature);
    }

    private void OnTemperatureChanged(string text)
    {
        _temperature = text;
        ValidateSaveButton();
    }

    private void OnDeleted()
    {
        Deleted?.Invoke();
    }
}
