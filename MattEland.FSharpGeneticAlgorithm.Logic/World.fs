module MattEland.FSharpGeneticAlgorithm.Logic.World

open System
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos

let getRandomPos(maxX:int32, maxY:int32, getRandom): WorldPos =
  let x = getRandom maxX
  let y = getRandom maxY
  newPos x y

let buildItemsArray (maxX:int32, maxY:int32, getRandom): Actor array =
  [| { Pos = getRandomPos(maxX, maxY, getRandom); ActorKind = Squirrel false }
     { Pos = getRandomPos(maxX, maxY, getRandom); ActorKind = Tree }
     { Pos = getRandomPos(maxX, maxY, getRandom); ActorKind = Doggo }
     { Pos = getRandomPos(maxX, maxY, getRandom); ActorKind = Acorn }
     { Pos = getRandomPos(maxX, maxY, getRandom); ActorKind = Rabbit }
  |]
let hasInvalidlyPlacedItems (items: Actor array, maxX: int32, maxY: int32): bool =
  let mutable hasIssues = false

  for itemA in items do
    // Don't allow items to spawn in corners
    if (itemA.Pos.X = 1 || itemA.Pos.X = maxX) && (itemA.Pos.Y = 1 || itemA.Pos.Y = maxY) then
      hasIssues <- true

    for itemB in items do
      if itemA <> itemB then

        // Don't allow two objects to start next to each other
        if isAdjacentTo itemA.Pos itemB.Pos then
          hasIssues <- true
    
  hasIssues

let generate (maxX:int32, maxY:int32, getRandom): Actor array =
  let mutable items: Actor array = buildItemsArray(maxX, maxY, getRandom)

  // It's possible to generate items in invalid starting configurations. Make sure we don't do that.
  while hasInvalidlyPlacedItems(items, maxX, maxY) do
    items <- buildItemsArray(maxX, maxY, getRandom)

  items

type World =
  { MaxX : int
    MaxY : int
    Squirrel : Actor
    Tree : Actor
    Doggo : Actor
    Acorn : Actor
    Rabbit : Actor }
  member this.Actors = [| this.Squirrel; this.Tree; this.Doggo; this.Acorn; this.Rabbit |]
let makeWorld maxX maxY random =
  let actors = generate(maxX, maxY, random)
  { MaxX = maxX
    MaxY = maxY
    Squirrel = actors.[0]
    Tree = actors.[1]
    Doggo = actors.[2]
    Acorn = actors.[3]
    Rabbit = actors.[4] }

let getCharacterAtCell(x, y) (world:World) =
  let actorAtCell =
    world.Actors
    |> Seq.tryFind(fun actor -> actor.Pos.X = x && actor.Pos.Y = y)

  match actorAtCell with
  | Some actor -> getChar actor
  | None -> '.'