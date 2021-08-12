class Parser {
    constructor(blockSize) {
        this.blockSize = blockSize;
    }

    getBlocks = (data, raw) => {
        const size = (data.length - 2) / this.blockSize;
        const blocks = new Array(size);

        for (let i=0; i < blocks.length; ++i)
        {
            const start = 2 + i * this.blockSize;

            const block = data.substr(start, this.blockSize);
            
            blocks[i] = raw === true ? block : parseInt(block, 16);
        }

        return blocks;
    };
}

export default Parser;