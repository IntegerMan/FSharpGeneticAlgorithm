namespace MattEland.FSharpGeneticAlgorithm.Logic

open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.Actors

module Simulator =

  let moveActor (world: World) (actor: Actor) (xDiff: int32) (yDiff: int32): World = 
    let pos = newPos (actor.Pos.X + xDiff) (actor.Pos.Y + yDiff)

    // TODO: Check for edge of world boundaries

    // TODO: Check for collisions

    actor.Pos <- pos
    world

  // TODO: I'll need a way of simulating an actor's turn