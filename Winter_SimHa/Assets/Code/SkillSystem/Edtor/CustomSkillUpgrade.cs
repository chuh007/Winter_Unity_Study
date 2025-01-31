using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.SkillSystem.Editor
{
    [UnityEditor.CustomEditor(typeof(SkillPerkUpgradeSO))]
    public class CustomSkillUpgradeEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset customTreeAsset;

        private SkillPerkUpgradeSO _skillPerkUpgradeSO;
        private Assembly _skillAssembly;
        private VisualElement _root;
        private DropdownField _fieldList;
        private IntegerField _intField;
        private FloatField _floatField;

        public override VisualElement CreateInspectorGUI()
        {
            _skillPerkUpgradeSO = target as SkillPerkUpgradeSO;

            _root = new VisualElement();
            InspectorElement.FillDefaultInspector(_root, serializedObject, this);
            customTreeAsset.CloneTree(_root);
            MakeSkillDropDown();

            EnumField upgradeTypeSelector = _root.Q<EnumField>("UpgradeType");
            upgradeTypeSelector.RegisterValueChangedCallback(HandleUpgradeTypeChange);

            _intField = _root.Q<IntegerField>("IntegerValue");
            _floatField = _root.Q<FloatField>("FloatValue");
            _fieldList = _root.Q<DropdownField>("FieldList");
            UpdateFieldList();

            return _root;
        }


        private void HandleUpgradeTypeChange(ChangeEvent<Enum> evt)
        {
            UpdateFieldList();

            switch (evt.newValue)
            {
                case SkillPerkUpgradeSO.UpgradeType.Boolean:
                    _intField.style.display = DisplayStyle.None;
                    _floatField.style.display = DisplayStyle.None;
                    _fieldList.style.display = DisplayStyle.Flex;
                    break;
                case SkillPerkUpgradeSO.UpgradeType.Integer:
                    _intField.style.display = DisplayStyle.Flex;
                    _floatField.style.display = DisplayStyle.None;
                    _fieldList.style.display = DisplayStyle.Flex;

                    break;
                case SkillPerkUpgradeSO.UpgradeType.Float:
                    _intField.style.display = DisplayStyle.None;
                    _floatField.style.display = DisplayStyle.Flex;
                    _fieldList.style.display = DisplayStyle.Flex;
                    break;
                case SkillPerkUpgradeSO.UpgradeType.Method:
                    _intField.style.display = DisplayStyle.None;
                    _floatField.style.display = DisplayStyle.None;
                    _fieldList.style.display = DisplayStyle.None;

                    break;
            }
        }

        private void UpdateFieldList()
        {
            _fieldList.choices = _skillPerkUpgradeSO.upgradeType switch
            {
                SkillPerkUpgradeSO.UpgradeType.Boolean => _skillPerkUpgradeSO.boolFields.Select(field => field.Name).ToList(),
                SkillPerkUpgradeSO.UpgradeType.Integer => _skillPerkUpgradeSO.intFields.Select(field => field.Name).ToList(),
                SkillPerkUpgradeSO.UpgradeType.Float => _skillPerkUpgradeSO.floatFields.Select(field => field.Name).ToList(),
                SkillPerkUpgradeSO.UpgradeType.Method => new List<string>(),
                _ => new List<string>()
            };

            if (_fieldList.choices.Count > 0 &&
                _fieldList.choices.Contains(_skillPerkUpgradeSO.selectFieldName) == false)
            {
                _skillPerkUpgradeSO.selectFieldName = _fieldList.choices[0];
                Debug.Log(_fieldList.choices[0]);
            }
        }

        private void MakeSkillDropDown()
        {
            DropdownField typeSelector = _root.Q<DropdownField>("TypeSelector");

            _skillAssembly = Assembly.GetAssembly(typeof(Skill));
            List<Type> derivedTypes = _skillAssembly.GetTypes()
                .Where(type => type.IsClass && type.IsAbstract == false && type.IsSubclassOf(typeof(Skill)))
                .ToList();

            derivedTypes.ForEach(type => typeSelector.choices.Add(type.FullName));

            typeSelector.RegisterValueChangedCallback(HandleTypeChange);
            typeSelector.SetValueWithoutNotify(_skillPerkUpgradeSO.targetSkill);

            _skillPerkUpgradeSO.targetSkill = typeSelector.value;
            _skillPerkUpgradeSO.GetFieldsFromTargetSkill();
        }

        private void HandleTypeChange(ChangeEvent<string> evt)
        {
            _skillPerkUpgradeSO.targetSkill = evt.newValue;
            _skillPerkUpgradeSO.GetFieldsFromTargetSkill();
            UpdateFieldList();
        }
    }
}