import { default as ContractAddresses } from './contracts.json' assert {type: "json" };

import { default as AuthContractABI } from './ABI/AuthContract.json' assert { type: "json" };
import { default as RandomContractABI } from './ABI/RandomContract.json' assert { type: "json" };
import { default as CharacterContractABI } from './ABI/CharacterContract.json' assert { type: "json" };
import { default as FightContractABI } from './ABI/FightContract.json' assert { type: "json" };
import { default as FightManagerContractABI } from './ABI/FightManagerContract.json' assert { type: "json" };
import { default as EquipmentContractABI } from './ABI/EquipmentContract.json' assert { type: "json" };
import { default as EquipmentManagerContractABI } from './ABI/EquipmentManagerContract.json' assert { type: "json" };
import { default as Act1MilestonesABI } from './ABI/Act1Milestones.json' assert { type: "json" };
import { default as Act1SidequestsABI } from './ABI/Act1Sidequests.json' assert { type: "json" };
import { default as PurrOwnershipABI } from './ABI/PurrOwnership.json' assert { type: "json" };

const ContractRegistry = {
    AuthContract: { address: ContractAddresses.AuthContract, abi: AuthContractABI },
    RandomContract: { address: ContractAddresses.RandomContract, abi: RandomContractABI },
    CharacterContract: { address: ContractAddresses.CharacterContract, abi: CharacterContractABI },
    FightContract: { address: ContractAddresses.FightContract, abi: FightContractABI },
    FightManagerContract: { address: ContractAddresses.FightManagerContract, abi: FightManagerContractABI },
    EquipmentContract: { address: ContractAddresses.EquipmentContract, abi: EquipmentContractABI },
    EquipmentManagerContract: { address: ContractAddresses.EquipmentManagerContract, abi: EquipmentManagerContractABI },
    Act1Milestones: { address: ContractAddresses.Act1Milestones, abi: Act1MilestonesABI },
    Act1Sidequests: { address: ContractAddresses.Act1Sidequests, abi: Act1SidequestsABI },
    PurrOwnership: { address: ContractAddresses.PurrOwnership, abi: PurrOwnershipABI }
}

export { ContractRegistry };