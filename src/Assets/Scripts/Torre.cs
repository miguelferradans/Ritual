﻿using UnityEngine;
using System.Collections;




public class Torre : MonoBehaviour {
    [Header("Tower Properties")]
    [SerializeField]
    float _towerDamage;
    [SerializeField]
    float _towerCooldown;

    [SerializeField]
    double _hpPerLevel;
    [SerializeField]
    double _manaPerLevel;

    [System.Serializable]
    public class spritesTower
    {
        public Sprite m_sprite;
        public double life;
    }

    public spritesTower[] sprites;
    
    [SerializeField]
    Sprite _healthyTower;
    [SerializeField]
    double manaRegen = 1;


    public double TowerMaxHP = 100;
    public double TowerMaxMana = 100;
    public double TowerCurrentHP;
    public double TowerCurrentMana;

    public GameObject projectile;
    public Vector3 _projectileSpawnDeviation;
    

    private SpriteRenderer _towerSprite;
    private float _towerCooldownTimer;
    // Use this for initialization
    void Start () {
        TowerCurrentHP = TowerMaxHP;
        _towerSprite = GetComponent<SpriteRenderer>();
        if (_towerSprite.sprite == null)
            _towerSprite.sprite = _healthyTower;

        _towerCooldownTimer = _towerCooldown;
        GameManager.OnRoundChange += OnRoundChange;
    }

    void Update()
    {
        _towerCooldownTimer += Time.deltaTime;
        TowerCurrentMana += manaRegen * Time.deltaTime;
        TowerCurrentMana = (TowerCurrentMana<TowerMaxMana)? TowerCurrentMana : TowerMaxMana;
    }

    // Función para registrar el daño que recibe la torre
    public void HitEnemy(double damageReceived)
    {
        TowerCurrentHP -= damageReceived;
        if (TowerCurrentHP > (TowerMaxHP * sprites[sprites.Length - 1].life) && _towerSprite.sprite != _healthyTower)
            _towerSprite.sprite = _healthyTower;
        else
            this.changeSprite();
        
    }


    // Función para registrar las reparaciones que hace el jugador a la torre
    public void repairTower(float percentage)
    {
        double repairAmount = TowerMaxHP * percentage;
        if (TowerCurrentHP < TowerMaxHP) // Solo se repara si la vida actual es menor que la vida máxima
        {
            TowerCurrentHP += repairAmount;

            if (TowerCurrentHP > TowerMaxHP) // Si la reparación pone la vida por encima de la vida máxima, se iguala con la vida máxima
                TowerCurrentHP = TowerMaxHP;

            if (TowerCurrentHP > (TowerMaxHP * sprites[sprites.Length - 1].life) && _towerSprite.sprite != _healthyTower)
                _towerSprite.sprite = _healthyTower;
            else
                this.changeSprite();
        }
    }

    public void restoreMana(float percentage)
    {
        double restoredMana = TowerMaxMana * percentage;
        if (TowerCurrentMana < TowerMaxMana) // Solo se repara si la vida actual es menor que la vida máxima
        {
            TowerCurrentMana += restoredMana;

            if (TowerCurrentMana > TowerMaxMana) // Si la reparación pone la vida por encima de la vida máxima, se iguala con la vida máxima
                TowerCurrentMana = TowerMaxMana;
        }
    }

    void OnRoundChange()
    {
        TowerMaxHP += _hpPerLevel;
        TowerMaxMana += _manaPerLevel;
    }

    void changeSprite()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (TowerCurrentHP <= (TowerMaxHP * sprites[i].life))
            {
                _towerSprite.sprite = sprites[i].m_sprite;
                break;
            }
        }
    }

    public void registerMouse()
    {

        if (_towerCooldownTimer >= _towerCooldown)
        {
                Vector3 _throwPosition = transform.position + _projectileSpawnDeviation;
                Vector3 _directionVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _throwPosition;

                GameObject newProjectile = Instantiate(projectile, _throwPosition, Quaternion.identity) as GameObject;
                newProjectile.GetComponent<ProjectileManager>().initParameters(_towerDamage, _directionVector, this.gameObject.tag);
                _towerCooldownTimer = 0.0f;

        }
       
    }



}
