module MattEland.FSharpGeneticAlgorithm.Genetics.Genes

  open GeneticSharp.Domain.Chromosomes
  open GeneticSharp.Domain.Fitnesses

  type SquirrelChromosome() = 
    inherit ChromosomeBase(5) // Number of genes in the chromosome

    override this.CreateNew(): IChromosome =
      new SquirrelChromosome() :> IChromosome

    override this.GenerateGene(index: int): Gene =
      new Gene(42)

  type SquirrelFitness() =

    interface IFitness with
      member this.Evaluate(chromosome: IChromosome): double =
        let domainChromosome = chromosome :?> SquirrelChromosome
        42.0

  type SquirrelPriorities =
    {
      dogImportance: double
      acornImportance: double
      rabbitImportance: double
      treeImportance: double
      selfImportance: double
      randomImportance: double
    }

  let getRandomGene (random: System.Random): double = (random.NextDouble() * 2.0) - 1.0

  let getRandomPriorities (random: System.Random) = 
    {
      dogImportance = getRandomGene random;
      acornImportance = getRandomGene random;
      rabbitImportance = getRandomGene random;
      treeImportance = getRandomGene random;
      selfImportance = getRandomGene random;
      randomImportance = getRandomGene random;
    }