var WBFunctions = {
    jsPrintString: function (str) {
        console.log(Pointer_stringify(str));
    },

    jsConnectWallet: function () {
        
        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }

        window.WB.connectWallet();
    },

    jsGetState: function (strAddress) {
        console.log("jsGetState");

        const address = Pointer_stringify(strAddress);

        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }

        window.WB.getState(address);
    },

    jsRegisterCharacter: function (strAddress) {
        const address = Pointer_stringify(strAddress);

        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }

        window.WB.registerCharacter(address);
    },

    jsGetBuffs: function (strAddress) {
        const address = Pointer_stringify(strAddress);

        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }

        window.WB.getBuffs(address);
    },

    jsCastSpell: function (strAddress, index, wordCount) {
        const address = Pointer_stringify(strAddress);

        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }

        window.WB.castSpell(address, index, wordCount);
    },

    jsConductFight: function (strAddress) {
        const address = Pointer_stringify(strAddress);

        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }

        window.WB.conductFight(address);
    },

    jsGetFight: function (fightId) {
        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }

        window.WB.getFight(fightId);
    },
    
    jsGetCharacter: function (characterId)
    {
        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }
        
        window.WB.getCharacter(characterId);
    },
    
    jsUpgradeAttack: function (strAddress, characterId)
    {
        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }

        const address = Pointer_stringify(strAddress);
        
        window.WB.upgradeAttack(address, characterId);
    },

    jsUpgradeHealth: function (strAddress, characterId)
    {
        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }

        const address = Pointer_stringify(strAddress);

        window.WB.upgradeHealth(address, characterId);
    },
    
    jsUpgradeArmour: function (strAddress, characterId)
    {
        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }

        const address = Pointer_stringify(strAddress);

        window.WB.upgradeArmour(address, characterId);
    },
    
    jsGetLeaderboard: function ()
    {
        if (typeof window.WB == "undefined")
        {
            console.error("can't find wb instance");
            return;
        }

        window.WB.getLeaderboard();
    }
};

mergeInto(LibraryManager.library, WBFunctions);