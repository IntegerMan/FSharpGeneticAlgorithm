namespace MattEland.FSharpGeneticAlgorithm.Logic

open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.Actors

module Simulator =

  let isValidPos pos (world: World): bool = 
    pos.X >= 1 && pos.Y >= 1 && pos.X <= world.MaxX && pos.Y <= world.MaxY

  let hasObstacle pos (world: World): bool = 
    let mutable obstructed = false
    for actor in world.Actors do
      if isSamePos pos actor.Pos then
        obstructed <- true
    obstructed

  let moveActor (world: World) (actor: Actor) (xDiff: int32) (yDiff: int32): World = 
    let pos = newPos (actor.Pos.X + xDiff) (actor.Pos.Y + yDiff)

    if (isValidPos pos world) && not (hasObstacle pos world) then
      actor.Pos <- pos

    world

  // TODO: I'll need a way of simulating an actor's turn