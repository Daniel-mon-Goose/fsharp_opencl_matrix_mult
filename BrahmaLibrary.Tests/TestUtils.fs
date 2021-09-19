module BrahmaLibrary.Tests.TestUtils

open System
let randomizer = Random()

let CreateRandomMatrix rows cols  =
    Array.create (rows * cols) (float32 (randomizer.NextDouble()))