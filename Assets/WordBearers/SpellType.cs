namespace WordBearers
{
    public enum SpellType
    {
        // COMMON (1 word)
        // +1 attack
        Enchant,
    
        // +5 health
        FalseLife,
    
        // +1 armour
        Shield,
    
        // UNCOMMON (2 words)
    
    
        // RARE (3 words)
        // always crit
        CriticalStrike,
    
        // +1 attack +5 health + 1 armour
        Bless,
    
    
        // EPIC (4 words)
        // 1 damage to an enemy when they attack
        Reflect,
    
        // heal +1 if damage an enemy and crit
        Vampirism,
    
    
        // LEGENDARY (5words), depletable, 1 charge
        // do not die if die
        Salvation
    }
}