import { AbiRegistry } from "./AbiRegistry.js";
import BuffsParser from "./BuffsParser.js";
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
        this.CoreContract =  "0x95b25B0855f90F43ae60a7fAb67EadDC3412d30f";
        this.CharContract = "0xa99F479b6134dE8ff4c633EE9e89e2cAD3DeE23b";
        this.FightLogicContract = "0x115aBfE20480AC494bacDd9Ed048B4b96De7F23D";
        this.FightContract = "0xaBd075bda3A28FdC7C4Cd84067bC2FD94338116C";
        
        this.buffsParser = new BuffsParser(this.blockSize);
        this.stateParser = new StateParser(this.blockSize);
        this.characterParser = new CharacterParser(this.blockSize);
        this.fightParser = new FightParser(this.blockSize);
        this.newStatsParser = new NewStatsParser(this.blockSize);
        this.levelUpParser = new LevelUpParser(this.blockSize);
        
        this.eventLookup = new EventLookup(this.CoreContract, this.CharContract, this.FightLogicContract, this.FightContract);
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

    getBuffs = async (playerAddress) =>
    {
        console.log("WB.getBuffs");
        const abi = AbiRegistry.Core;

        const contract = new this.web3.eth.Contract(abi, this.CoreContract);
        const response = await contract.methods.getBuffs(playerAddress).call();
        const state = { Buffs: response };

        this.unityInstance.SendMessage("[WalletManager]", "BuffsLoaded", JSON.stringify(state));
    };

    registerCharacter = async (playerAddress) =>
    {
        console.log("WB.registerCharacter");
        const abi = AbiRegistry.Core;

        const contract = new this.web3.eth.Contract(abi, this.CoreContract);

        this.unityInstance.SendMessage("[WalletManager]", "ShowLoadingScreen");
        const tx = await contract.methods.registerCharacter(0).send({from: playerAddress});
        this.unityInstance.SendMessage("[WalletManager]", "HideLoadingScreen");

        if (tx.status === false)
        {
            console.error("Could not create a character", tx);
            return;
        }

        const character = this.characterParser.parse(this.eventLookup.characterEvent(tx));
        this.unityInstance.SendMessage("[WalletManager]", "CharacterLoaded", JSON.stringify(character));

        const buffs = { Buffs: this.buffsParser.parse(this.eventLookup.buffsEvent(tx)) };
        this.unityInstance.SendMessage("[WalletManager]", "BuffsLoaded", JSON.stringify(buffs));

        const state = this.stateParser.parse( this.eventLookup.stateEvent(tx));
        this.unityInstance.SendMessage("[WalletManager]", "SetState", JSON.stringify(state));
    };

    castSpell = async (playerAddress, index, wordCount) =>
    {
        console.log("WB.castSpell");

        this.unityInstance.SendMessage("[WalletManager]", "ShowLoadingScreen");
        
        const abi = AbiRegistry.Core;
        const value = Web3.utils.toWei((0.001 * wordCount).toString());
        const contract = new this.web3.eth.Contract(abi, this.CoreContract);
        const tx = await contract.methods.castSpell(index).send({from: playerAddress, value: value});
        const buffs = this.buffsParser.parse(this.eventLookup.buffsEvent(tx));
        this.unityInstance.SendMessage("[WalletManager]", "BuffsLoaded", JSON.stringify({Buffs: buffs}));

        this.unityInstance.SendMessage("[WalletManager]", "HideLoadingScreen");
    };

    conductFight = async (playerAddress) =>
    {
        const abi = AbiRegistry.Core;
        this.unityInstance.SendMessage("[WalletManager]", "ShowLoadingScreen");

        const contract = new this.web3.eth.Contract(abi, this.CoreContract);
        
        try {
            const tx = await contract.methods.conductFight().send({from: playerAddress});
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

    getCharacter = async (characterId) =>
    {
        console.log("WB.getCharacter");

        const abi = AbiRegistry.Character;

        const contract = new this.web3.eth.Contract(abi, this.CharContract);
        const response = await contract.methods.getCharacter(characterId).call();

        const stats = {
            Attack: response.stats.attack,
            Health: response.stats.health,
            Armour: response.stats.armour,
        };

        const character = {
            Stats: stats,
            Upgrades: response.upgrades,
            Exp: response.exp,
            Alive: response.alive,
            Level: response.level,
            Exists: response.exists,
        };

        this.unityInstance.SendMessage("[WalletManager]", "CharacterLoaded", JSON.stringify(character));
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

    getLeaderboard = async () =>
    {
        console.log("WB.getLeaderboard");

        const abi = AbiRegistry.Core;

        const contract = new this.web3.eth.Contract(abi, this.CoreContract);
        const response = await contract.methods.getLeaderboard().call();

        const records = [];
        
        for (let i = 0; i < response.length; ++i)
        {
            records.push({
                Player: response[i].player,
                Score: response[i].score
            });    
        }
        
        const leaderboard = { Records: records };
        
        this.unityInstance.SendMessage("[WalletManager]", "LeaderboardLoaded", JSON.stringify(leaderboard));
    };
}

export default WB;
    