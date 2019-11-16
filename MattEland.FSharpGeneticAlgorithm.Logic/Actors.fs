module MattEland.FSharpGeneticAlgorithm.Logic.Actors

open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos

type ActorKind =
  | Squirrel of hasAcorn:bool
  | Tree
  | Acorn
  | Rabbit
  | Doggo

type Actor =
  { Pos : WorldPos;
    ActorKind : ActorKind;
    IsActive : bool
  }