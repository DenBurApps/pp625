using System;
using System.Linq;
using Bitsplash.DatePicker;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class AddCareScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _topText;
    [SerializeField] private DatePickerSettings _datePicker;
    [SerializeField] private Care _watering;
    [SerializeField] private Care _manuring;
    [SerializeField] private Care _plantCare;
    [SerializeField] private Temperature _temperature;
    [SerializeField] private Lightning _lightning;
    [SerializeField] private Transplantation _transplantation;
    [SerializeField] private ChooseCare _chooseCare;
    [SerializeField] private Button _careButton;
    [SerializeField] private ImagePlacer _imagePlacer;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private AddPlantScreen _addPlantScreen;
    [SerializeField] private Button _backButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    private FilledPlane _filledPlane;
    private PlantData _plantData;

    public event Action<WateringData> SavedWatering;
    public event Action<ManuringData> SavedManuring;
    public event Action<PlantCareData> PlantCareSaved;
    public event Action<TransplantationData> TransplantationSaved;
    public event Action<TemperatureData> TemperatureSaved;
    public event Action<LightningData> LightningSaved;

    public event Action<FilledPlane> NewDataSaved;
    public event Action DataDeleted;
    public event Action BackButtonClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    public void OpenCareScreen(FilledPlane filledPlane, CareData data)
    {
        _temperature.Disable();
        _manuring.Disable();
        _watering.Disable();
        _transplantation.Disable();
        _plantCare.Disable();
        _lightning.Disable();
        _chooseCare.Disable();
        _careButton.gameObject.SetActive(false);
        
        _filledPlane = filledPlane;
        _plantData = filledPlane.PlantData;

        ActivateCarePlane(data);

        _screenVisabilityHandler.EnableScreen();
        
        if (filledPlane.PlantData.ImagePath != null)
        {
            _imagePlacer.SetImage(filledPlane.PlantData.ImagePath);
        }
        else
        {
            _imagePlacer.gameObject.SetActive(false);
        }
        
        _backButton.onClick.AddListener(OnBackClicked);
    }

    private void ActivateCarePlane(CareData data)
    {
        if (data is WateringData wateringData)
        {
            _watering.Enable();
            _watering.SetData(wateringData);
        }
        else if (data is ManuringData manuringData)
        {
            _manuring.Enable();
            _manuring.SetData(manuringData);
        }
        else if (data is PlantCareData plantCareData)
        {
            _plantCare.Enable();
            _plantCare.SetData(plantCareData);
        }
        else if (data is TransplantationData transplantationData)
        {
            _transplantation.Enable();
            _transplantation.SetData(transplantationData);
        }
        else if (data is TemperatureData temperatureData)
        {
            _temperature.Enable();
            _temperature.SetData(temperatureData);
        }
        else if (data is LightningData lightningData)
        {
            _lightning.Enable();
            _lightning.SetData(lightningData);
        }
        else if(data == null)
        {
            _careButton.gameObject.SetActive(true);
        }

        _topText.text = "Care";
    }

    public void EnableCalendar()
    {
        _datePicker.gameObject.SetActive(true);
    }

    public void CloseCalendar()
    {
        _datePicker.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _watering.BackClicked += OnBackClicked;
        _watering.SavedClicked += SaveWateringData;
        _watering.CalendarOpened += _screenVisabilityHandler.SetTransperent;
        _watering.TimerOpened += _screenVisabilityHandler.SetTransperent;
        _watering.Enabled += _screenVisabilityHandler.EnableScreen;
        _watering.DateDeleted += DeleteWateringData;

        _manuring.BackClicked += OnBackClicked;
        _manuring.SavedClicked += SaveManuringData;
        _manuring.CalendarOpened += _screenVisabilityHandler.SetTransperent;
        _manuring.TimerOpened += _screenVisabilityHandler.SetTransperent;
        _manuring.Enabled += _screenVisabilityHandler.EnableScreen;
        _manuring.DateDeleted += DeleteManuringData;

        _plantCare.BackClicked += OnBackClicked;
        _plantCare.SavedClicked += SavedPlantCareData;
        _plantCare.CalendarOpened += _screenVisabilityHandler.SetTransperent;
        _plantCare.TimerOpened += _screenVisabilityHandler.SetTransperent;
        _plantCare.Enabled += _screenVisabilityHandler.EnableScreen;
        _plantCare.DateDeleted += DeletePlantCareData;

        _temperature.BackClicked += OnBackClicked;
        _temperature.SavedClicked += SaveTemperatureData;
        _temperature.Deleted += DeleteTemperatureData;

        _lightning.BackClicked += _screenVisabilityHandler.DisableScreen;
        _lightning.SavedClicked += SaveLightningData;
        _lightning.Deleted += DeleteLightningData;

        _transplantation.BackClicked += OnBackClicked;
        _transplantation.SavedClicked += SaveTransplantationData;
        _transplantation.TimerOpened += _screenVisabilityHandler.SetTransperent;
        _transplantation.Enabled += _screenVisabilityHandler.EnableScreen;
        _transplantation.CalendarOpened += _screenVisabilityHandler.SetTransperent;
        _transplantation.Deleted += DeleteTransplantationData;

        _careButton.onClick.AddListener(OpenChooseCare);

        _chooseCare.CategoryChosen += CategoryChosen;
        _chooseCare.Canceled += _screenVisabilityHandler.EnableScreen;

        _addPlantScreen.AddCare += OpenScreen;
    }

    private void OnDisable()
    {
        _watering.BackClicked -= OnBackClicked;
        _watering.SavedClicked -= SaveWateringData;
        _watering.CalendarOpened -= _screenVisabilityHandler.SetTransperent;
        _watering.TimerOpened -= _screenVisabilityHandler.SetTransperent;
        _watering.Enabled -= _screenVisabilityHandler.EnableScreen;
        _watering.DateDeleted -= DeleteWateringData;

        _manuring.BackClicked -= OnBackClicked;
        _manuring.SavedClicked -= SaveManuringData;
        _manuring.CalendarOpened -= _screenVisabilityHandler.SetTransperent;
        _manuring.TimerOpened -= _screenVisabilityHandler.SetTransperent;
        _manuring.Enabled -= _screenVisabilityHandler.EnableScreen;
        _manuring.DateDeleted -= DeleteWateringData;

        _plantCare.BackClicked -= OnBackClicked;
        _plantCare.SavedClicked -= SavedPlantCareData;
        _plantCare.CalendarOpened -= _screenVisabilityHandler.SetTransperent;
        _plantCare.TimerOpened -= _screenVisabilityHandler.SetTransperent;
        _plantCare.Enabled -= _screenVisabilityHandler.EnableScreen;
        _plantCare.DateDeleted -= DeleteWateringData;

        _temperature.BackClicked -= OnBackClicked;
        _temperature.SavedClicked -= SaveTemperatureData;
        _temperature.Deleted -= DeleteTemperatureData;

        _lightning.BackClicked -= OnBackClicked;
        _lightning.SavedClicked -= SaveLightningData;
        _lightning.Deleted += DeleteLightningData;

        _transplantation.BackClicked -= OnBackClicked;
        _transplantation.SavedClicked -= SaveTransplantationData;
        _transplantation.TimerOpened -= _screenVisabilityHandler.SetTransperent;
        _transplantation.Enabled -= _screenVisabilityHandler.EnableScreen;
        _transplantation.CalendarOpened -= _screenVisabilityHandler.SetTransperent;
        _transplantation.Deleted -= DeleteTransplantationData;

        _careButton.onClick.RemoveListener(OpenChooseCare);

        _chooseCare.CategoryChosen -= CategoryChosen;
        _chooseCare.Canceled -= _screenVisabilityHandler.EnableScreen;
        _addPlantScreen.AddCare -= OpenScreen;
        _backButton.onClick.RemoveListener(OnBackClicked);
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
        _chooseCare.Disable();

        _watering.SetDatePicker(_datePicker);
        _manuring.SetDatePicker(_datePicker);
        _plantCare.SetDatePicker(_datePicker);
        _transplantation.SetDatePicker(_datePicker);
        CloseCalendar();
    }

    private void OpenScreen(byte[] image, string name)
    {
        if (image != null)
        {
            _imagePlacer.SetImage(image);
        }
        else
        {
            _imagePlacer.gameObject.SetActive(false);
        }

        _name.text = name;

        _screenVisabilityHandler.EnableScreen();
        _temperature.Disable();
        _manuring.Disable();
        _watering.Disable();
        _transplantation.Disable();
        _plantCare.Disable();
        _lightning.Disable();
        _careButton.gameObject.SetActive(true);
        _topText.text = "New care";
    }

    private void OpenChooseCare()
    {
        _chooseCare.Enable();
        _screenVisabilityHandler.SetTransperent();
    }

    private void CategoryChosen(CareType type)
    {
        _chooseCare.Disable();

        _screenVisabilityHandler.EnableScreen();

        switch (type)
        {
            case CareType.Watering:
                _watering.Enable();
                break;
            case CareType.Manuring:
                _manuring.Enable();
                break;
            case CareType.PlantCare:
                _plantCare.Enable();
                break;
            case CareType.Temperature:
                _temperature.Enable();
                break;
            case CareType.Lightning:
                _lightning.Enable();
                break;
            case CareType.Transplantation:
                _transplantation.Enable();
                break;
            default:
                Debug.LogWarning("Unhandled CareType: " + type);
                break;
        }

        _careButton.gameObject.SetActive(false);
    }

    private void SaveWateringData()
    {
        var data = new WateringData(_watering.Time, _watering.Date, _watering.Days);

        if (_plantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<WateringData>().FirstOrDefault();
            if (existingData != null)
            {
                _plantData.CareDatas.Remove(existingData);
                _plantData.CareDatas.Add(data);
            }  
            else
            {
                _plantData.CareDatas.Add(data);
            }

            NewDataSaved?.Invoke(_filledPlane);
            _screenVisabilityHandler.DisableScreen();
            return;
        }

        _screenVisabilityHandler.DisableScreen();
        SavedWatering?.Invoke(data);
    }

    private void SaveManuringData()
    {
        var data = new ManuringData(_manuring.Time, _manuring.Date, _manuring.Days);

        if (_plantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<ManuringData>().FirstOrDefault();
            if (existingData != null)
            {
                _plantData.CareDatas.Remove(existingData);
                _plantData.CareDatas.Add(data);
            }  
            else
            {
                _plantData.CareDatas.Add(data);
            }

            NewDataSaved?.Invoke(_filledPlane);
            _screenVisabilityHandler.DisableScreen();
            return;
        }

        _screenVisabilityHandler.DisableScreen();
        SavedManuring?.Invoke(data);
    }

    private void SavedPlantCareData()
    {
        var data = new PlantCareData(_plantCare.Time, _plantCare.Date, _plantCare.Days);

        if (_plantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<PlantCareData>().FirstOrDefault();
            if (existingData != null)
            {
                _plantData.CareDatas.Remove(existingData);
                _plantData.CareDatas.Add(data);
            }  
            else
            {
                _plantData.CareDatas.Add(data);
            }

            NewDataSaved?.Invoke(_filledPlane);
            _screenVisabilityHandler.DisableScreen();
            return;
        }

        _screenVisabilityHandler.DisableScreen();
        PlantCareSaved?.Invoke(data);
    }

    private void SaveTransplantationData()
    {
        var data = new TransplantationData(_transplantation.Date, _transplantation.Time);

        if (_plantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<TransplantationData>().FirstOrDefault();
            if (existingData != null)
            {
                _plantData.CareDatas.Remove(existingData);
                _plantData.CareDatas.Add(data);
            }  
            else
            {
                _plantData.CareDatas.Add(data);
            }

            NewDataSaved?.Invoke(_filledPlane);
            _screenVisabilityHandler.DisableScreen();
            return;
        }

        _screenVisabilityHandler.DisableScreen();
        TransplantationSaved?.Invoke(data);
    }

    private void SaveTemperatureData()
    {
        var data = new TemperatureData(_temperature.TemperatureValue);

        if (_plantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<TemperatureData>().FirstOrDefault();
            if (existingData != null)
            {
                _plantData.CareDatas.Remove(existingData);
                _plantData.CareDatas.Add(data);
            }  
            else
            {
                _plantData.CareDatas.Add(data);
            }

            NewDataSaved?.Invoke(_filledPlane);
            _screenVisabilityHandler.DisableScreen();
            return;
        }

        _screenVisabilityHandler.DisableScreen();
        TemperatureSaved?.Invoke(data);
    }

    private void SaveLightningData()
    {
        var data = new LightningData(_lightning.Type);

        if (_plantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<LightningData>().FirstOrDefault();
            if (existingData != null)
            {
                _plantData.CareDatas.Remove(existingData);
                _plantData.CareDatas.Add(data);
            }  
            else
            {
                _plantData.CareDatas.Add(data);
            }

            NewDataSaved?.Invoke(_filledPlane);
            _screenVisabilityHandler.DisableScreen();
            return;
        }

        _screenVisabilityHandler.DisableScreen();
        LightningSaved?.Invoke(data);
    }

    private void DeleteWateringData()
    {
        if (_filledPlane.PlantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<WateringData>().FirstOrDefault();
            
            _plantData.CareDatas.Remove(existingData);
            _watering.ReturnToDefault();
            _watering.Disable();
            DataDeleted?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }
    }
    
    private void DeleteManuringData()
    {
        if (_filledPlane.PlantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<ManuringData>().FirstOrDefault();
            
            _plantData.CareDatas.Remove(existingData);
            _manuring.ReturnToDefault();
            _manuring.Disable();
            DataDeleted?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }
    }
    
    private void DeletePlantCareData()
    {
        if (_filledPlane.PlantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<PlantCareData>().FirstOrDefault();
            
            _plantData.CareDatas.Remove(existingData);
            _plantCare.ReturnToDefault();
            _plantCare.Disable();
            DataDeleted?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }
    }

    private void DeleteTransplantationData()
    {
        if (_filledPlane.PlantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<TransplantationData>().FirstOrDefault();
            
            _plantData.CareDatas.Remove(existingData);
            _transplantation.ReturnToDefault();
            _transplantation.Disable();
            DataDeleted?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }
    }
    
    private void DeleteTemperatureData()
    {
        if (_filledPlane.PlantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<TemperatureData>().FirstOrDefault();
            
            _plantData.CareDatas.Remove(existingData);
            _temperature.ReturnToDefault();
            _temperature.Disable();
            DataDeleted?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }
    }

    private void DeleteLightningData()
    {
        if (_filledPlane.PlantData != null)
        {
            var existingData = _plantData.CareDatas.OfType<LightningData>().FirstOrDefault();
            
            _plantData.CareDatas.Remove(existingData);
            _lightning.ReturnToDefault();
            _lightning.Disable();
            DataDeleted?.Invoke();
            _screenVisabilityHandler.DisableScreen();
        }
    }

    private void OnBackClicked()
    {
        _backButton.onClick.RemoveListener(OnBackClicked);
        BackButtonClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
        _plantData = null;
        _filledPlane = null;
    }
}