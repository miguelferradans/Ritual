﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCircle : MonoBehaviour {
    // Constantes públicas
    public const float RUNE_ALPHA_ACTIVE = 1f;
    public const float RUNE_ALPHA_UNACTIVE = 0.5f;
    public int manaPenalization = 20;
    public int manaPenalizationPerRound = 5;

    // Variables publicas

    // Variables privadas
    static SpellCircle instance = null;
    List<SpellRune.Rune> _spell = new List<SpellRune.Rune>();
    SpellTree _grimorium = new SpellTree();

    private Torre Torre;

    // Metodos Awake, Start, Update....

    void Start()
    {
        GameObject torre = GameObject.FindGameObjectWithTag("Torre");
        if (torre != null)
            Torre = torre.GetComponent<Torre>();
    }

    // Use this for spawn this instance
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Hacemos que las runas empicen a escuchar el evento MouseEnter
            gameObject.BroadcastMessage("Available");
        }
        if (Input.GetMouseButtonUp(0))
        {
            // Notificar a las runas que vuelvan a su estado desactivado
            gameObject.BroadcastMessage("Deactivate");
            
            // Buscar el hechizo
            GameObject spellPrefab = _grimorium.FindSpell(_spell);
            // Hacer la magia
            if (spellPrefab != null)
            {
                if (Torre != null)
                {
                    if (Torre.TowerCurrentMana >= spellPrefab.GetComponent<Spell>().manaCost)
                    {
                        Torre.TowerCurrentMana -= spellPrefab.GetComponent<Spell>().manaCost;
                        GameObject.Instantiate(spellPrefab);
                    }
                    else
                        NotEnoughtMana();
                }
                else
                    GameObject.Instantiate(spellPrefab);
            }
            else
            {
                if (_spell.Count > 0 && Torre != null)
                {
                    AudioManager.Instance.Play("spellNot");
                    Torre.TowerCurrentMana -= manaPenalization + GameManager.Round * manaPenalizationPerRound;
                    if (Torre.TowerCurrentMana < 0)
                    {
                        Torre.TowerCurrentMana = 0;
                    }
                }
            }

            // Desactivar el círculo de los conjuros
            _spell.Clear();
            gameObject.SetActive(false);

        }
    }

    // Otros métodos publicos

    /// <summary>
    /// Añade una runa al hechizo actual
    /// </summary>
    /// <param name="rune">Runa a añadir</param>
    public static void AddRune(SpellRune.Rune rune)
    {
        instance._spell.Add(rune);
    }

    /// <summary>
    /// Añade un hechizo al grimorio
    /// </summary>
    /// <param name="spell">Hechizo a añadir</param>
    public static void LearnSpell (GameObject spell){
        instance._grimorium.AddSpell(spell);
    }

    /// <summary>
    /// Añade un hechizo al grimorio
    /// </summary>
    /// <param name="spell">Hechizo a añadir</param>
    public void learnSpell(GameObject spell)
    {
        _grimorium.AddSpell(spell);
    }

    // Otros metodos privados

    private void NotEnoughtMana()
    {
        AudioManager.Instance.Play("outOfMana");
    }
}

public class SpellTree
{
    class Nodo
    {
        public SpellRune.Rune rune;
        public GameObject spellPrefab = null;
        public Dictionary<SpellRune.Rune,Nodo> nextRunes = new Dictionary<SpellRune.Rune,Nodo>();
    }

    Nodo raiz = new Nodo();

    public void AddSpell(GameObject spellPrefab)
    {
        var runes = spellPrefab.GetComponent<Spell>().runes;

        Nodo actualNode = raiz;
        foreach (SpellRune.Rune rune in runes)
        {
            Nodo nextNode;
            if (actualNode.nextRunes.TryGetValue(rune,out nextNode))
            {
                actualNode = nextNode;
            }
            else
            {
                Nodo newNode = new Nodo();
                newNode.rune = rune;
                actualNode.nextRunes.Add(rune, newNode);
                actualNode = actualNode.nextRunes[rune];
            }
        }
        actualNode.spellPrefab = spellPrefab;
    }

    public GameObject FindSpell(List<SpellRune.Rune> spell)
    {
        Nodo actualNode = raiz;
        foreach (SpellRune.Rune rune in spell)
        {
            Nodo nextNode;
            if (actualNode.nextRunes.TryGetValue(rune, out nextNode))
            {
                actualNode = nextNode;
            }
            else
            {
                return null;
            }
        }
        return actualNode.spellPrefab;
    }
}
