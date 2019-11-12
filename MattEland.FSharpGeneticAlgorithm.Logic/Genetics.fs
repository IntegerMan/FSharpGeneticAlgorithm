module MattEland.FSharpGeneticAlgorithm.Genetics.Genes

  open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
  open MattEland.FSharpGeneticAlgorithm.Logic.Actors
  open MattEland.FSharpGeneticAlgorithm.Logic.World
  open MattEland.FSharpGeneticAlgorithm.Logic.States

  type ActorGeneIndex = Doggo = 0| Acorn = 1| Rabbit = 2| Tree = 3| Squirrel = 4 | Random = 5

  type ActorChromosome =
    {
      genes: double[]
    }

  let getRandomGene (random: System.Random) = (random.NextDouble() * 2.0) - 1.0

  let getRandomChromosome (random: System.Random) = 
    {
      genes = Seq.init 6 (fun _ -> getRandomGene random) |> Seq.toArray
    }

  let getChildGene (random: System.Random, value1, value2, mutationChance) =
    let value = match random.Next(2) with
    | 0 -> value1
    | _ -> value2

    // TODO: Mutate

    // TODO: Constrain
    value

  let createChild (random: System.Random, parent1: ActorChromosome, parent2: ActorChromosome, mutationChance: float) =
    let genes = Seq.map2 (fun m f -> getChildGene(random, m, f, mutationChance)) parent1.genes parent2.genes
    {
      genes = Seq.toArray genes
    }
    

  let evaluateProximity actor pos weight =
    if actor.IsActive then
      getDistance(actor.Pos, pos) * weight
    else
      0.0

  let getGene (geneIndex: ActorGeneIndex) (genes: double[]) =
    genes.[int geneIndex]

  let evaluateTile brain world pos (random: System.Random) =
    let genes = brain.genes
    evaluateProximity world.Squirrel pos (getGene ActorGeneIndex.Squirrel genes) + 
    evaluateProximity world.Rabbit pos (getGene ActorGeneIndex.Rabbit genes) + 
    evaluateProximity world.Doggo pos (getGene ActorGeneIndex.Doggo genes) + 
    evaluateProximity world.Acorn pos (getGene ActorGeneIndex.Acorn genes) + 
    evaluateProximity world.Tree pos (getGene ActorGeneIndex.Tree genes) + 
    (random.NextDouble() * (getGene ActorGeneIndex.Random genes))

  type SimulationResult = {
    score: float
    states: GameState[]
    brain: ActorChromosome
  }