using System;
using JetBrains.Annotations;
using MattEland.FSharpGeneticAlgorithm.Genetics;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient.ViewModels
{
    public class BrainInfoViewModel
    {
        public BrainInfoViewModel([NotNull] Genes.ActorChromosome brain)
        {
            Model = brain ?? throw new ArgumentNullException(nameof(brain));
        }

        private double GetGene(Genes.ActorGeneIndex actor) => Genes.getGene(actor, Model.genes);

        public double SquirrelPriority => GetGene(Genes.ActorGeneIndex.Squirrel);
        public double DoggoPriority => GetGene(Genes.ActorGeneIndex.Doggo);
        public double RabbitPriority => GetGene(Genes.ActorGeneIndex.Rabbit);
        public double AcornPriority => GetGene(Genes.ActorGeneIndex.Acorn);
        public double TreePriority => GetGene(Genes.ActorGeneIndex.Tree);
        public double RandomPriority => GetGene(Genes.ActorGeneIndex.Random);

        public Genes.ActorChromosome Model { get; }
    }
}

