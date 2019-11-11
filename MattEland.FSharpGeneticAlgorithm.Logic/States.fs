module MattEland.FSharpGeneticAlgorithm.Logic.States

open MattEland.FSharpGeneticAlgorithm.Logic.World

type SimulationState = Simulating=0 | Won=1 | Lost=2

type GameState = { World : World; SimState: SimulationState; TurnsLeft: int}
