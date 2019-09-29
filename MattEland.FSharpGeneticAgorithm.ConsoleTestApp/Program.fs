// Learn more about F# at http://fsharp.org

open Cells

[<EntryPoint>]
let main argv =
  let world = new World.World( 8, 8)
  let actors = world.Generate
  for y in 1..world.MaxX do
    for x in 1..world.MaxY do   
      // Determine which character should exist in this line
      let mutable char = '.'
      for actor in actors do
        if actor.Pos.X = x && actor.Pos.Y = y then
          char <- actor.Character

      printCell char (x = world.MaxX)

  0 // return an integer exit code
