module MattEland.FSharpGeneticAlgorithm.Logic.Fitness

open MattEland.FSharpGeneticAlgorithm.Logic.States

let evaluateFitness (gameStates: GameState[], fitnessFunction): float = fitnessFunction(gameStates)

let standardFitnessFunction (gameStates: GameState[]): float =
  let lastState: GameState = Seq.last gameStates

  let gameLength = float(gameStates.Length)

  let gotAcornBonus = 
    match lastState.World.Acorn.IsActive with 
    | true  -> 25.0 
    | false -> 0.0

  let finalStateBonus =
    match lastState.SimState with
    | SimulationState.Won  -> 100.0 - gameLength
    | SimulationState.Lost -> -50.0 + gameLength
    | _                    -> 0.0 + gameLength

  gotAcornBonus + finalStateBonus
