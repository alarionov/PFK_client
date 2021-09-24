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
        public bool Vampirism;
        public bool Reflect;
    }

    public class FightManager : MonoBehaviour
    {
        [SerializeField] private CharacterView _player;
        [SerializeField] private CharacterView[] _enemyUnits;
        
        [DllImport("__Internal")]
        private static extern void jsPrintString(string str);
        
        private SeedReader _seedReader;

        private readonly Queue<FightAction> _fightQueue = new Queue<FightAction>();

        private void Start()
        {
            Fight fight = Fight.Instance;
            _seedReader = new SeedReader(fight.FightParams.Seed);

            BaseStats baseStats = fight.FightParams.Stats;
            bool[] playerBuffs = fight.FightParams.Buffs;

            EnemyUnit[] skeletons = new EnemyUnit[_enemyUnits.Length];

            for (int i = 0; i < _enemyUnits.Length; ++i)
            {
                skeletons[i] = new EnemyUnit()
                {
                    Index = i, 
                    Stats = _enemyUnits[i].BaseStats
                };
            }

            BaseStats.ApplyBuffs(baseStats, playerBuffs);
            
            _player.UpdateStats(baseStats);

            QueueAllAttacks(baseStats, skeletons, playerBuffs);
            
            StartCoroutine(FightCoroutine());
        }

        private void QueueAllAttacks(BaseStats baseStats, EnemyUnit[] skeletons, bool[] playerBuffs)
        {
            bool[] skeletonBuffs = new bool[playerBuffs.Length];

            #if UNITY_WEBGL && !UNITY_EDITOR
                jsPrintString($"Player stats: { baseStats.Attack } - { baseStats.Health } - { baseStats.Armour }");
                jsPrintString($"Number of skeletons: { skeletons.Length }");
            #endif
            
            for (int i = 0; i < 10; ++i)
            {
                int index = _seedReader.Roll((byte)skeletons.Length);
                EnemyUnit target = skeletons[index];
                
                EnqueueAttack(-1, baseStats, playerBuffs,
                    target.Index, target.Stats, skeletonBuffs);
                
                skeletons = RecountSkeletons(skeletons);
                
                #if UNITY_WEBGL && !UNITY_EDITOR
                    jsPrintString($"Number of skeletons: { skeletons.Length }");
                #endif

                foreach (EnemyUnit attacker in skeletons)
                {
                    EnqueueAttack(attacker.Index, attacker.Stats, skeletonBuffs,
                        -1, baseStats, playerBuffs);

                    if (baseStats.Health <= 0) return;
                }

                skeletons = RecountSkeletons(skeletons);
                
                if (skeletons.Length == 0) return;
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

        private void EnqueueAttack(int attackerID, BaseStats attacker, bool[] buffsAttacker,
            int victimID, BaseStats victim, bool[] buffsVictim)
        {
            FightAction action = new FightAction()
            {
                AttackerID = attackerID,
                VictimID = victimID
            };

            #if UNITY_WEBGL && !UNITY_EDITOR
                jsPrintString($"Actual Attack form {attackerID} to {victimID}");
                jsPrintString($"{attacker.Attack} {attacker.Health} {attacker.Armour}");
                jsPrintString($"{victim.Attack} {victim.Health} {victim.Armour}");
            #endif
            
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
            
            #if UNITY_WEBGL && !UNITY_EDITOR
                jsPrintString($"{action.Type} {action.Damage}");
                jsPrintString($"{attacker.Attack} {attacker.Health} {attacker.Armour}");
                jsPrintString($"{victim.Attack} {victim.Health} {victim.Armour}");
            #endif            

            _fightQueue.Enqueue(action);   
        }

        private IEnumerator FightCoroutine()
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
                jsPrintString("==== FIGHT ====");
            #endif
            
            yield return new WaitForSeconds(1);

            while (_fightQueue.Count > 0)
            {
                FightAction action = _fightQueue.Dequeue();

                for (int i = 0; i < _enemyUnits.Length; ++i)
                {
                    CharacterView charComponent = 
                        _enemyUnits[i].GetComponent<CharacterView>();
                }

                #if UNITY_WEBGL && !UNITY_EDITOR
                    jsPrintString($"A: {action.AttackerID} V: {action.VictimID}  Type: {action.Type} D: {action.Damage} V: {action.Vampirism} R: {action.Reflect}");
                #endif                

                CharacterView attackerComponent = 
                    action.AttackerID == -1 ? 
                        _player : _enemyUnits[(int) action.AttackerID];
                
                CharacterView victimComponent =
                    action.VictimID == -1 ?
                        _player : _enemyUnits[(int) action.VictimID];
                
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
