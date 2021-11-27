var WBFunctions = {
    jsPrintString: function(str) { console.log(UTF8ToString(str)); },
    jsConnectWallet: function() { window.WB.connectWallet(); },
    jsGetState: function(strAddress) { window.WB.getState(UTF8ToString(strAddress)); },
    jsGetCharacter: function(token) { window.WB.getCharacter(token); },
    jsConductFight: function(strAddress) { window.WB.conductFight(UTF8ToString(strAddress)); }
};

mergeInto(LibraryManager.library, WBFunctions);