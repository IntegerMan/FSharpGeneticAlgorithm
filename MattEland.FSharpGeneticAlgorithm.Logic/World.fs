module World

open Actors
open WorldPos
open System

  type World (maxX: int32, maxY: int32) = 
    member this.MaxX = maxX
    member this.MaxY = maxY
    member this.Randomizer = new Random()

    member this.randomPos: WorldPos =
      let x = this.Randomizer.Next(this.MaxX) + 1
      let y = this.Randomizer.Next(this.MaxY) + 1
      newPos x y

    member this.Generate = 
      seq {
        createSquirrel this.randomPos
      }
    
    