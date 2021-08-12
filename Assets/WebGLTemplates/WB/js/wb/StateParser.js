import Parser from './Parser.js';

class StateParser extends Parser {
    parse = (data) =>
    {
        console.log("StateParser.parse");
        const values = this.getBlocks(data);

        return {
            tokenId: values[0],
            level: values[1],
            difficulty: values[2]
        };
    };
}

export default StateParser;