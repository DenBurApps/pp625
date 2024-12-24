using System;
using UnityEngine;
using UnityEngine.UI;

public class LightPlane : MonoBehaviour
{
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;
    [SerializeField] private Image[] _images;
    [SerializeField] private Button _openButton;

    public event Action<LightningData> Opened;
    
    public LightningData Data { get; private set; }
    public bool IsActive { get; private set; }

    private void OnEnable()
    {
        _openButton.onClick.AddListener(OnButtonClicked);

        foreach (var image in _images)
        {
            image.color = _unselectedColor;
        }
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(OnButtonClicked);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        IsActive = true;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        IsActive = false;
    }

    public void SetData(LightningData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        Data = data;

        if (Data.Type == LightType.Lamp)
        {
            _images[1].color = _selectedColor;
        }
        else if(Data.Type == LightType.Sunlight)
        {
            _images[0].color = _selectedColor;
        }
        else
        {
            _images[2].color = _selectedColor;
        }
    }

    private void OnButtonClicked() => Opened?.Invoke(Data);
}
