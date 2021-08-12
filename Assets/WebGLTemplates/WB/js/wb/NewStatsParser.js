import Parser from './Parser.js';

class NewStatsParser extends Parser {
    parse = (data) =>
    {
        console.log("NewStatsParser.parse");
        const blocks = this.getBlocks(data);

        const character = {
            Stats: { 
                Attack: blocks[0],
                Health: blocks[1],
                Armour: blocks[2]
            },
            Upgrades: blocks[3]
        };
        
        return character;
    };
}

export default NewStatsParser;