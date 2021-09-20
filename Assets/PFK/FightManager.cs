using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using PFK;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WordBearers
{
    public class EnemySkeleton
    {
        public UnitIDEnum ID;
        public BaseStats Stats;
    }

    public enum AttackType
    {
        Miss = 0,
        Hit = 1,
        Crit = 2
    }

    public class FightAction
    {
        public AttackType Type;
        public UnitIDEnum AttackerID;
        public UnitIDEnum VictimID;
        public int Damage;
        public bool Vampirism;
        public bool Reflect;
    }

    public class FightManager : MonoBehaviour
    {
        [Header("Scenes")] 
        [SerializeField] private string _victoryScene;
        [SerializeField] private string _drawScene;
        [SerializeField] private string _diedScene;
        
        [SerializeField] private FightScene.Character[] _characters;
        
        [DllImport("__Internal")]
        private static extern void jsPrintString(string str);
        
        private SeedReader _seedReader;

        private readonly Queue<FightAction> _fightQueue = new Queue<FightAction>();

        private void Start()
        {
            foreach (var character in _characters)
            {
                character.gameObject.SetActive(false);
            }

            Fight fight = Fight.Instance;
            _seedReader = new SeedReader(fight.FightParams.Seed);

            BaseStats baseStats = fight.FightParams.Stats;
            bool[] playerBuffs = fight.FightParams.Buffs;
            
            EnemySkeleton[] skeletons = GetSkeletonsByLevel(fight.FightParams.OldState.level);

            foreach (var skeleton in skeletons)
            {
                _characters[(int)skeleton.ID].UpdateStats(skeleton.Stats);
            }

            BaseStats.ApplyBuffs(baseStats, playerBuffs);
            
            _characters[(int)UnitIDEnum.Player].UpdateStats(baseStats);

            QueueAllAttacks(baseStats, skeletons, playerBuffs);
            
            StartCoroutine(FightCoroutine());
        }

        private void QueueAllAttacks(BaseStats baseStats, EnemySkeleton[] skeletons, bool[] playerBuffs)
        {
            bool[] skeletonBuffs = new bool[playerBuffs.Length];
            
            jsPrintString($"Player stats: { baseStats.Attack } - { baseStats.Health } - { baseStats.Armour }");
            jsPrintString($"Number of skeletons: { skeletons.Length }");
            
            for (int i = 0; i < 10; ++i)
            {
                int index = _seedReader.Roll((byte)skeletons.Length);
                EnemySkeleton target = skeletons[index];
                
                EnqueueAttack(UnitIDEnum.Player, baseStats, playerBuffs,
                    target.ID, target.Stats, skeletonBuffs);
                
                skeletons = RecountSkeletons(skeletons);
                jsPrintString($"Number of skeletons: { skeletons.Length }");

                foreach (EnemySkeleton attacker in skeletons)
                {
                    EnqueueAttack(attacker.ID, attacker.Stats, skeletonBuffs,
                        UnitIDEnum.Player, baseStats, playerBuffs);

                    if (baseStats.Health <= 0) return;
                }

                skeletons = RecountSkeletons(skeletons);
                
                if (skeletons.Length == 0) return;
            }
        }

        private EnemySkeleton[] RecountSkeletons(EnemySkeleton[] skeletons)
        {
            return skeletons.Where(_ => _.Stats.Health > 0).ToArray();
        }

        private EnemySkeleton[] GetSkeletonsByLevel(int level)
        {
            switch (level)
            {
                case 0:
                    return new[] { GetWeak() };
                case 1:
                    return new[] { GetQuick() };
                case 2:
                    return new[] { GetThick() };
                case 3:
                    return new[] { GetWeak(), GetQuick() };
                case 4:
                    return new[] { GetWeak(), GetThick() };
                case 5:
                    return new[] { GetQuick(), GetThick() };
                case 6:
                    return new[] { GetWeak(), GetQuick(), GetThick() };
                default:
                    throw new System.Exception("Wrong level");
            }
        }

        private EnemySkeleton GetWeak()
        {
            int difficulty = Fight.Instance.FightParams.OldState.difficulty;
            return new EnemySkeleton()
            {
                ID = UnitIDEnum.Weak, 
                Stats = new BaseStats()
                {
                    Attack = 1 + difficulty, 
                    Health = 1 + difficulty, 
                    Armour = 0 + difficulty
                }
            };
        }
        
        private EnemySkeleton GetThick()
        {
            int difficulty = Fight.Instance.FightParams.OldState.difficulty;
            return new EnemySkeleton()
            {
                ID = UnitIDEnum.Thick, 
                Stats = new BaseStats()
                {
                    Attack = 2 + difficulty, 
                    Health = 3 + difficulty, 
                    Armour = 1 + difficulty
                }
            };
        }
        
        private EnemySkeleton GetQuick()
        {
            int difficulty = Fight.Instance.FightParams.OldState.difficulty;
            return new EnemySkeleton()
            {
                ID = UnitIDEnum.Quick, 
                Stats = new BaseStats()
                {
                    Attack = 2 + difficulty, 
                    Health = 1 + difficulty, 
                    Armour = 0 + difficulty
                }
            };
        }

        private void EnqueueAttack(UnitIDEnum attackerID, BaseStats attacker, bool[] buffsAttacker,
            UnitIDEnum victimID, BaseStats victim, bool[] buffsVictim)
        {
            FightAction action = new FightAction()
            {
                AttackerID = attackerID,
                VictimID = victimID
            };
            
            jsPrintString($"Actual Attack form {attackerID} to {victimID}");
            jsPrintString($"{attacker.Attack} {attacker.Health} {attacker.Armour}");
            jsPrintString($"{victim.Attack} {victim.Health} {victim.Armour}");

            int damage = attacker.Attack;

            int hit = _seedReader.Roll(2);
            int crit = _seedReader.Roll(2);
            
            if (buffsAttacker[(int) SpellType.CriticalStrike])
                crit = 1;

            if (hit > 0)
            {
                action.Type = AttackType.Hit;

                if (crit > 0)
                    action.Type = AttackType.Crit;
                else 
                    damage -= victim.Armour;
                
                if (damage > 0)
                {
                    victim.Health -= damage;

                    if (crit > 0 && buffsAttacker[(int) SpellType.Vampirism])
                    {
                        action.Vampirism = true;
                        attacker.Health += 1;
                    }

                    if (buffsVictim[(int) SpellType.Reflect])
                    {
                        action.Reflect = true;
                        attacker.Health -= 1;
                    }
                }
            }
            else
            {
                damage = 0;
                action.Type = AttackType.Miss;
            }

            action.Damage = Mathf.Clamp(damage, 0, damage);
            
            jsPrintString($"{action.Type} {action.Damage}");
            
            jsPrintString($"{attacker.Attack} {attacker.Health} {attacker.Armour}");
            jsPrintString($"{victim.Attack} {victim.Health} {victim.Armour}");
            
            _fightQueue.Enqueue(action);   
        }

        private IEnumerator FightCoroutine()
        {
            jsPrintString("==== FIGHT ====");
            yield return new WaitForSeconds(1);

            while (_fightQueue.Count > 0)
            {
                FightAction action = _fightQueue.Dequeue();

                for (int i = 0; i < _characters.Length; ++i)
                {
                    FightScene.Character charComponent = 
                        _characters[i].GetComponent<FightScene.Character>();

                    charComponent.HideAttackInfo();
                }

                jsPrintString($"A: {action.AttackerID} V: {action.VictimID}  Type: {action.Type} D: {action.Damage} V: {action.Vampirism} R: {action.Reflect}");
                
                FightScene.Character attackerComponent = 
                    _characters[(int) action.AttackerID].GetComponent<FightScene.Character>();
                
                FightScene.Character victimComponent = 
                    _characters[(int) action.VictimID].GetComponent<FightScene.Character>();
                
                attackerComponent.ShowAttack();
                victimComponent.ShowDamage(action);

                yield return new WaitForSeconds(1);
                
                if (action.Vampirism) attackerComponent.ShowVampirism();
                
                if (action.Reflect)
                {
                    yield return new WaitForSeconds(1);
                    attackerComponent.ShowDamage(1);
                    victimComponent.ShowReflect();
                }
                
                yield return new WaitForSeconds(2);
            }

            FightParams fightParams = Fight.Instance.FightParams;

            if (fightParams.LevelUps.Length > 0)
            {
                LevelUp levelUp = fightParams.LevelUps[fightParams.LevelUps.Length - 1];

                PlayerState.GetInstance().Character.Level = levelUp.Level;
                PlayerState.GetInstance().Character.Exp = levelUp.Exp;
                PlayerState.GetInstance().Character.Upgrades = levelUp.UpgradesTotal;
            }
            else
            {
                PlayerState.GetInstance().Character.Exp += fightParams.Score;
            }

            if (fightParams.Victory)
            {
                jsPrintString("Load Victory");
                SceneManager.LoadScene(_victoryScene, LoadSceneMode.Single);
            }
            else if (fightParams.Died)
            {
                jsPrintString("Load Died");
                SceneManager.LoadScene(_diedScene, LoadSceneMode.Single);
            }
            else
            {
                jsPrintString("Load Draw");
                SceneManager.LoadScene(_drawScene, LoadSceneMode.Single);
            }
            jsPrintString("Scene should be loaded");
        }
    }
}
