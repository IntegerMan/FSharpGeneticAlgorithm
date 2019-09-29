module World

open Actors

  let generate: Actor seq = 
    seq {
      createSquirrel 4 2
    }
    
    