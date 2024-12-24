using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransplantationPlane : MonoBehaviour
{
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Button _openButton;

    public event Action<TransplantationData> Opened; 
    
    public TransplantationData CareData { get; private set; }
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

    public void SetData(TransplantationData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        CareData = data;

        _dateText.text = CareData.Date;
        _timeText.text = CareData.Time;
    }
    
    private void OnButtonClicked() => Opened?.Invoke(CareData);
}
