import Parser from './Parser.js';

class CharacterParser extends Parser {
    parse = (data) =>
    {
        console.log("CharacterParser.parse");
        const values = this.getBlocks(data);

        const stats = { Attack: values[2], Health: values[3], Armour: values[4] };
        const character = {
            Stats: stats,
            Alive: values[5] > 0,
            Exists: values[6] > 0,
            Level: values[8],
            Exp: values[9],
            Upgrades: values[10],
        };

        return character;
    };
    
    getStatsFromJson = (json) =>
    {
        const stats = {
            Strength: parseInt(json.strength),
            Dexterity: parseInt(json.dexterity),
            Constitution: parseInt(json.constitution),
            Luck: parseInt(json.luck),
            Armour: parseInt(json.armor),
            Attack: parseInt(json.attack),
            Health: parseInt(json.health)
        }
        return stats;
    };
}

export default CharacterParser;