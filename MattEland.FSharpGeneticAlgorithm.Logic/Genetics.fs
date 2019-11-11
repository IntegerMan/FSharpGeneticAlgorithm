module MattEland.FSharpGeneticAlgorithm.Genetics.Genes

  open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
  open MattEland.FSharpGeneticAlgorithm.Logic.Actors
  open MattEland.FSharpGeneticAlgorithm.Logic.World
  open MattEland.FSharpGeneticAlgorithm.Logic.States

  type ActorChromosome =
    {
      dogImportance: double
      acornImportance: double
      rabbitImportance: double
      treeImportance: double
      squirrelImportance: double
      randomImportance: double
    }

  let getRandomGene (random: System.Random) = (random.NextDouble() * 2.0) - 1.0

  let getRandomChromosome (random: System.Random) = 
    {
      dogImportance = getRandomGene random;
      acornImportance = getRandomGene random;
      rabbitImportance = getRandomGene random;
      treeImportance = getRandomGene random;
      squirrelImportance = getRandomGene random;
      randomImportance = getRandomGene random;
    }

  let getChildGene (random: System.Random, value1, value2, mutationChance) =
    let value = match random.Next(2) with
    | 0 -> value1
    | _ -> value2

    // TODO: Mutate

    // TODO: Constrain
    value

  let createChild (random: System.Random, parent1: ActorChromosome, parent2: ActorChromosome, mutationChance: float) =
    {
      dogImportance = getChildGene(random, parent1.dogImportance, parent2.dogImportance, mutationChance);
      acornImportance = getChildGene(random, parent1.acornImportance, parent2.acornImportance, mutationChance);
      rabbitImportance = getChildGene(random, parent1.rabbitImportance, parent2.rabbitImportance, mutationChance);
      treeImportance = getChildGene(random, parent1.treeImportance, parent2.treeImportance, mutationChance);
      squirrelImportance = getChildGene(random, parent1.squirrelImportance, parent2.squirrelImportance, mutationChance);
      randomImportance = getChildGene(random, parent1.randomImportance, parent2.randomImportance, mutationChance);
    }
    

  let evaluateProximity actor pos weight =
    if actor.IsActive then
      getDistance(actor.Pos, pos) * weight
    else
      0.0

  let evaluateTile brain world pos (random: System.Random) =
    evaluateProximity world.Squirrel pos brain.squirrelImportance + 
    evaluateProximity world.Rabbit pos brain.rabbitImportance + 
    evaluateProximity world.Doggo pos brain.dogImportance + 
    evaluateProximity world.Acorn pos brain.acornImportance + 
    evaluateProximity world.Tree pos brain.treeImportance + 
    (random.NextDouble() * brain.randomImportance)

  type SimulationResult = {
    score: float
    states: GameState[]
    brain: ActorChromosome
  }