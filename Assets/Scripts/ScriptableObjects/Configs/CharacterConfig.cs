using UnityEngine;

    [CreateAssetMenu(fileName = "NewCharacterConfig", menuName = "Configs/Character Config")]
    public class CharacterConfig : ScriptableObject
    {
        public string characterName;
        public int maxHealth;
        public int damage;
        public float attackInterval;
        public float speedMovement;
    }
