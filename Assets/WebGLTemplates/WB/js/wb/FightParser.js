import Parser from './Parser.js';

class FightParser extends Parser 
{
    parse = (data) =>
    {
        console.log("FightParser.parse");
        
        const blocks = this.getBlocks(data, true);

        const stats = {
            Attack: parseInt(blocks[5], 16),
            Health: parseInt(blocks[6], 16),
            Armour: parseInt(blocks[7], 16)
        };
        
        const oldState = {
            tokenId: parseInt(blocks[8], 16),
            level: parseInt(blocks[9], 16),
            difficulty: parseInt(blocks[10], 16)
        };
        const newState = {
            tokenId: parseInt(blocks[11], 16),
            level: parseInt(blocks[12], 16),
            difficulty: parseInt(blocks[13], 16)
        }
        
        const buffs = new Array(8);
        
        for (let i = 0; i < 8; ++i)
        {
            buffs[i] = parseInt(blocks[23+i], 16);
        }
        
        const fight = {
            Id: parseInt(blocks[1], 16),
            Season: parseInt(blocks[2], 16),
            Seed: blocks[18] + blocks[19] + blocks[20] + blocks[21],
            Score: parseInt(blocks[4], 16),
            Stats: stats,
            OldState: oldState,
            NewState: newState,
            Buffs: buffs,
            Victory: parseInt(blocks[15], 16) > 0,
            Died: parseInt(blocks[16], 16) > 0
        };
        
        return fight;
    };
}

export default FightParser;