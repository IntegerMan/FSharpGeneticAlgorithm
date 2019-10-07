module MattEland.FSharpGeneticAlgorithm.Logic.Actors

open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos

type ActorKind =
  | Squirrel of hasAcorn:bool
  | Tree
  | Acorn
  | Rabbit
  | Doggo

type Actor =
  { Pos : WorldPos
    ActorKind : ActorKind }

let getChar actor =
  match actor.ActorKind with
  | Squirrel hasAcorn -> match hasAcorn with | true -> 'S' | false -> 's'
  | Tree _ -> 't'
  | Acorn _ -> 'a'
  | Rabbit _ -> 'R'
  | Doggo _ -> 'D'