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

        public double SquirrelPriority => Model.squirrelImportance;
        public double DoggoPriority => Model.dogImportance;
        public double RabbitPriority => Model.rabbitImportance;
        public double AcornPriority => Model.acornImportance;
        public double TreePriority => Model.treeImportance;
        public double RandomPriority => Model.randomImportance;
        public Genes.ActorChromosome Model { get; }
        public int Id => Model.id;
    }
}