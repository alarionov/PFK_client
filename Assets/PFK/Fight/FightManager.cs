using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PFK
{
    public class EnemyUnit
    {
        public int Index;
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
        public int AttackerID;
        public int VictimID;
        public int Damage;
    }

    public class FightManager : MonoBehaviour
    {
        [SerializeField] private CharacterView _player;
        [SerializeField] private CharacterView[] _enemyUnits;
        
        [DllImport("__Internal")]
        private static extern void jsPrintString(string str);
        
        private SeedReader _seedReader;

        private void Start()
        {
            Fight.Fight fight = Fight.Fight.Instance;

            _seedReader = new SeedReader(fight.FightParams.Seed);

            EnemyUnit[] enemies = new EnemyUnit[_enemyUnits.Length];
            for (int i = 0; i < _enemyUnits.Length; ++i)
            {
                _enemyUnits[i].UpdateStats(fight.FightParams.Enemies[i]);
                
                enemies[i] = new EnemyUnit()
                {
                    Index = i, 
                    Stats = fight.FightParams.Enemies[i]
                };
            }
            
            _player.UpdateStats(fight.FightParams.Character);

            QueueAllAttacks(fight.FightParams.Character, enemies);
            
            StartCoroutine(FightCoroutine());
        }

        private readonly Queue<FightAction> _fightQueue = new();

        private void QueueAllAttacks(BaseStats player, EnemyUnit[] enemies)
        {
            for (int i = 0; i < 10; ++i)
            {
                int index = _seedReader.Roll((byte)enemies.Length);
                EnemyUnit target = enemies[index];
                
                EnqueueAttack(-1, player, target.Index, target.Stats);
                
                enemies = RecountSkeletons(enemies);

                foreach (EnemyUnit attacker in enemies)
                {
                    EnqueueAttack(attacker.Index, attacker.Stats, -1, player);

                    if (player.Health <= 0) return;
                }

                enemies = RecountSkeletons(enemies);
                
                if (enemies.Length == 0) return;
            }
        }

        private EnemyUnit[] RecountSkeletons(EnemyUnit[] skeletons)
        {
            return skeletons.Where(_ => _.Stats.Health > 0).ToArray();
        }

        private EnemyUnit[] GetSkeletonsByLevel(int level)
        {
            return new EnemyUnit[0];
        }
        
        private int CalculateChance(int b, int bonus, int from, int to)
        {
            int top = from - to;
            int bottom = from + to;
        
            if (bottom == 0) return b;
        
            int chance = b + bonus * top / bottom;
        
            if (chance < 0) chance = 0;
        
            return chance;
        }

        private int GetCritChance(BaseStats target, BaseStats attacker)
        {
            return CalculateChance(32, 96, attacker.Luck, target.Luck);
        }
        
        private int GetHitChance(BaseStats target, BaseStats attacker)
        {
            return CalculateChance(64, 64, attacker.Dexterity, target.Dexterity);
        }

        private void EnqueueAttack(int attackerID, BaseStats attacker, int victimID, BaseStats victim)
        {
            FightAction action = new()
            {
                AttackerID = attackerID,
                VictimID = victimID
            };

            int damage = attacker.Attack;

            int hitChance = GetHitChance(victim, attacker);
            int hitRoll = _seedReader.Roll(128);
            bool hit = hitRoll < hitChance;

            int critChance = GetCritChance(victim, attacker);
            int critRoll = _seedReader.Roll(128); 
            bool crit = critRoll < critChance;

            if (hit)
            {
                action.Type = AttackType.Hit;

                if (crit)
                    action.Type = AttackType.Crit;
                else 
                    damage = Mathf.Clamp(damage - victim.Armour, 0, damage);
                
                victim.Health -= damage;
            }
            else
            {
                damage = 0;
                action.Type = AttackType.Miss;
            }

            action.Damage = damage; 
            
            _fightQueue.Enqueue(action);   
        }

        private IEnumerator FightCoroutine()
        {
            yield return new WaitForSeconds(1);

            while (_fightQueue.Count > 0)
            {
                FightAction action = _fightQueue.Dequeue();
                
                CharacterView attackerComponent = 
                    action.AttackerID == -1 ? _player : _enemyUnits[action.AttackerID];
                
                CharacterView victimComponent =
                    action.VictimID == -1 ? _player : _enemyUnits[action.VictimID];
                
                attackerComponent.ShowAttack();
                victimComponent.ShowDamage(action);

                yield return new WaitForSeconds(1);
            }

            Fight.FightParams fightParams = Fight.Fight.Instance.FightParams;

            if (fightParams.LevelUps.Length > 0)
            {
                LevelUp levelUp = fightParams.LevelUps[^1];

                PlayerState.GetInstance().Character.Level = levelUp.Level;
                PlayerState.GetInstance().Character.Exp = levelUp.Exp;
                PlayerState.GetInstance().Character.Upgrades = levelUp.UpgradesTotal;
            }
            else
            {
                //PlayerState.GetInstance().Character.Exp += fightParams.Score;
            }

            yield return new WaitForSeconds(1);

            AsyncOperation asyncLoad = 
                SceneManager.LoadSceneAsync("PFK/Acts/Act001/Act1", LoadSceneMode.Single);
            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            
            /*
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
            */
        }
    }
}
