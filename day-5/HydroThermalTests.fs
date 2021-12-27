module HydroThermalTests

open FsUnit
open Xunit

open HydroThermal

[<Fact>]
let ``Part 1: reading HydroTthermal vent coordinates from lines-test.txt, It's getting hot in here`` () =
    pathFinder nonDiagonal "lines-test.txt" |> should equal 5
    
[<Fact>]    
let ``Part 1: reading HydroTthermal vent coordinates from lines.txt, It's getting hot in here`` () =
    pathFinder nonDiagonal "lines.txt" |> should equal 8350
    
[<Fact>]
let ``Part 2: reading all HydroTthermal vent coordinates from lines-test.txt, It's getting hot in here`` () =
    pathFinder all "lines-test.txt" |> should equal 12

[<Fact>]
let ``Part 2: reading all HydroTthermal vent coordinates from lines.txt, It's getting hot in here`` () =
    pathFinder all "lines.txt" |> should equal 19374
    