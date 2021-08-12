class EventLookup
{
    constructor(Core, Char, FLogic, FToken) 
    {
        this.CoreContract = Core;
        this.CharContract = Char;
        this.FLogicContract = FLogic;
        this.FTokenContract = FToken;
        
        this.StateTopic = "0x30cb76938f0aeca5c9f920967dbd60ce65f776087c05e75053aaaf7a9c7d59ae";
        this.CharacterTopic = "0x1f1b05581c78f5f86e65fd78c3008ae03faf0be224944f84577fa403100373a5";
        this.NewFightTopic = "0x2b7a82a1055b6b78f109f5d0ead04b76672468d759deb494d72d34505b8dadbf";
        this.BuffsTopic = "0xaab3b4a17b6c2490d24f831caaf905914d10ddc88d0839ab608f242fdf6c5933";
        this.LevelUpTopic = "0x78e2d3f9d54f6b793c45a5d3d60ac4e6061fb0001ef5feb0b67bc38fdddf91c6";
        this.NewStatsTopic = "0xf6b6548ec95687007362fe70afa063f75101182af5eb27fa5040d426ef198d0b";
    }
    
    getEvent = (tx, contract, topic) =>
    {
        return this.getEvents(tx, contract, topic)[0];
    };
    
    getEvents = (tx, contract, topic) => 
    {
        const events = [];
        Object.entries(tx.events).forEach(([index, event]) => {
            if (event.address !== contract) return;
            if (event.raw.topics[0] !== topic) return;
            events.push(event.raw.data);
        });
        return events;
    };
    
    stateEvent = (tx) => { return this.getEvent(tx, this.CoreContract, this.StateTopic); };
    characterEvent = (tx) => { return this.getEvent(tx, this.CharContract, this.CharacterTopic); };
    fightEvent = (tx) => { return this.getEvent(tx, this.FTokenContract, this.NewFightTopic); };
    buffsEvent = (tx) => { return this.getEvent(tx, this.CoreContract, this.BuffsTopic); };
    levelUpEvents = (tx) => { return this.getEvents(tx, this.CharContract, this.LevelUpTopic); };
    newStatsEvent = (tx) => { return this.getEvent(tx, this.CharContract, this.NewStatsTopic); };
}

export default EventLookup;