module MattEland.FSharpGeneticAlgorithm.Logic.WorldGeneration

open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.World

let hasInvalidlyPlacedItems (actors: Actor array, maxX: int32, maxY: int32): bool =
  let mutable hasIssues = false

  for actA in actors do
    // Don't allow items to spawn in corners
    if (actA.Pos.X = 1 || actA.Pos.X = maxX) && (actA.Pos.Y = 1 || actA.Pos.Y = maxY) then
      hasIssues <- true

    for actB in actors do
      if actA <> actB then

        // Don't allow two objects to start next to each other
        if isAdjacentTo actA.Pos actB.Pos then
          hasIssues <- true
    
  hasIssues

let buildActors (maxX:int32, maxY:int32, getRandom): Actor array =
  [|  { Pos = getRandomPos(maxX, maxY, getRandom); ActorKind = Squirrel false }
      { Pos = getRandomPos(maxX, maxY, getRandom); ActorKind = Tree }
      { Pos = getRandomPos(maxX, maxY, getRandom); ActorKind = Doggo }
      { Pos = getRandomPos(maxX, maxY, getRandom); ActorKind = Acorn }
      { Pos = getRandomPos(maxX, maxY, getRandom); ActorKind = Rabbit }
  |]

let generate (maxX:int32, maxY:int32, getRandom): Actor array =
  let mutable items: Actor array = buildActors(maxX, maxY, getRandom)

  // It's possible to generate items in invalid starting configurations. Make sure we don't do that.
  while hasInvalidlyPlacedItems(items, maxX, maxY) do
    items <- buildActors(maxX, maxY, getRandom)

  items

let makeWorld maxX maxY random =
  let actors = generate(maxX, maxY, random)
  { MaxX = maxX
    MaxY = maxY
    Squirrel = actors.[0]
    Tree = actors.[1]
    Doggo = actors.[2]
    Acorn = actors.[3]
    Rabbit = actors.[4] }