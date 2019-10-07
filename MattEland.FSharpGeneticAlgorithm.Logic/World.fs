module MattEland.FSharpGeneticAlgorithm.Logic.World

open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos

type World =
  { MaxX : int
    MaxY : int
    Squirrel : Actor
    Tree : Actor
    Doggo : Actor
    Acorn : Actor
    Rabbit : Actor }
  member this.Actors = [| 
    this.Squirrel; 
    this.Tree; 
    this.Doggo; 
    this.Acorn; 
    this.Rabbit 
  |]

let tryGetActor(x, y) (world:World) =
  world.Actors 
  |> Seq.tryFind(fun actor -> actor.IsActive && actor.Pos.X = x && actor.Pos.Y = y)

let getCharacterAtCell(x, y) (world:World) =
  match tryGetActor(x,y) world with
  | Some actor -> getChar actor
  | None -> '.'

let isValidPos pos world = 
  pos.X >= 1 && pos.Y >= 1 && pos.X <= world.MaxX && pos.Y <= world.MaxY

let hasObstacle pos (world: World) : bool =
  world.Actors
  |> Seq.exists(fun actor -> pos = actor.Pos)

let getPlayer world playerType =
  match playerType with 
  | Squirrel _ -> world.Squirrel
  | Rabbit _ -> world.Rabbit
  | Doggo _ -> world.Doggo
  | Tree _ -> world.Tree
  | Acorn _ -> world.Acorn
