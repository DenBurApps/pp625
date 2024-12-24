using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TemperaturePlane : MonoBehaviour
{
    [SerializeField] private TMP_Text _temperatureText;
    [SerializeField] private Button _openButton;
    
    public event Action<TemperatureData> Opened;
    
    public TemperatureData Data { get; private set; }
    public bool IsActive { get; private set; }
    
    private void OnEnable()
    {
        _openButton.onClick.AddListener(OnButtonClicked);
        IsActive = true;
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(OnButtonClicked);
        IsActive = false;
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void SetData(TemperatureData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        Data = data;

        _temperatureText.text = Data.Temperature;
    }

    private void OnButtonClicked() => Opened?.Invoke(Data);
}
