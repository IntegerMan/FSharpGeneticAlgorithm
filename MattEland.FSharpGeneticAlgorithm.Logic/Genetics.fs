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

  let mutate (random: System.Random, magnitude, value) =
    (value + (random.NextDouble() * magnitude))
    |> max -1.0 |> min 1.0

  let mutateGenes (random: System.Random) mutationChance genes = 
    Array.map (fun g -> if random.NextDouble() <= mutationChance then
                          mutate(random, 0.5, g)
                        else
                          g
              ) genes

  let getChildGenes (random: System.Random) parent1 parent2 mutationChance =

    // Map from one parent to another, choosing a point to switch from one parent as the source
    // to the other. Being an identical copy to either parent is also possible
    let crossoverIndex = random.Next(Array.length parent1 + 1)

    Array.mapi2 (fun i m f -> if i <= crossoverIndex then
                                m
                              else
                                f
                ) parent1 parent2
    // Next allow each gene to be potentially mutated
    |> mutateGenes random mutationChance

  let createChild (random: System.Random, parent1: double[], parent2: double[], mutationChance: float) =

    let genes = getChildGenes random parent1 parent2 mutationChance
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

  type IndividualWorldResult = {
    score: float
    states: GameState[]
  }

  type SimulationResult = {
    totalScore: float
    results: IndividualWorldResult[]
    brain: ActorChromosome
  }