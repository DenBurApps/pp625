using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilledPlane : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private CategoryImage _categoryImage;
    [SerializeField] private Image _plantImage;
    [SerializeField] private TMP_Text _nextCareDate;
    [SerializeField] private CareImage[] _careImages;
    [SerializeField] private Button _openButton;
    [SerializeField] private ImagePlacer _imagePlacer;

    public event Action<FilledPlane> Opened;
    public event Action Updated;

    public PlantData PlantData { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime NextCareDate { get; private set; }

    private void OnEnable()
    {
        _openButton.onClick.AddListener(OnButtonClicked);
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

    public void SetData(PlantData plantData)
    {
        if (plantData == null)
            throw new ArgumentNullException(nameof(plantData));

        PlantData = plantData;

        _nameText.text = PlantData.Name;

        if (PlantData.ImagePath == null)
        {
            _plantImage.enabled = false;
        }
        else
        {
            _imagePlacer.SetImage(PlantData.ImagePath);
            _plantImage.enabled = true;
        }

        _categoryImage.SetImage(PlantData.Category);

        foreach (var image in _careImages)
        {
            foreach (var careType in PlantData.CareDatas)
            {
                if (image.Type == careType.FilledType)
                {
                    image.SetFilled();
                }
                else
                {
                    image.SetNotFilled();
                }
            }
        }

        CalculateNextCareDate();
    }

    public void UpdateUI()
    {
        _nameText.text = PlantData.Name;

        if (PlantData.ImagePath == null)
        {
            _plantImage.enabled = false;
        }
        else
        {
            _imagePlacer.SetImage(PlantData.ImagePath);
            _plantImage.enabled = true;
        }

        _categoryImage.SetImage(PlantData.Category);

        foreach (var image in _careImages)
        {
            foreach (var careType in PlantData.CareDatas)
            {
                if (image.Type == careType.FilledType)
                {
                    image.SetFilled();
                }
                else
                {
                    image.SetNotFilled();
                }
            }
        }

        CalculateNextCareDate();
        Updated?.Invoke();
    }

    private void CalculateNextCareDate()
    {
        List<DateTime> careDates = new List<DateTime>();
        
        string dateFormat = "dd.MM.yyyy";
        
        foreach (var careData in PlantData.CareDatas)
        {
            DateTime careDate;
            
            if (careData is WateringData wateringData && DateTime.TryParseExact(wateringData.Date, dateFormat, null, System.Globalization.DateTimeStyles.None, out careDate) ||
                careData is ManuringData manuringData && DateTime.TryParseExact(manuringData.Date, dateFormat, null, System.Globalization.DateTimeStyles.None, out careDate) ||
                careData is TransplantationData transplantationData && DateTime.TryParseExact(transplantationData.Date, dateFormat, null, System.Globalization.DateTimeStyles.None, out careDate) ||
                careData is PlantCareData plantCareData && DateTime.TryParseExact(plantCareData.Date, dateFormat, null, System.Globalization.DateTimeStyles.None, out careDate))
            {
                careDates.Add(careDate);
            }
        }
        
        if (careDates.Count > 0)
        {
            DateTime nearestCareDate = careDates.Min();
            _nextCareDate.text = nearestCareDate.ToString("dd.MM.yyyy");
            NextCareDate = nearestCareDate;
        }
        else
        {
            _nextCareDate.text = "No upcoming care needed";
        }
    }

    private void OnButtonClicked() => Opened?.Invoke(this);
}