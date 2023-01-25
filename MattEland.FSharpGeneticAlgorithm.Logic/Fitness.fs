module MattEland.FSharpGeneticAlgorithm.Logic.Fitness

open MattEland.FSharpGeneticAlgorithm.Logic.States

let evaluateFitness (gameStates: GameState[], fitnessFunction): float = fitnessFunction(gameStates)

let standardFitnessFunction (gameStates: GameState[]): float =
  let lastState: GameState = Seq.last gameStates

  let gameLength = float(gameStates.Length)

  let gotAcornBonus = 
    match lastState.World.Acorn.IsActive with 
    | true  -> 100.0 
    | false -> 0.0

  let finalStateBonus =
    match lastState.SimState with
    | SimulationState.Won  -> 1000.0 - (gameLength * 25.0) // Reward quick wins
    | _ -> -50.0 + gameLength

  gotAcornBonus + finalStateBonus

let killRabbitFitnessFunction (gameStates: GameState[]): float =
  let lastState: GameState = Seq.last gameStates

  let gameLength = float(gameStates.Length)

  let gotAcornBonus = 
    match lastState.World.Acorn.IsActive with 
    | true  -> 100.0 
    | false -> 0.0

  let isSquirrelAlive = lastState.World.Squirrel.IsActive

  let wonBonus = 
    match lastState.SimState with 
    | SimulationState.Won  -> 250.0 
    | _ -> match isSquirrelAlive with
           | true -> 0.0
           | false -> -100.0 + gameLength

  let isRabbitAlive = lastState.World.Rabbit.IsActive

  let rabbitBonus =
    match isRabbitAlive with
    | false -> 1000.0 - (gameLength * 5.0) // Heavily reward dead rabbits
    | true -> 0.0

  gotAcornBonus + rabbitBonus + wonBonus