module MattEland.FSharpGeneticAlgorithm.Genetics.Genes

  open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
  open MattEland.FSharpGeneticAlgorithm.Logic.Actors
  open MattEland.FSharpGeneticAlgorithm.Logic.World

  type ActorChromosome =
    {
      dogImportance: double
      acornImportance: double
      rabbitImportance: double
      treeImportance: double
      squirrelImportance: double
      randomImportance: double
    }

  let getRandomGene (random: System.Random): double = (random.NextDouble() * 2.0) - 1.0

  let getRandomChromosome (random: System.Random) = 
    {
      dogImportance = getRandomGene random;
      acornImportance = getRandomGene random;
      rabbitImportance = getRandomGene random;
      treeImportance = getRandomGene random;
      squirrelImportance = getRandomGene random;
      randomImportance = getRandomGene random;
    }

  let evaluateProximity(actor: Actor, pos:WorldPos, weight: float): float =
    if actor.IsActive then
      getDistance(actor.Pos, pos) * weight
    else
      0.0

  let evaluateTile(brain: ActorChromosome, world: World, pos: WorldPos, random: System.Random): float =
    evaluateProximity(world.Squirrel, pos, brain.squirrelImportance) + 
    evaluateProximity(world.Rabbit, pos, brain.rabbitImportance) + 
    evaluateProximity(world.Doggo, pos, brain.dogImportance) + 
    evaluateProximity(world.Acorn, pos, brain.acornImportance) + 
    evaluateProximity(world.Tree, pos, brain.treeImportance) + 
    (random.NextDouble() * brain.randomImportance)