using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSplitWindow : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Slider slider;
    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private Action<int> _onConfirm;

    private void OnEnable()
    {
        confirmButton.onClick.AddListener(HandleButtonConfirm);
        cancelButton.onClick.AddListener(HandleButtonCancel);
        slider.onValueChanged.AddListener(HandleSliderValueChange);
        addButton.onClick.AddListener(() => slider.value ++);
        removeButton.onClick.AddListener(() => slider.value --);
    }
    //maxAmount - то, что придет в ячейку
    public void ShowWindow(int maxAmount, Action<int> onConfirm) //Action нужен, чтобы передавать сколько отошло
    {
        _onConfirm = onConfirm;
        slider.minValue = 1;
        slider.maxValue = maxAmount;
        slider.value = maxAmount;
        
        amountText.text = maxAmount.ToString();
        root.SetActive(true);//показываем окошко
    }
    
    private void HandleButtonConfirm()
    {
        int amount = Mathf.RoundToInt(slider.value);
        _onConfirm?.Invoke(amount);
        HandleButtonCancel();
    }

    private void HandleButtonCancel()
    {
        root.SetActive(false);
        _onConfirm = null;
    }
    
    private void HandleSliderValueChange(float value)
    {
        amountText.text = Mathf.RoundToInt(value).ToString(CultureInfo.InvariantCulture);
    }
    
    private void OnDisable()
    {
        confirmButton.onClick.RemoveListener(HandleButtonConfirm);
        cancelButton.onClick.RemoveListener(HandleButtonCancel);
        slider.onValueChanged.RemoveListener(HandleSliderValueChange);
        addButton.onClick.RemoveAllListeners();
        removeButton.onClick.RemoveAllListeners();
    }
}
