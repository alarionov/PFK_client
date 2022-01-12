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
        
        this.eventLookup = new EventLookup(new Web3(), ContractRegistry);
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

            const blockUpdate = async (block) => 
            {
                this.unityInstance.SendMessage("[BlockExplorer]", "SetCurrentBlock", block.number);
            };

            this.web3.eth.subscribe('newBlockHeaders').on("data", blockUpdate.bind(this));
        }
        catch (e)
        {
            this.showNoWalletError();
            console.log("Error: ", e.message);
        }
    };
    
    auth = async (playerAddress, token) => 
    {
        console.log("WB.auth");
        
        const purrOwnership =
            new this.web3.eth.Contract(
                ContractRegistry.PurrOwnership.abi,
                ContractRegistry.PurrOwnership.address);
        
        const owner = await purrOwnership.methods.ownerOf(token).call();
        
        if (owner.toLowerCase() !== playerAddress.toLowerCase())
        {
            this.unityInstance.SendMessage("[WalletManager]", "ShowLoadingScreen");
            
            const response = await fetch(`https://authority.dev.pfk.kotobaza.co/ownership/${ token }`);
            const verification = await response.json();
            
            if (verification.owner.toLowerCase() !== playerAddress.toLowerCase() && false) 
            {
                console.log("not your token");
                return;
            }
           
            const tx = 
                await purrOwnership.methods.verify(
                    verification.owner, 
                    verification.token, 
                    verification.timestamp, 
                    verification.signature).send({from: playerAddress});
            
            this.unityInstance.SendMessage("[WalletManager]", "HideLoadingScreen");
            
            console.log(tx);
            
            if (!tx.status) 
            {
                console.log("Transaction failed");
                console.log(tx);
                return;
            }
        }
        
        await this.getCharacter(ContractRegistry.PurrOwnership.address, token);
        await this.getAct1SidequestCooldowns(ContractRegistry.PurrOwnership.address, token);
        await this.getAct1Progress(ContractRegistry.PurrOwnership.address, token);
    };
    
    conductFight = async (playerAddress, mapContract, level, characterContract, tokenId)  =>
    {
        this.unityInstance.SendMessage("[WalletManager]", "ShowLoadingScreen");
        
        const contract = 
            new this.web3.eth.Contract(
                ContractRegistry.FightManagerContract.abi,
                ContractRegistry.FightManagerContract.address);
        
        try {
            const tx = await contract.methods.conductFight(mapContract, level, characterContract, tokenId).send({from: playerAddress});
            
            const events = this.eventLookup.parse(tx.events);
            
            console.log(events);
            
            let fight;
            
            for (const event of events)
            {
                switch (event.name)
                {
                    case "FightDetails":
                        fight = event.fight;
                        break;
                    case "Milestone":
                        this.unityInstance.SendMessage("[StorylineProgress]", "SetProgress", parseInt(event.index));
                        break;
                    case "Cooldown":
                        const params = JSON.stringify({ index: level, blockNumber: parseInt(event.activeAfter) });
                        this.unityInstance.SendMessage("[SideQuestManager]", "SetQuestCooldown", params);
                        break;
                    default:
                        console.log("Event:");
                        console.log(event);
                }
            }
            
            const fightWrapper = {
                ContractAddress: mapContract,
                SceneIndex: level,
                FightParams: {
                    Id: parseInt(fight.id),
                    Seed: fight.seed,
                    Victory: fight.victory,
                    Character: this.characterParser.getStatsFromJson(fight.character), 
                    Enemies: fight.enemies.map(_ => this.characterParser.getStatsFromJson(_)),
                    Exp: parseInt(fight.exp)
                    //public LevelUp[] LevelUps;
                }
            };
            
            console.log(fightWrapper);

            this.unityInstance.SendMessage("[WalletManager]", "LoadFight", JSON.stringify(fightWrapper));
            
            /*            
            console.log("Level Up events:", this.eventLookup.levelUpEvents(tx));
            
            const levelups = [];
            const levelUpEvents = this.eventLookup.levelUpEvents(tx);
            
            for (let i = 0; i < levelUpEvents.length; ++i)
            {
                levelups.push(this.levelUpParser.parse(levelUpEvents[i]));
            }
            
            fight.LevelUps = levelups.sort((a,b) => a.Level < b.Level ? -1 : 1);                    
            */
            
        } 
        catch (e) 
        {
            this.unityInstance.SendMessage("[WalletManager]", "HideLoadingScreen");
            
            if (e.code === 4001)
            {
                console.log("User rejected the transaction");
            }
            else 
            {
                throw e;
            }
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

        console.log(character);
        
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

    getAct1SidequestCooldowns = async (contractAddress, characterId) =>
    {
        console.log("WB.getSideQuestCooldowns");

        const characterContract =
            new this.web3.eth.Contract(
                ContractRegistry.CharacterContract.abi,
                ContractRegistry.CharacterContract.address);

        const character = await characterContract.methods.getCharacter(contractAddress, characterId).call();

        const mapContract =
            new this.web3.eth.Contract(
                ContractRegistry.Act1Sidequests.abi,
                ContractRegistry.Act1Sidequests.address);

        const cooldowns = await mapContract.methods.getCooldowns(character).call();
        
        for (let i=0; i < cooldowns.length; ++i)
        {
            this.unityInstance.SendMessage(
                "[SideQuestManager]", 
                "SetQuestCooldown", 
                JSON.stringify({index: i, blockNumber: parseInt(cooldowns[i])}));
        }
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
    
    resetProgress = async (playerAddress, contractAddress, characterId) =>
    {
        const characterContract =
            new this.web3.eth.Contract(
                ContractRegistry.CharacterContract.abi,
                ContractRegistry.CharacterContract.address);

        const character = await characterContract.methods.getCharacter(contractAddress, characterId).call();

        const mapContract =
            new this.web3.eth.Contract(
                ContractRegistry.Act1Milestones.abi,
                ContractRegistry.Act1Milestones.address);

        mapContract.methods.resetProgress(character).send({from: playerAddress});
    };
    
    setCooldowns = async () => {
        const addr = "0x8229d792c1BCCdb9Cc336821502aC906005317a6";
        const mapContract =
            new this.web3.eth.Contract(
                ContractRegistry.Act1Sidequests.abi,
                ContractRegistry.Act1Sidequests.address);
        
        mapContract.methods.setCooldowns([10,20,30,40,50,60]).send({from: addr});
    };
}

export default WB;
    