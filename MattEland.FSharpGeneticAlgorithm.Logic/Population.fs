module MattEland.FSharpGeneticAlgorithm.Genetics.Population

open MattEland.FSharpGeneticAlgorithm.Logic.States
open MattEland.FSharpGeneticAlgorithm.Logic.Simulator
open MattEland.FSharpGeneticAlgorithm.Genetics.Genes
open MattEland.FSharpGeneticAlgorithm.Logic.World

let simulateGeneration states random actors =
  actors 
  |> Seq.map (fun b -> simulate random b states) 
  |> Seq.sortByDescending (fun r -> r.totalScore)

let buildInitialPopulation random =
  Seq.init<ActorChromosome> 10 (fun _ -> getRandomChromosome random)
  
let simulateFirstGeneration states random =
  buildInitialPopulation random 
  |> simulateGeneration states random
  
let mutateBrains (random: System.Random, brains: ActorChromosome[]): ActorChromosome[] =
  if brains.Length <> 10 then failwith "Expecting exactly 10 entries"
  let survivors = [| brains.[0]; brains.[1]; |]
  let randos = [|
    getRandomChromosome random;
    getRandomChromosome random;
    getRandomChromosome random;
    getRandomChromosome random;
  |]

  let children = [| 
    createChild(random, survivors.[0].genes, survivors.[1].genes, 0.05);
    createChild(random, survivors.[0].genes, survivors.[1].genes, 0.1);
    createChild(random, survivors.[0].genes, survivors.[1].genes, 0.25);
    createChild(random, survivors.[0].genes, survivors.[1].genes, 0.5);
  |]

  Array.append children randos |> Array.append survivors

let mutateAndSimulateGeneration (random: System.Random, worlds: World[], results: SimulationResult[]) =
  let brains = Seq.map (fun b -> b.brain) results |> Seq.toArray
  mutateBrains(random, brains) |> simulateGeneration worlds random