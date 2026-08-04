using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterConfig", menuName = "Configs/Character Config")]

public class CharacterConfig : ScriptableObject
    {
        public string heroName;
        public float speed;
        public int damage;
        public int maxHeroHealth;
}

