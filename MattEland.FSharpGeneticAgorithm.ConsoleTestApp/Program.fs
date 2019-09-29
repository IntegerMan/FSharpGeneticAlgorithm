open System
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.Cells

[<EntryPoint>]
let main argv =
  let randomizer = new Random()
  let world = new World(8, 8, randomizer)
  world.Generate |> ignore

  for y in 1..world.MaxX do
    for x in 1..world.MaxY do   
      // Determine which character should exist in this line
      let char = world.GetCharacterAtCell(x,y)
      printCell char (x = world.MaxX)

  0 // return an integer exit code
