module MattEland.FSharpGeneticAlgorithm.Logic.Commands

type GameCommand =
  | MoveLeft | MoveRight | MoveUp | MoveDown
  | MoveUpLeft | MoveUpRight | MoveDownLeft | MoveDownRight
  | Wait
  | Restart
