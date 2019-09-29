namespace MattEland.FSharpGeneticAlgorithm.Logic

open System
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos

module World =

  type World (maxX: int32, maxY: int32, random: Random) = 
  
    let getRandomPos: WorldPos =
      let x = random.Next(maxX) + 1
      let y = random.Next(maxY) + 1
      newPos x y

    let mutable actors: Actor seq = Seq.empty
    member this.Actors = actors

    member this.GetCharacterAtCell(x, y) =
      let mutable char = '.'
      for actor in this.Actors do
        if actor.Pos.X = x && actor.Pos.Y = y then
          char <- actor.Character
      char

    member this.MaxX = maxX
    member this.MaxY = maxY

    member this.Generate: Actor seq =
      actors <- Seq.empty
      actors <- Seq.append actors [createSquirrel getRandomPos]
      actors

         
    