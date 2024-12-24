using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenDateElement : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _selectedDayTextColor;
    [SerializeField] private Color _selectedDateTextColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;
    [SerializeField] private Color _unselectedDayTextColor;
    [SerializeField] private Color _unselectedDateTextColor;
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private Button _selectionButton;

    private bool _isSelected;
    
    public string Date { get; private set; }
    public string Day { get; private set; }

    public event Action<MainScreenDateElement> ElementClicked;
    
    private void OnEnable()
    {
        _selectionButton.onClick.AddListener(OnButtonClicked);
        
        Reset();
    }

    private void OnDisable()
    {
        _selectionButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void SetDatesText(string date, string day)
    {
        Date = date;
        Day = day;
        
        _dateText.text = date;
        _dayText.text = day;
    }

    public void Reset()
    {
        _dayText.color = _unselectedDayTextColor;
        _dateText.color = _unselectedDateTextColor;
        _image.color = _unselectedColor;
        _isSelected = false;
    }

    private void OnButtonClicked()
    {
        if (!_isSelected)
        {
            _dayText.color = _selectedDayTextColor;
            _dateText.color = _selectedDateTextColor;
            _image.color = _selectedColor;
            _isSelected = true;
            ElementClicked?.Invoke(this);
        }
        else
        {
            _dayText.color = _unselectedDayTextColor;
            _dateText.color = _unselectedDateTextColor;
            _image.color = _unselectedColor;
            _isSelected = false;
        }
    }
}
