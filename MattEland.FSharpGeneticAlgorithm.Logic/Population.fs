module MattEland.FSharpGeneticAlgorithm.Genetics.Population

open MattEland.FSharpGeneticAlgorithm.Logic.Simulator
open MattEland.FSharpGeneticAlgorithm.Genetics.Genes
open MattEland.FSharpGeneticAlgorithm.Logic.World

let simulateGeneration states actors =
  actors 
  |> Seq.map (fun b -> simulate b states) 
  |> Seq.sortByDescending (fun r -> r.totalScore)

let buildInitialPopulation random =
  Seq.init<ActorChromosome> 100 (fun _ -> getRandomChromosome random)
  
let simulateFirstGeneration states random =
  buildInitialPopulation random |> simulateGeneration states
  
let mutateBrains (random: System.Random, brains: ActorChromosome[]): ActorChromosome[] =
  let numBrains = brains.Length
  let survivors = [| brains.[0]; brains.[1]; |]
  let randos = Seq.init (numBrains - 4) (fun _ -> getRandomChromosome random) |> Seq.toArray

  let children = [| 
    createChild(random, survivors.[0].genes, survivors.[1].genes, 0.25);
    createChild(random, survivors.[0].genes, survivors.[1].genes, 0.5);
  |]

  Array.append children randos |> Array.append survivors

let mutateAndSimulateGeneration (random: System.Random, worlds: World[], results: SimulationResult[]) =
  let brains = Seq.map (fun b -> b.brain) results |> Seq.toArray
  mutateBrains(random, brains) |> simulateGeneration worlds

let mutateAndSimulateMultiple (random: System.Random, worlds: World[], generations: int, results: SimulationResult[]) =
  let mutable currentResults = results
  for _ = 1 to generations do    
    let brains = Seq.map (fun b -> b.brain) currentResults |> Seq.toArray
    currentResults <- mutateBrains(random, brains) |> simulateGeneration worlds |> Seq.toArray
  currentResults
  