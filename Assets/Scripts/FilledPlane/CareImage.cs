using UnityEngine;
using UnityEngine.UI;

public class CareImage : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Sprite _unselectedSprite;
    [SerializeField] private Color _unfilledColor;
    [SerializeField] private Color _filledColor;
    [SerializeField] private CareType _careType;
    
    public CareType Type => _careType;

    public void SetSelected()
    {
        _image.sprite = _selectedSprite;
        _image.color = _filledColor;
    }

    public void SetFilled()
    {
        _image.sprite = _unselectedSprite;
        _image.color = _filledColor;
    }

    public void SetNotFilled()
    {
        _image.sprite = _unselectedSprite;
        _image.color = _unfilledColor;
    }
}

public enum CareType
{
    Watering,
    Manuring,
    PlantCare,
    Transplantation,
    Temperature,
    Lightning
}
