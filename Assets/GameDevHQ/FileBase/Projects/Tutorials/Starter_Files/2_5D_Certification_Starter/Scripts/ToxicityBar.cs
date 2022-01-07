﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToxicityBar : MonoBehaviour
{
    [SerializeField] private Slider _toxicityBar;
    [SerializeField] private int _currentToxicity;
    [SerializeField] private int _maxToxicity;
    [SerializeField] private int _minToxicity;

    void Start()
    {
        _currentToxicity = _minToxicity;
        _toxicityBar.maxValue = _maxToxicity;
        _toxicityBar.minValue = _minToxicity;
        _toxicityBar.value = _currentToxicity;
    }

    public void ToxicExposure(int amount)
    {
        if(_currentToxicity + amount <= _maxToxicity)
        {
            _currentToxicity += amount;
            _toxicityBar.value = _currentToxicity;
        } 
    }

    public void ToxicCure(int amount)
    {
        if(_currentToxicity - amount >= _minToxicity)
        {
            _currentToxicity -= amount;
            _toxicityBar.value = _currentToxicity;
        }
      
    }
}
