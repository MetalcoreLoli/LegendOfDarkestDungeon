using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core;
using UnityEngine;

public class SpellsShortcutMenu : MonoBehaviour
{
    [SerializeField] private Transform _spellSelector = null;

    [SerializeField] private List<SpellCell> _spells = new List<SpellCell>();

    // Start is called before the first frame update
    void Start()
    {
        if (_spellSelector is null)
            throw new NullReferenceException("There is no spell selector");
        _spellSelector.position = _spells.FirstOrDefault().Link.position;
    }

    // Update is called once per frame
    void Update()
    {
        var selectedSpell = _spells.Single(s => s.IsSelected);
        var chosenSpell = _spells.SingleOrDefault(s => GameInput.GetKeyDown(s.Key));
        if (chosenSpell is null) return;
        if (!selectedSpell.Key.Equals(chosenSpell.Key))
        {
            selectedSpell.IsSelected = false;
            chosenSpell.IsSelected = true;
        }
        _spellSelector.position = chosenSpell.Link.position;
    }
    
    [Serializable]
    private class SpellCell
    {
        [SerializeField]private bool _isSelected;
        [SerializeField]private Transform _link;
        [SerializeField]private string _key;
        public Transform Link => _link;
        public bool IsSelected
        {
            get => _isSelected;
            set => _isSelected = value;
        }

        public string Key => _key;

        public  SpellCell(Transform link, string key)
        {
            (_link) = (link);
            _key = key;
            _isSelected = false;
        }
    }
}
