module BrahmaLibrary.ContextOpenCL

open Brahma.OpenCL

let CreateProvider platformName deviceType =
    try ComputeProvider.Create(platformName, deviceType)
        with 
        | ex -> failwith ex.Message
        
let CreateCommandQueue provider = new CommandQueue(provider, Seq.head <| provider.Devices)