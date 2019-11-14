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
    | SimulationState.Won  -> 500.0 - gameLength // Reward quick wins
    | SimulationState.Lost -> -50.0 + gameLength
    | _                    -> 0.0 + gameLength

  gotAcornBonus + finalStateBonus

let killRabbitFitnessFunction (gameStates: GameState[]): float =
  let rabbitBonus = Seq.map (fun s -> match s.World.Rabbit.IsActive with
                                      | false -> 10.0
                                      | _ -> 0.0) gameStates |> Seq.sum 

  rabbitBonus + standardFitnessFunction gameStates