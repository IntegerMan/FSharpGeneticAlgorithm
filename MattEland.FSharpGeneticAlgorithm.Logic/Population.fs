module MattEland.FSharpGeneticAlgorithm.Genetics.Population

open MattEland.FSharpGeneticAlgorithm.Logic.Simulator
open MattEland.FSharpGeneticAlgorithm.Genetics.Genes

let simulateGeneration random actors =
  let initialState = buildStartingState random
  actors |> Seq.map (fun b -> simulate random b initialState) |> Seq.sortByDescending (fun r -> r.score)

let buildInitialPopulation random =
  seq {
    for id in 1 .. 10 do
      let brain = getRandomChromosome random
      yield brain
  }
  
let simulateFirstGeneration random =
  buildInitialPopulation random 
  |> simulateGeneration random
  
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

let mutateAndSimulateGeneration (random: System.Random, results: SimulationResult[]) =
  let brains = Seq.map (fun b -> b.brain) results |> Seq.toArray
  mutateBrains(random, brains) |> simulateGeneration random
 