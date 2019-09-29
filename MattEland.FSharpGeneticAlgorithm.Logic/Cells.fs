module Cells

open Actors

  let printCell char isLastCell =
    if isLastCell then
      printfn "%c" char
    else
      printf "%c" char

  let getCellCharacter (x: int32) (y: int32) (actors: Actor seq): char =
    let mutable char = '.'
    for actor in actors do
      if actor.Pos.X = x && actor.Pos.Y = y then
        char <- actor.Character
    char
