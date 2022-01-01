class EventLookup
{
    constructor(web3, ContractRegistry) 
    {
        this.web3 = web3;
        this.topics = {};
        
        let name, contract;
        
        for ([name, contract] of Object.entries(ContractRegistry))
        {
            const topics = {};
            
            for (const _ of contract.abi)
            {
                if (_.type !== "event")
                    continue;
                
                topics[this.web3.eth.abi.encodeEventSignature(_)] = _;      
            }

            if (Object.keys(topics).length > 0)
                this.topics[contract.address] = topics;
        }
    }
    
    parse = (logs) => 
    {
        const events = [];
        
        let key, record;
        for ([key, record] of Object.entries(logs))
        {
            const event = this.topics[record.address][record.raw.topics[0]];
            const data = this.web3.eth.abi.decodeLog(event.inputs, record.raw.data, record.raw.topics);
            
            events.push(Object.assign({ name: event.name }, data));
        }
        
        return events;
    };
}

export default EventLookup;