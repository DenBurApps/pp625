using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CarePlane : MonoBehaviour
{
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private Image[] _dayImages;
    [SerializeField] private Button _openButton;

    public event Action<CareData> Opened; 
    
    public CareData CareData { get; private set; }
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

    public void SetData(CareData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        CareData = data;
        
        _timeText.text = data.FilledTime;
        _dateText.text = data.FilledDate;
        
        foreach (var dayImage in _dayImages)
        {
            dayImage.color = _unselectedColor;
        }
        
        HighlightSelectedDays();
    }
    
    private void HighlightSelectedDays()
    {
        List<string> filledDays = null;
        
        if (CareData is WateringData wateringData)
        {
            filledDays = wateringData.FilledDays;
        }
        else if (CareData is ManuringData manuringData)
        {
            filledDays = manuringData.FilledDays;
        }
        else if (CareData is PlantCareData plantCareData)
        {
            filledDays = plantCareData.FilledDays;
        }
    
        if (filledDays == null)
            return;

        foreach (var day in filledDays)
        {
            
            foreach (var dayImage in _dayImages)
            {
                var dayText = dayImage.GetComponentInChildren<TMP_Text>().text;
                Debug.Log(dayText);
                if (day.Equals(dayText, StringComparison.OrdinalIgnoreCase))
                {
                    dayImage.color = _selectedColor;
                    break;
                }
            }
        }
    }

    private void OnButtonClicked() => Opened?.Invoke(CareData);
}