module BrahmaLibrary.MatrixMultiply

open Brahma.OpenCL
open Brahma.FSharp.OpenCL.Core
open Brahma.FSharp.OpenCL.Extensions

open ContextOpenCL

let GetOutputMatrixDimensions aRows aCols bRows bCols =
    if aCols <> bRows then failwith "Cannot multiply these two matrices"
    aCols, bRows
    
let MultiplyMatricesViaCPU (a : array<_>) (aRows: int) (aCols: int)
                           (b : array<_>) (bRows: int) (bCols: int) =
    let resultRows, resultCols = GetOutputMatrixDimensions aRows aCols bRows bCols
    let result = Array.create (resultRows * resultCols) 0.0f
    
    for i in 0 .. resultRows - 1 do
        for j in 0 .. resultCols - 1 do
            let mutable buf = 0.0f
            for k in 0 .. aCols - 1 do
                 buf <- buf + a.[i * aCols + k] * b.[k * bCols + j]
            result.[i * resultCols + j] <- result.[i * resultCols + j] + buf
            
    result
    
let MultiplyMatricesViaGPU (provider: ComputeProvider)
                     (firstMatrix: float32 array) (secondMatrix: float32 array) (size: int): float32 array =
    let result = Array.create (size * size) 0.0f
    let localWorkSize = 2
        
    let mutable commandQueue = CreateCommandQueue provider
    let kernelCode = MatrixKernels.matrixMultiplyKernel
    
    let kernel, kernelPrepare, kernelRun = provider.Compile kernelCode
    let gpuWorkSize = _2D(size, size, localWorkSize, localWorkSize)
    kernelPrepare gpuWorkSize firstMatrix secondMatrix result size
    ignore <| commandQueue.Add(kernelRun()).Finish()
    
    let _ = commandQueue.Add(result.ToHost provider).Finish()
    
    commandQueue.Dispose()
    provider.CloseAllBuffers()
    result