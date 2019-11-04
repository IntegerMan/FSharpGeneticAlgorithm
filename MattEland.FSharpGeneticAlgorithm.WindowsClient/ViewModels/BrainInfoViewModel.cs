using System;
using JetBrains.Annotations;
using MattEland.FSharpGeneticAlgorithm.Genetics;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient.ViewModels
{
    public class BrainInfoViewModel
    {
        private readonly Genes.SquirrelPriorities _brain;

        public BrainInfoViewModel([NotNull] Genes.SquirrelPriorities brain)
        {
            _brain = brain ?? throw new ArgumentNullException(nameof(brain));
        }

        public double SquirrelPriority => _brain.selfImportance;
        public double DoggoPriority => _brain.dogImportance;
        public double RabbitPriority => _brain.rabbitImportance;
        public double AcornPriority => _brain.acornImportance;
        public double TreePriority => _brain.treeImportance;
        public double RandomPriority => _brain.randomImportance;
        public Genes.SquirrelPriorities Model => _brain;
    }
}