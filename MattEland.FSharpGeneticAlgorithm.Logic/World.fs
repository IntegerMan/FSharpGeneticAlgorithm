namespace MattEland.FSharpGeneticAlgorithm.Logic

open System
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos

module World =

  let getRandomPos(maxX:int32, maxY:int32, random: Random): WorldPos =
    let x = random.Next(maxX) + 1
    let y = random.Next(maxY) + 1
    newPos x y

  let buildItemsArray (maxX:int32, maxY:int32, random: Random): Actor array =
    [|
      createSquirrel (getRandomPos(maxX, maxY, random))
      createTree (getRandomPos(maxX, maxY, random))
      createDoggo (getRandomPos(maxX, maxY, random))
      createAcorn (getRandomPos(maxX, maxY, random))
      createRabbit (getRandomPos(maxX, maxY, random))
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

  let generate (maxX:int32, maxY:int32, random: Random): Actor array =
    let mutable items: Actor array = buildItemsArray(maxX, maxY, random)

    // It's possible to generate items in invalid starting configurations. Make sure we don't do that.
    while hasInvalidlyPlacedItems(items, maxX, maxY) do
      items <- buildItemsArray(maxX, maxY, random)

    items

  type World (maxX: int32, maxY: int32, random: Random) = 
    let actors = generate(maxX, maxY, random)
    member this.Actors = actors
    member this.MaxX = maxX
    member this.MaxY = maxY

    member this.GetCharacterAtCell(x, y) =
      let mutable char = '.'
      for actor in this.Actors do
        if actor.Pos.X = x && actor.Pos.Y = y then
          char <- actor.Character
      char