var WBFunctions = {
    jsPrintString: function(str) { 
        console.log(UTF8ToString(str)); 
    },
    
    jsConnectWallet: function() { 
        window.WB.connectWallet(); 
    },
    
    jsGetState: function(strAddress) { 
        window.WB.getState(UTF8ToString(strAddress)); 
    },
    
    jsGetAct1Progress: function(contractAddress, token) { 
        window.WB.getAct1Progress(UTF8ToString(contractAddress), token); 
    },
    
    jsGetAct1SidequestCooldowns: function(contractAddress, token) { 
        window.WB.getAct1SidequestCooldowns(UTF8ToString(contractAddress), token); 
    },
    
    jsGetCharacter: function(strAddress, token) { 
        window.WB.getCharacter(UTF8ToString(strAddress), token); 
    },
    
    jsConductFight: function(playerAddress, mapContract, level, characterContract, tokenId) { 
        window.WB.conductFight(
            UTF8ToString(playerAddress), 
            UTF8ToString(mapContract), 
            level, 
            UTF8ToString(characterContract), tokenId); 
    },
};

mergeInto(LibraryManager.library, WBFunctions);