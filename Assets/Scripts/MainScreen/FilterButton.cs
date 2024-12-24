using System;
using System.Collections.Generic;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class FilterButton : MonoBehaviour
{
    [SerializeField] private ChoseCareMainScreen _choseCareMainScreen;
    [SerializeField] private ChooseCategoryScreen _chooseCategoryScreen;
    [SerializeField] private CategoryImage _categoryImage;
    [SerializeField] private CareImage[] _careImages;
    [SerializeField] private Button _refreshButton;
    [SerializeField] private Image _defaultImage;

    private Button _selectButton;

    private List<CareType> _chosenTypes = new List<CareType>();

    public event Action ChooseOpened;
    public event Action ReturnedToDefault;
    public event Action AllChosen;

    public PlantCategory ChosenCategory { get; private set; }
    public IReadOnlyCollection<CareType> ChosenTypes => _chosenTypes;

    private void Awake()
    {
        _selectButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _selectButton.onClick.AddListener(OnSelectionClicked);
        
        _chooseCategoryScreen.CategoryChosen += CategoryChosen;
        _chooseCategoryScreen.Canceled += CancelCategory;
        
        _choseCareMainScreen.CategoryClicked += OpenCategory;
        _choseCareMainScreen.CaresChosen += CaresChosen;
        
        _refreshButton.onClick.AddListener(ReturnToDefault);
    }

    private void OnDisable()
    {
        _selectButton.onClick.RemoveListener(OnSelectionClicked);
        
        _chooseCategoryScreen.CategoryChosen -= CategoryChosen;
        _chooseCategoryScreen.Canceled -= CancelCategory;
        
        _choseCareMainScreen.CategoryClicked -= OpenCategory;
        _choseCareMainScreen.CaresChosen -= CaresChosen;
        
        _refreshButton.onClick.RemoveListener(ReturnToDefault);
    }

    private void Start()
    {
        _defaultImage.enabled = true;

        foreach (var image in _careImages)
        {
            image.SetNotFilled();
            image.GetComponent<BetterImage>().enabled = false;
        }
        
        _refreshButton.gameObject.SetActive(false);
        
        _chooseCategoryScreen.Disable();
        _choseCareMainScreen.Disable();

        ChosenCategory = PlantCategory.None;
        _chosenTypes.Clear();
    }

    private void ReturnToDefault()
    {
        _defaultImage.enabled = true;

        foreach (var image in _careImages)
        {
            image.SetNotFilled();
            image.GetComponent<BetterImage>().enabled = false;
        }
        
        _refreshButton.gameObject.SetActive(false);
        
        _chooseCategoryScreen.Disable();
        _choseCareMainScreen.Disable();

        ChosenCategory = PlantCategory.None;
        _chosenTypes.Clear();
        ReturnedToDefault?.Invoke();
    }

    public void ResetCategory()
    {
        ChosenCategory = PlantCategory.None;
        AllChosen?.Invoke();
    }

    private void OnSelectionClicked()
    {
        _choseCareMainScreen.Enable();
        ChooseOpened?.Invoke();
    }

    private void OpenCategory()
    {
        _chooseCategoryScreen.Enable();
        _choseCareMainScreen.Disable();
    }

    private void CategoryChosen(PlantCategory category)
    {
        _categoryImage.SetImage(category);
        ChosenCategory = category;
        _chooseCategoryScreen.Disable();
        _choseCareMainScreen.Enable();
    }

    private void CancelCategory()
    {
        _chooseCategoryScreen.Disable();
        _choseCareMainScreen.Enable();
    }

    private void CaresChosen(List<CareType> careTypes)
    {
        _chosenTypes = careTypes;

        foreach (var image in _careImages)
        {
            image.SetNotFilled();
            image.GetComponent<BetterImage>().enabled = false;
        }
        
        foreach (var careType in _chosenTypes)
        {
            foreach (var image in _careImages)
            {
                if (image.Type == careType)
                {
                    image.SetSelected();
                    image.GetComponent<BetterImage>().enabled = true;
                    break;
                }
            }
        }
        
        _refreshButton.gameObject.SetActive(true);
        _defaultImage.enabled = false;
        _chooseCategoryScreen.Disable();
        _choseCareMainScreen.Disable();
        AllChosen?.Invoke();
    }
}
