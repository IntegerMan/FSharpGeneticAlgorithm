module MattEland.FSharpGeneticAlgorithm.Genetics.Population

open MattEland.FSharpGeneticAlgorithm.Logic.Simulator
open MattEland.FSharpGeneticAlgorithm.Genetics.Genes

let simulateGeneration random actors =
  let initialState = buildStartingState random
  actors |> Seq.map (fun b -> simulate random b initialState) |> Seq.sortByDescending (fun r -> r.score)

let buildInitialPopulation random =
  seq {
    for id in 1 .. 10 do
      let brain = getRandomChromosome random id
      yield brain
  }
  
let simulateFirstGeneration random =
  buildInitialPopulation random 
  |> simulateGeneration random
  
let mutateBrains random results =
  42

let mutateAndSimulateGeneration (random: System.Random, results: SimulationResult[]) =
  Seq.map (fun b -> b.brain) results // TODO: Acutally mutate
  |> simulateGeneration random
 