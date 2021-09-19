module BrahmaLibrary.MatrixKernels

open Brahma.OpenCL

let matrixMultiplyKernel =
    <@
        fun (r:_2D) (a:array<float32>) (b:array<float32>) (c:array<float32>) (size: int) ->
            let xGlobal = r.GlobalID0
            let yGlobal = r.GlobalID1
            
            let mutable buffer = c.[yGlobal * size + xGlobal]
            for k in 0 .. size - 1 do
                buffer <- buffer + a.[yGlobal * size + k] * b.[k * size + xGlobal]
                
            c.[yGlobal * size + xGlobal] <- buffer
    @>