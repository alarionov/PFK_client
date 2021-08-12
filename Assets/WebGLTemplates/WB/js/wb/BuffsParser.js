import Parser from './Parser.js';

class BuffsParser extends Parser {
    parse = (data) =>
    {
        console.log("BuffsParser.parse");
        const blocks = this.getBlocks(data);
        
        const Buffs = [];
        for (let i = 2; i < blocks.length; ++i)
        {
            Buffs.push(blocks[i] > 0);
        }
        return Buffs;
    };
}

export default BuffsParser;