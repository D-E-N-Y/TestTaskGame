using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _hpBar;
    private Camera _cam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    
    private void Start() 
    {
        _cam = Camera.main;
    }

    private void Update() 
    {
        transform.rotation = _cam.transform.rotation;
        transform.position = target.position + offset;
    }

    public void UpdateHealthBar(float currentHP, float maxHP)
    {
        _hpBar.value = currentHP / maxHP;
    }
}
