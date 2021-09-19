module BrahmaLibrary.Tests.MatrixTests

open BrahmaLibrary
open OpenCL.Net
open TestUtils
open Xunit

open ContextOpenCL
open MatrixMultiply

let private CompareCPUAndGPU provider matrixSize =
    let firstMatrix = CreateRandomMatrix matrixSize matrixSize
    let secondMatrix = CreateRandomMatrix matrixSize matrixSize
    
    let resultCPU = MultiplyMatricesViaCPU firstMatrix matrixSize matrixSize secondMatrix matrixSize matrixSize
    
    let resultGPU = MultiplyMatricesViaGPU provider firstMatrix secondMatrix matrixSize
    
    for i in 0 .. matrixSize * matrixSize - 1 do
        Assert.False(System.Math.Abs(float32 (resultGPU.[i] - resultCPU.[i])) > 0.01f,
                     $"GPU {resultGPU.[i]} and CPU {resultCPU.[i]} of index {i} differ more than 0.01")
    
[<Fact>]
let ``Compare CPU with GPU matrices computation``(): unit =
    let provider = CreateProvider "*" DeviceType.Default
    CompareCPUAndGPU provider 8
    CompareCPUAndGPU provider 10
    CompareCPUAndGPU provider 128
    CompareCPUAndGPU provider 300
    CompareCPUAndGPU provider 600
    CompareCPUAndGPU provider 1000
    provider.Dispose()
