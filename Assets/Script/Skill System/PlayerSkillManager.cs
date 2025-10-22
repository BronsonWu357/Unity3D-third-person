using UnityEngine;
using UnityEngine.Events;

public class PlayerSkillManager : MonoBehaviour
{
    private int _strength;

    private int _health;

    private int _speed;

    private int _attackSpeed;

    private int _defense;

    private int _CD;

    private int _doubleJump;

    private int _dash;

    private int _teleport;

    private int _skillPoint;


    public int Strength => _strength;

    public int Health => _health;

    public int Speed => _speed;

    public int AttackSpeed => _attackSpeed;

    public int Defense => _defense;

    public int CD => _CD;


    public bool DoubleJump => _doubleJump > 0;

    public bool Dash => _dash > 0;

    public bool Teleport => _teleport > 0;

    public int SkillPoint => _skillPoint;

    public UnityAction OnSkillChanged;

    public void Awake()
    {
        _skillPoint = 3;

        _strength = 0;
        
        _health = 0;

        _speed = 0;

        _attackSpeed = 0;

        _defense = 0;

        _CD = 0;

        _doubleJump = 0;

        _dash = 0;

        _teleport = 0;
    }

    

}
