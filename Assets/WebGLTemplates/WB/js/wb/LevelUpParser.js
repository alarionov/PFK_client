import Parser from './Parser.js';

class LevelUpParser extends Parser
{
    parse = (data) => {
        console.log("FightParser.parse");

        const blocks = this.getBlocks(data);
        
        console.log(blocks);
        
        return {
            Level: blocks[0], 
            Exp: blocks[1], 
            Tnl: blocks[2], 
            UpgradesGiven: blocks[3],
            UpgradesTotal: blocks[4]
        };
    };
}

export default LevelUpParser;