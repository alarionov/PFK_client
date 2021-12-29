import { ContractRegistry } from "./ContractRegistry.js";
import CharacterParser from "./CharacterParser.js";
import EventLookup from "./EventLookup.js";
import FightParser from "./FightParser.js";
import LevelUpParser from "./LevelUpParser.js";
import NewStatsParser from "./NewStatsParser.js";
import StateParser from "./StateParser.js";

class WB 
{
    constructor(unityInstance)
    {
        if (typeof unityInstance === "undefined")
        {
            throw "Unity Instance is not defined";
        }
        
        this.blockSize = 64;
        this.unityInstance = unityInstance;
        this.stateParser = new StateParser(this.blockSize);
        this.characterParser = new CharacterParser(this.blockSize);
        this.fightParser = new FightParser(this.blockSize);
        this.newStatsParser = new NewStatsParser(this.blockSize);
        this.levelUpParser = new LevelUpParser(this.blockSize);
        
        //this.eventLookup = new EventLookup(this.CoreContract, this.CharContract, this.FightLogicContract, this.FightContract);
    }

    /*
    const urlParams = new URLSearchParams(window.location.search);
    const fightId = parseInt(urlParams.get('fight'));
    isNaN(fightId)
     */
    
    showNoWalletError = () =>
    {
        console.log("WB.showNoWalletError");
        this.unityInstance.SendMessage("[WalletManager]", "ShowNoWalletError");
    };

    connectWallet = async () =>
    {
        console.log("WB.connectWallet");

        try {
            if (typeof window.ethereum === "undefined") {
                console.error("no wallet");
                this.showNoWalletError();
                return;
            }

            const accounts = await ethereum.request({method: 'eth_requestAccounts'});

            if (!Array.isArray(accounts) || accounts.length === 0) {
                console.error("Reject");
                return;
            }

            this.web3 = new Web3(window.ethereum);

            const account = accounts[0];
            this.unityInstance.SendMessage("[WalletManager]", "SetWallet", account);
        }
        catch (e)
        {
            this.showNoWalletError();
            console.log("Error: ", e.message);
        }
    };

    getState = async (playerAddress) =>
    {
        console.log("WB.getState");
        
        const abi = AbiRegistry.Core;

        const contract = new this.web3.eth.Contract(abi, this.CoreContract);

        const state = await contract.methods.getState(playerAddress).call();

        const tokenId = parseInt(state.tokenId);

        if (tokenId > 0)
        {
            await this.getBuffs(playerAddress);
            await this.getCharacter(tokenId);
        }

        this.stateLoaded(state);
    };

    stateLoaded = (objState) =>
    {
        const state = {
            tokenId: objState.tokenId,
            level: objState.level,
            difficulty: objState.difficulty
        };

        this.unityInstance.SendMessage("[WalletManager]", "SetState", JSON.stringify(state));
    };

    conductFight = async (playerAddress, mapContract, level, characterContract, tokenId)  =>
    {
        this.unityInstance.SendMessage("[WalletManager]", "ShowLoadingScreen");
        
        const contract = 
            new this.web3.eth.Contract(
                ContractRegistry.FightManagerContract.abi,
                ContractRegistry.FightManagerContract.address);
        
        try {
            console.log(playerAddress);
            console.log(mapContract);
            console.log(level);
            console.log(characterContract);
            console.log(tokenId);
            
            const tx = await contract.methods.conductFight(mapContract, level, characterContract, tokenId).send({from: playerAddress});
            
            console.log(tx);
            
            /*
            
            const fight = this.fightParser.parse(this.eventLookup.fightEvent(tx));
            const buffs = this.buffsParser.parse(this.eventLookup.buffsEvent(tx));
            
            console.log("Level Up events:", this.eventLookup.levelUpEvents(tx));
            
            const levelups = [];
            const levelUpEvents = this.eventLookup.levelUpEvents(tx);
            
            for (let i = 0; i < levelUpEvents.length; ++i)
            {
                levelups.push(this.levelUpParser.parse(levelUpEvents[i]));
            }
            
            fight.LevelUps = levelups.sort((a,b) => a.Level < b.Level ? -1 : 1);
            
            this.unityInstance.SendMessage("[WalletManager]", "BuffsLoaded", JSON.stringify({ Buffs: buffs }));
            this.unityInstance.SendMessage("[WalletManager]", "FightLoaded", JSON.stringify(fight));            
             */
        } catch (e) {
            if (e.code === 4001)
            {
                console.log("User rejected the transaction");
            }
            else 
            {
                throw e;
            }
        }
        finally {
            this.unityInstance.SendMessage("[WalletManager]", "HideLoadingScreen");
        }
    };

    getCharacter = async (contractAddress, characterId) =>
    {
        console.log("WB.getCharacter");
        
        const contract = 
            new this.web3.eth.Contract(
                ContractRegistry.CharacterContract.abi, 
                ContractRegistry.CharacterContract.address);
        
        const response = await contract.methods.getCharacter(contractAddress, characterId).call();
        
        const equipment = {
            ArmorSetId: response.equipment.armorSetId,
            WeaponSetId: response.equipment.weaponSetId,
            ShieldId: response.equipment.shieldId,
        };
        
        const stats = {
            Strength: response.stats.strength,
            Dexterity: response.stats.dexterity,
            Constitution: response.stats.constitution,
            Luck: response.stats.luck,
            Armor: response.stats.armor,
            Attack: response.stats.attack,
            Health: response.stats.health,
            TakenDamage: response.stats.takenDamage
        };

        const character = {
            ContractAddress: contractAddress,
            TokenId: characterId, 
            Equipment: equipment,
            Stats: stats,
            Upgrades: response.upgrades,
            Exp: response.exp,
            Level: response.level
        };

        this.unityInstance.SendMessage("[WalletManager]", "CharacterLoaded", JSON.stringify(character));
    };

    getAct1Progress = async (contractAddress, characterId) => 
    {
        console.log("WB.getMapProgress");

        const characterContract =
            new this.web3.eth.Contract(
                ContractRegistry.CharacterContract.abi,
                ContractRegistry.CharacterContract.address);

        const character = await characterContract.methods.getCharacter(contractAddress, characterId).call();
        
        const mapContract =
            new this.web3.eth.Contract(
                ContractRegistry.Act1Milestones.abi,
                ContractRegistry.Act1Milestones.address);
        
        const progress = await mapContract.methods.getProgress(character).call();
        this.unityInstance.SendMessage("[WalletManager]", "LoadAct1", parseInt(progress));
    };
        
    upgradeAttack = async (playerAddress, characterId) => {
        this.unityInstance.SendMessage("[WalletManager]", "ShowLoadingScreen");
        
        const abi = AbiRegistry.Character;

        const contract = new this.web3.eth.Contract(abi, this.CharContract);
        const tx = await contract.methods.upgradeAttack(characterId).send({from: playerAddress});
        
        const character = this.newStatsParser.parse(this.eventLookup.newStatsEvent(tx));
        
        this.unityInstance.SendMessage("[WalletManager]", "NewStatsLoaded", JSON.stringify(character));

        this.unityInstance.SendMessage("[WalletManager]", "HideLoadingScreen");
    };

    upgradeHealth = async (playerAddress, characterId) => {
        this.unityInstance.SendMessage("[WalletManager]", "ShowLoadingScreen");
        
        const abi = AbiRegistry.Character;

        const contract = new this.web3.eth.Contract(abi, this.CharContract);
        const tx = await contract.methods.upgradeHealth(characterId).send({from: playerAddress});

        const character = this.newStatsParser.parse(this.eventLookup.newStatsEvent(tx));

        this.unityInstance.SendMessage("[WalletManager]", "NewStatsLoaded", JSON.stringify(character));
        this.unityInstance.SendMessage("[WalletManager]", "HideLoadingScreen");
    };

    upgradeArmour = async (playerAddress, characterId) => {
        this.unityInstance.SendMessage("[WalletManager]", "ShowLoadingScreen");
        
        const abi = AbiRegistry.Character;

        const contract = new this.web3.eth.Contract(abi, this.CharContract);
        const tx = await contract.methods.upgradeArmour(characterId).send({from: playerAddress});

        const character = this.newStatsParser.parse(this.eventLookup.newStatsEvent(tx));

        this.unityInstance.SendMessage("[WalletManager]", "NewStatsLoaded", JSON.stringify(character));
        this.unityInstance.SendMessage("[WalletManager]", "HideLoadingScreen");
    };
}

export default WB;
    