using Code.Entities;
using UnityEngine;

namespace Code.SkillSystem.Crystals
{
    public abstract class CrystalController : MonoBehaviour
    {
        public CrystalSkill.CrystalType crystalType;
        [HideInInspector] public CrystalSkill skill;
        
        protected Entity _owner;

        public delegate void CrystalStatusChange(bool before, bool next);

        public event CrystalStatusChange OnCrystalStatusChange;
        public bool HasCrystal { get; protected set; }
        
        public virtual void SetUp(CrystalSkill crystalSkill, Entity owner)
        {
            skill = crystalSkill;
            _owner = owner;
        }

        public void SetCrystalStatus(bool status)
        {
            bool before = HasCrystal;
            HasCrystal = status;
            if(before != HasCrystal)
                OnCrystalStatusChange?.Invoke(before, HasCrystal);
        }
        
        
        public abstract void CreateCrystal();
        public abstract void UseCrystal();
    }
}