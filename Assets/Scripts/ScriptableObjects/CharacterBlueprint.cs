using UnityEngine;

namespace Vampire
{
    [CreateAssetMenu(fileName = "Character", menuName = "Blueprints/Character", order = 1)]
    public class CharacterBlueprint : ScriptableObject
    {
        public new string name;
        
        [Tooltip("Character mở sẵn không cần mua (starter character). Runtime ownership được lưu trong PlayerPrefs.")]
        public bool owned = false;
        
        [Tooltip("Giá mua character. Nếu cost = 0 thì character miễn phí.")]
        public int cost = 999;
        public float hp;
        public float recovery;
        public int armor;
        public float movespeed;
        public float luck;
        public float acceleration;
        public Sprite[] walkSpriteSequence;
        public float walkFrameTime;
        public GameObject[] startingAbilities;

        public float LevelToExpIncrease(int level)
        {
            if (level < 10)
                return 10;
            if (level < 20)
                return 13;
            if (level < 30)
                return 16;
            else
                return 20;
        }
    }
}
